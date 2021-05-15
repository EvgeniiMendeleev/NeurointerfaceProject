using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidingObject : MonoBehaviour
{
    public void Hide(Vector3 playerPosition, Vector3 forwardVector, int level)
    {
        if (level > 80)
        {
            gameObject.transform.position = playerPosition - forwardVector * 2.0f;
        }
    }
}
