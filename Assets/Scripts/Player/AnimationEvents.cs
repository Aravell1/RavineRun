using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    
    public void OnStumbleEnd()
    {
        GetComponentInParent<PlayerMove>().SetCanMove(true);
    }
}
