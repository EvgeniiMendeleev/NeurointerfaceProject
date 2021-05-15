using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Neurointerface;

public class PlayerScript : MonoBehaviour
{
    private NeuroDevice device = new NeuroDevice("COM5");
    [Range(0,100)] public int level;
    [SerializeField] private Text levelMindText;
    void Start()
    {
        //while (!device.isConnection()) device.OpenConnection();
    }

    void LateUpdate()
    {
        levelMindText.text = $"{level.ToString()} %";
        Ray ray = new Ray(transform.position, transform.forward);
        Debug.DrawRay(transform.position, transform.forward * 100.0f, Color.green);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            GameObject obj = hit.collider.gameObject;

            if (obj.TryGetComponent<AngryObject>(out AngryObject angryObject)) angryObject.GetAngry(level);
            else if (obj.TryGetComponent<JumpingObject>(out JumpingObject jumpingObject)) jumpingObject.GetUp(level);
            else if (obj.TryGetComponent<SelectableObject>(out SelectableObject selectableObject)) selectableObject.TakeObject(transform.position, transform.forward, level);
            else if (obj.TryGetComponent<HidingObject>(out HidingObject hidingObject)) hidingObject.Hide(transform.position, transform.forward, level);
            else if (obj.TryGetComponent<WonkaBlueberryObject>(out WonkaBlueberryObject wonkaObject)) wonkaObject.SwellUp(level);
            else if (obj.TryGetComponent<RotatingObject>(out RotatingObject rotatingObject)) rotatingObject.RotateObject(level);
        }
    }
}
