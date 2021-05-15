using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingObject : MonoBehaviour
{
    [SerializeField] float rotateSpeed = 0.04f;
    public void RotateObject(int level)
    {
        gameObject.transform.Rotate(Vector3.right, level * rotateSpeed);
    }
}
