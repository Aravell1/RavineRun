using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DistanceTracking : MonoBehaviour
{
    public float position;

    // Start is called before the first frame update
    void Start()
    {
        position = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.canMove)
            position += GameManager.Instance.moveSpeed * Time.deltaTime;
    }
}
