using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushAndRotate : MonoBehaviour
{
    public float rotationSpeed = 20f;

    void Update()
    {
        float rotation = Input.GetAxis("Mouse ScrollWheel");
        transform.Rotate(Vector3.forward, rotation * rotationSpeed);
    }
}
