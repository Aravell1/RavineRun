using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

[Serializable]
public class PlayerData
{
    [Serializable]
    public struct Date
    {
        public int Year;
        public int Month;
        public int Day;

        public Date(int y, int m, int d)
        {
            Year = y;
            Month = m;
            Day = d;
        }

        public static bool CompareDate(Date savedDate, Date currentDate)
        {
            return savedDate.Year == currentDate.Year && savedDate.Month == currentDate.Month && savedDate.Day == currentDate.Day;
        }
    }

    public Date savedDate = new(0, 0, 0);
    public int dailyAds = 0;
    public int dailyCoins = 0;
    public int coins = 0;
    public int highScore = 0;
    public int luckyClover = 0;
    public int slowFeather = 0;
}

[DefaultExecutionOrder(-100)]
public class GameManager : Singleton<GameManager>
{
    int _score = 0;
    int _health = 3;
    float _distance = 0;
    float startSpeed = 0;
    readonly float speedIncrement = 0.5f;
    bool addingDistance = false;
    bool enableControls = false;
    CanvasManager canvas;
    GenerateLevel levelManager;
    GameObject player;

    private static readonly string key = "3817498320592469564264598042905429143";
    [Header("Saved Settings")]
    private string dataPath;
    public PlayerData Data;
    public bool adPlayed = false;
    public static int maxDailyCoins = 100;
    public int dailyCoins = 0;
    public int coins = 0;
    public int highScore;

    [Header("Items")]
    public int luckyClover = 0;
    public int slowFeather = 0;

    [Header("Movement Settings")]
    public DistanceTracking tracker;
    public float moveSpeed = 5;
    public bool canMove = true;

    [Header("Game State")]
    public GameState state = GameState.Menu;
    public enum GameState
    {
        Menu,
        Game,
        Pause
    }

    public int Score
    {
        get
        {
            return _score;
        }

        set
        {
            _score = value;

            if (canvas)
                canvas.scoreText.text = _score.ToString();
        }
    }

    public float Distance
    {
        get
        {
            return _distance;
        }

        set
        {
            if (!levelManager)
                levelManager = GameObject.Find("LevelManager").GetComponent<GenerateLevel>();

            float nextCheckpoint = _distance + (50 - (_distance % 50));
            if (value >= nextCheckpoint)
            {
                moveSpeed += speedIncrement;
                player.GetComponent<PlayerMove>().anim.SetFloat("SpeedMultiplier", GetSpeedMultiplier() + speedIncrement / startSpeed);
            }

            _distance = value;

            if (canvas)
                canvas.distanceText.text = Mathf.Floor(_distance).ToString();
        }
    }

    public int Health
    {
        get
        {
            return _health;
        }

        set
        {
            _health = value;

            if (canvas)
                canvas.healthText.text = _health.ToString();

            if (_health <= 0)
            {
                if (AdManager.Instance.dailyAds < AdManager.maxDailyAds && !adPlayed)
                {
                    if (canvas)
                    {
                        canvas.ShowAdMenu();
                    }
                }
                else
                    GameOver();
            }
        }
    }

    protected override void Awake()
    {
        base.Awake();

        Application.targetFrameRate = 60;
        SceneManager.sceneLoaded += OnSceneLoaded;

        //dataPath = Application.dataPath + "/Saves/Save01.sav";
        dataPath = Application.persistentDataPath + Path.DirectorySeparatorChar + "Save01.sav";

        if (File.Exists(dataPath))
        {
            string playerDataJSON = File.ReadAllText(dataPath);
            JsonUtility.FromJsonOverwrite(EncryptDecrypt(playerDataJSON), Data);
        }
        else
        {
            string playerDataJSON = EncryptDecrypt(JsonUtility.ToJson(Data));
            File.Create(dataPath);
            File.WriteAllText(dataPath, playerDataJSON);
        }
        LoadGame();

        PlayerData.Date today = new(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);

        if (!PlayerData.Date.CompareDate(Data.savedDate, today))
            DailyReset();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (startSpeed == 0)
            startSpeed = moveSpeed;

        if (!canvas)
            canvas = GameObject.Find("Canvas").GetComponent<CanvasManager>();

        if (state == GameState.Game && !player)
        {
            player = GameObject.Find("Player");
        }

        if (addingDistance && state == GameState.Game)
        {
            StartCoroutine(AddDistance());
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GetGamePaused())
                ResumeGame();
            else if (state == GameState.Game)
                SetGameState(GameState.Pause);
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        canvas = GameObject.Find("Canvas").GetComponent<CanvasManager>();
    }

