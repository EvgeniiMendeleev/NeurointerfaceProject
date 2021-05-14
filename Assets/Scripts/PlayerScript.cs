using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Neurointerface;

public class PlayerScript : MonoBehaviour
{
    private NeuroDevice device = new NeuroDevice("COM5");
    [Range(0,100)]
    public int level;
    void Start()
    {
        //while (!device.isConnection()) device.OpenConnection();
    }

    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        Debug.DrawRay(transform.position, transform.forward * 100.0f, Color.green);
        //Debug.Log($"Start: {transform.position}, Forward: {transform.forward}");
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            GameObject obj = hit.collider.gameObject;
            Debug.Log(obj.name);

            if (obj.TryGetComponent<AngryObject>(out AngryObject angryObject))
            {
                angryObject.GetAngry(level);
            }
        }
    }
}
