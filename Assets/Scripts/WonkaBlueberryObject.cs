using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WonkaBlueberryObject : MonoBehaviour
{

    void Update()
    {
        gameObject.GetComponent<Renderer>().material.color = Color.white;
    }

    public void SwellUp(int level)
    {
        if (level > 80)
        {
            gameObject.GetComponent<Renderer>().material.color = Color.blue;
            gameObject.transform.localScale *= 1.002f;
        }
        else
        {
            gameObject.transform.localScale /= 1.002f;
        }
    }
}
