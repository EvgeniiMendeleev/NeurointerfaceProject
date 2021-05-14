using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Threading;

namespace Neurointerface
{
    class NeuroDevice
    {
        const char EXCODE = '\x55', SYNC = '\xAA', ASIC_EEG_POWER_INT = '\x83', POOR_SIGNAL = '\x02', ATTENTION = '\x04', MEDITATION = '\x05', RAW_WAVE_VALUE = '\x80';

        static Mutex mtx = new Mutex();
        private Dictionary<string, int> currentBrainData = new Dictionary<string, int>();
        private SerialPort port = new SerialPort("COM5", 57600);

        public NeuroDevice(string COM)
        {
            port.PortName = COM;
            port.BaudRate = 57600;
            port.Parity = Parity.None;
            port.DataBits = 8;
            port.StopBits = StopBits.One;
            port.ReadTimeout = 12000;
        }

        public void OpenConnection()
        {
            port.Open();
            InitDictionaryWithBrainsData();
            new Thread(ReadingThread).Start();
        }

        public int GetBrainDataAbout(string brainWave)
        {
            mtx.WaitOne() ;
            int returnData = currentBrainData.Count > 0 && currentBrainData.ContainsKey(brainWave) ? currentBrainData[brainWave] : -1;
            mtx.ReleaseMutex();

            return returnData;
        }

        public bool isConnection()
        {
            return port.IsOpen;
        }

        public void CloseConnection()
        {
            if(port.IsOpen) port.Close();
        }

        private void InitDictionaryWithBrainsData()
        {
            List<string> brainWaves = new List<string>()
            {
                "attention", "meditation", "poor_signal", "raw_wave_value", "delta", "theta", "low-alpha", "high-alpha", "low-beta", "high-beta", "low-gamma", "mid-gamma"
            };

            foreach (string wave in brainWaves) currentBrainData.Add(wave, 0);
        }

        private void ReadingThread()
        {
            try
            {
                while (true)
                {
                    var dataFromPort = ReadPayloadFromPort();
                    if (dataFromPort.isReadPayload) ParsingPayload(dataFromPort.payload);
                }
            }
            catch (Exception exp)
            {
                Console.WriteLine($"[Reading thread]: {exp.Message}");
            }

            CloseConnection();
        }

        private (bool isReadPayload, List<int> payload) ReadPayloadFromPort()
        {
            WaitOnPortTwoSync();
            ReadPayloadLength(out int pLength);
            if (pLength > SYNC) return (false, null);
            var payloadResult = ReadPayload(pLength);
            int readedChecksum = port.ReadByte();
            if (readedChecksum != payloadResult.calculatedChecksum) return (false, null);

            return (true, payloadResult.payload);
        }
        
        private void WaitOnPortTwoSync()
        {
            int firstByte = port.ReadByte();
            int secondByte = port.ReadByte();

            while (firstByte != SYNC || secondByte != SYNC)
            {
                firstByte = secondByte;
                secondByte = port.ReadByte();
            }
        }

        private void ReadPayloadLength(out int pLength)
        {
            do
            {
                pLength = port.ReadByte();
            } while (pLength == SYNC);
        }

        private (int calculatedChecksum, List<int> payload) ReadPayload(in int pLength)
        {
            var funcResult = (calculatedChecksum: 0, payload: new List<int>());

            for (int i = 0; i < pLength; i++)
            {
                int dataFromPort = port.ReadByte();

                funcResult.payload.Add(dataFromPort);
                funcResult.calculatedChecksum += dataFromPort;
            }

            funcResult.calculatedChecksum &= 0xFF;
            funcResult.calculatedChecksum = ~funcResult.calculatedChecksum & 0xFF;

            return funcResult;
        }
        
        private void ParsingPayload(in List<int> payload)
        {
            int bytesParsed = 0;
            while (bytesParsed < payload.Count)
            {
                while (payload[bytesParsed] == EXCODE) bytesParsed++;

                int code = payload[bytesParsed++];
                int vLength = Convert.ToBoolean(code & RAW_WAVE_VALUE) ? payload[bytesParsed++] : 1;

                if (code == ASIC_EEG_POWER_INT) SaveBrainDataAboutASIG(payload, bytesParsed);
                else if (code == MEDITATION) SaveBrainDataAbout("meditation", payload[bytesParsed]);
                else if (code == ATTENTION) SaveBrainDataAbout("attention", payload[bytesParsed]);
                else if (code == POOR_SIGNAL) SaveBrainDataAbout("poor_signal", payload[bytesParsed]);
                else if (code == RAW_WAVE_VALUE) SaveBrainDataAbout("raw_wave_value", (payload[bytesParsed] << 8) | payload[bytesParsed + 1]);

                bytesParsed += vLength;
            }
        }

        private void SaveBrainDataAboutASIG(in List<int> payload, int bytesParsed)
        {
            List<string> minadWavesNames = new List<string>() { "delta", "theta", "low-alpha", "high-alpha", "low-beta", "high-beta", "low-gamma", "mid-gamma"};
            foreach(string mindWave in minadWavesNames)
            {
                SaveBrainDataAbout(mindWave, payload[bytesParsed] * 65536 + payload[bytesParsed + 1] * 256 + payload[bytesParsed + 2]);
                bytesParsed += 3;
            }
        }

        private void SaveBrainDataAbout(in string brainWave, in int data)
        {
            mtx.WaitOne();
            currentBrainData[brainWave] = data;
            mtx.ReleaseMutex();
        }
    }
}
