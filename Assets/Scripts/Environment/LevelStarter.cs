using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStarter : MonoBehaviour
{
    public GameObject[] countDown;
    public GameObject backDrop;
    public AudioSource goFX;
    public AudioSource readyFX;

    IEnumerator CountSequence()
    {
        backDrop.GetComponent<Animator>().SetTrigger("FadeIn");
        yield return new WaitForSeconds(1.5f);
        countDown[0].SetActive(true);
        readyFX.Play();

        yield return new WaitForSeconds(1f);
        countDown[0].SetActive(false);
        countDown[1].SetActive(true);
        readyFX.Play();

        yield return new WaitForSeconds(1f);
        countDown[1].SetActive(false);
        countDown[2].SetActive(true);
        readyFX.Play();

        yield return new WaitForSeconds(1f);
        countDown[2].SetActive(false);
        countDown[3].SetActive(true);
        goFX.Play();

        yield return new WaitForSeconds(1f);
        countDown[3].SetActive(false);
        backDrop.SetActive(false);

        GameManager.Instance.StartTracking();
    }

    void Start()
    {
        if (GameManager.Instance.state == GameManager.GameState.Game)
            StartCoroutine(CountSequence());
    }
}