    public void HalveSpeed()
    {
        Debug.Log(GetSpeedMultiplier());
        player.GetComponent<PlayerMove>().anim.SetFloat("SpeedMultiplier", (GetSpeedMultiplier() - 1) / 2 + 1);
        Debug.Log(GetSpeedMultiplier());
    }

    public bool GetEnableControls()
    {
        return enableControls;
    }

    public bool GetGamePaused()
    {
        return state == GameState.Pause;
    }

    public void SetGameState(GameState s)
    {
        state = s;
        StateMode();
    }

    public void ResumeGame()
    {
        if (canvas)
            canvas.pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        enableControls = true;
        SetGameState(GameState.Game);
    }

    void StateMode()
    {
        switch (state)
        {
            case GameState.Pause:
                canvas.pauseMenu.SetActive(true);
                enableControls = false;
                Time.timeScale = 0f;
                break;
            case GameState.Game:
                Time.timeScale = 1f;
                break;
            case GameState.Menu:
                Time.timeScale = 0f;
                enableControls = false;
                break;
        }

    }

    public void GameOver()
    {
        if (dailyCoins < maxDailyCoins)
            AddCoins(Score);
        SaveGame();

        SetGameState(GameState.Menu);
        SceneManager.LoadScene("GameOver");
    }

    void AddCoins(int count)
    {
        if (count > maxDailyCoins)
            count = maxDailyCoins;
        if (count + dailyCoins > maxDailyCoins)
            count = maxDailyCoins - dailyCoins;

        dailyCoins += count;
        coins += count;
    }

    public void StartTracking()
    {
        addingDistance = true;
        enableControls = true;
        if (canvas)
            canvas.gameHUD.SetActive(true);
    }

    IEnumerator AddDistance()
    {
        addingDistance = false;
        if (!tracker)
            tracker = (DistanceTracking)FindObjectOfType(typeof(DistanceTracking));
        Distance = tracker.position;
        yield return new WaitForSeconds(2);
        addingDistance = true;
    }

    public float GetSpeedMultiplier()
    {
        return player.GetComponent<PlayerMove>().anim.GetFloat("SpeedMultiplier");
    }

    public void ResetValues()
    {
        canMove = true;
        moveSpeed = 5f;
        _health = 3;
        _score = 0;
        _distance = 0;
        adPlayed = false;
    }

    void DailyReset()
    {
        AdManager.Instance.dailyAds = 0;
        dailyCoins = 0;
        SaveGame();
    }

    public void SaveGame()
    {
        //Getting Data and Setting Values
        Data.savedDate = new(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);
        Data.dailyAds = AdManager.Instance.dailyAds;
        Data.dailyCoins = dailyCoins;
        Data.coins = coins;
        Data.slowFeather = slowFeather;
        Data.luckyClover = luckyClover;

        if (Score > highScore)
        {
            highScore = Score;
            Data.highScore = highScore;
        }

        //Showing Data
        string playerDataJSON = EncryptDecrypt(JsonUtility.ToJson(Data));
        if (File.Exists(dataPath)) 
            File.WriteAllText(dataPath, playerDataJSON);
        else
        {
            File.Create(dataPath);
            File.WriteAllText(dataPath, playerDataJSON);
        }
    }

    void OnApplicationQuit()
    {
        SaveGame();
    }

    public void LoadGame()
    {
        AdManager.Instance.dailyAds = Data.dailyAds;
        highScore = Data.highScore;
        coins = Data.coins;
        dailyCoins = Data.dailyCoins;
        luckyClover = Data.luckyClover;
        slowFeather = Data.slowFeather;
    }

    private static string EncryptDecrypt(string data)
    {
        string result = "";
        for (int i = 0; i < data.Length; i++) { result += (char)(data[i] ^ key[i % key.Length]); }
        return result;
    }
}
