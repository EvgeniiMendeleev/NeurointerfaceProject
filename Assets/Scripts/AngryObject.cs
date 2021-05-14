using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngryObject : MonoBehaviour
{
    void Start()
    {
        gameObject.GetComponent<Renderer>().material.color = Color.green;
    }

    public void GetAngry(int level)
    {
        if (level < 50) gameObject.GetComponent<Renderer>().material.color = Color.green;
        else if (level >= 50 && level < 70) gameObject.GetComponent<Renderer>().material.color = Color.yellow;
        else if (level >= 70 && level < 90) gameObject.GetComponent<Renderer>().material.color = Color.red;
        else Destroy(gameObject);
    }
}
