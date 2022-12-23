using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBoundary : MonoBehaviour
{
    public static GameObject baseFloor;
    public static float leftSide;
    public static float rightSide;

    private void Awake()
    {
        if (GameManager.Instance.state == GameManager.GameState.Game)
        {
            baseFloor = GameObject.Find("SandFloor");
            leftSide = -(baseFloor.transform.localScale.x - 1) / 2;
            rightSide = (baseFloor.transform.localScale.x - 1) / 2;
        }
    }
}
