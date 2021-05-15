using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngryObject : MonoBehaviour
{
    void Update()
    {
        gameObject.GetComponent<Renderer>().material.color = Color.green;
    }

    public void GetAngry(int level)
    {
        if (level >= 50 && level < 70) gameObject.GetComponent<Renderer>().material.color = Color.yellow;
        else if (level >= 70 && level < 90) gameObject.GetComponent<Renderer>().material.color = Color.red;
        else if(level >= 90) Destroy(gameObject);
    }
}
