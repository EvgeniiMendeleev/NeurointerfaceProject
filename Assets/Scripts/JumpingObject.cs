using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingObject : MonoBehaviour
{
    public void GetUp(int level)
    {
        if (level > 50)
        {
            gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up, ForceMode.Impulse);
        }
    }
}
