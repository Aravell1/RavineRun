using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainMovement : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.canMove)
            transform.Translate(GameManager.Instance.moveSpeed * Time.deltaTime * Vector3.back, Space.World);
    }
}
