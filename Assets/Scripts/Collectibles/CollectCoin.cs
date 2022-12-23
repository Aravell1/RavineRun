using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectCoin : MonoBehaviour
{
    private AudioSource coinFX;

    private void Start()
    {
        coinFX = GameObject.Find("CoinCollect").GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            coinFX.Play();
            GameManager.Instance.Score++;

            Destroy(gameObject);
        }
    }

}
