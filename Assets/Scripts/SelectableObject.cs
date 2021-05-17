using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableObject : MonoBehaviour
{
    [SerializeField] 
    private float distance = 5.0f;

    public void TakeObject(Vector3 playerPosition, Vector3 forwardVector, int level)
    {
        if (level > 50)
        {
            gameObject.transform.position = playerPosition + forwardVector * distance;
        }
    }
}
