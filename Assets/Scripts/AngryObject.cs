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
        if (level >= 20 && level < 40) gameObject.GetComponent<Renderer>().material.color = Color.yellow;
        else if (level >= 40 && level < 60) gameObject.GetComponent<Renderer>().material.color = Color.red;
        else if(level >= 70) Destroy(gameObject);
    }
}
