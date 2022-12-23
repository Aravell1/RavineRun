using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    private float rotationSpeed = 180;

    void Update()
    {
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0, Space.World);
    }
}
