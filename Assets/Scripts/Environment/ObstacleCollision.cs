using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleCollision : MonoBehaviour
{
    public AudioSource stumbleFX;
    public Camera mainCamera;

    private void Start()
    {
        stumbleFX = GameObject.Find("Stumble").GetComponent<AudioSource>();
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GetComponent<Collider>().enabled = false;
            other.gameObject.GetComponent<PlayerMove>().Stumble();
            stumbleFX.Play();
            mainCamera.GetComponent<Animator>().SetTrigger("Shake");
            GameManager.Instance.Health--;
        }
    }
}
