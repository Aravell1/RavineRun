using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class CanvasManager : MonoBehaviour
{
    [Header("Text")]
    public TMP_Text scoreText;
    public TMP_Text distanceText;
    public TMP_Text healthText;
    public TMP_Text highScoreText;
    public TMP_Text coinsText;
    public TMP_Text healthCountText;
    public TMP_Text slowCountText;

    [Header("Buttons")]
    public Button startButton;
    public Button quitButton;
    public Button pauseButton;
    public Button menuButton;
    public Button adButton;
    public Button gameOverButton;
    public Button shopButton;
    public Button healthItemButton;
    public Button slowItemButton;

    //[Header("Bars")]

    [Header("Menus")]
    public GameObject mainMenu;
    public GameObject pauseMenu;
    public GameObject gameOverMenu;
    public GameObject gameHUD;
    public GameObject adOptionMenu;
    public GameObject shopMenu;

    void Start()
    {
        if (scoreText)
            scoreText.text = GameManager.Instance.Score.ToString();
        if (distanceText)
            distanceText.text = GameManager.Instance.Distance.ToString();
        if (healthText)
            healthText.text = GameManager.Instance.Health.ToString();
        if (highScoreText)
        {
            if (GameManager.Instance.highScore > 0)
                highScoreText.text = "High Score: " + GameManager.Instance.highScore.ToString();
            else
                highScoreText.text = "";
        }
        if (coinsText)
            coinsText.text = GameManager.Instance.coins.ToString();
        if (healthCountText)
            healthCountText.text = GameManager.Instance.luckyClover.ToString();
        if (slowCountText)
            slowCountText.text = GameManager.Instance.slowFeather.ToString();

        if (startButton)
            startButton.onClick.AddListener(StartGame);
        if (pauseButton)
            pauseButton.onClick.AddListener(PauseGame);
        if (quitButton)
            quitButton.onClick.AddListener(QuitGame);
        if (menuButton)
            menuButton.onClick.AddListener(ReturnToMenu);
        if (adButton)
            adButton.onClick.AddListener(PlayAd);
        if (gameOverButton)
            gameOverButton.onClick.AddListener(GameOver);
        if (shopButton)
            shopButton.onClick.AddListener(OpenShopMenu);
        if (healthItemButton)
            healthItemButton.onClick.AddListener(UseHealthItem);
        if (slowItemButton)
            slowItemButton.onClick.AddListener(UseSlowItem);
    }

    void StartGame()
    {
        GameManager.Instance.ResetValues();
        GameManager.Instance.SetGameState(GameManager.GameState.Game);
        SceneManager.LoadScene("GameScene");
    }

    void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    void PauseGame()
    {
        if (GameManager.Instance.state == GameManager.GameState.Game)
            GameManager.Instance.SetGameState(GameManager.GameState.Pause);
        else if (GameManager.Instance.state == GameManager.GameState.Pause)
            GameManager.Instance.ResumeGame();
    }

    void UseHealthItem()
    {
        if (GameManager.Instance.luckyClover > 0)
        {
            GameManager.Instance.luckyClover--;
            GameManager.Instance.Health++;
            GameManager.Instance.SaveGame();

            if (healthCountText)
                healthCountText.text = GameManager.Instance.luckyClover.ToString();

            if (adOptionMenu.activeSelf)
            {
                HideAdMenu();
            }
        }
    }

    void UseSlowItem()
    {
        if (GameManager.Instance.slowFeather > 0)
        {
            GameManager.Instance.slowFeather--;
            GameManager.Instance.HalveSpeed();
            GameManager.Instance.SaveGame();

            if (slowCountText)
                slowCountText.text = GameManager.Instance.slowFeather.ToString();
        }
    }

    void ReturnToMenu()
    {
        GameManager.Instance.SetGameState(GameManager.GameState.Menu);
        SceneManager.LoadScene("MainMenu");
    }

    public void ShowAdMenu()
    {
        Time.timeScale = 0;
        adOptionMenu.SetActive(true);
    }

    public void HideAdMenu()
    {
        Time.timeScale = 1;
        adOptionMenu.SetActive(false);
    }

    void PlayAd()
    {
        AdManager.Instance.PlayAd();
    }

    void GameOver()
    {
        GameManager.Instance.GameOver();
    }

    void OpenShopMenu()
    {
        shopMenu.SetActive(true);
    }
}
