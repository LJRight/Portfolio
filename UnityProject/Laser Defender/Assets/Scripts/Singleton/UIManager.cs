using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    [Header("Panels")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject inGameBottomUIPanel;
    [SerializeField] private GameObject inGameTopUIPanel;
    [SerializeField] private GameObject shopMenuPanel;
    [SerializeField] private GameObject settingMenuPanel;
    private VariableJoystick joyStick;

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI endingScoreText;
    string defaultEndingText = "Your Score : ";
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI lifeText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI playerDpsText;
    private int mainMenuIdx = 0, firstStageIdx = 1;
    private SlideFadeManager fm;
    public SlideFadeManager SFM => fm;
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        fm = GetComponent<SlideFadeManager>();
        joyStick = GetComponentInChildren<VariableJoystick>(true);
        if (fm == null)
            Debug.LogWarning("SlideFadeManager component not found on UIManager GameObject.");
        if (joyStick == null)
            Debug.LogWarning("VariableJoystick component not found on UIManager GameObject.");

    }
    private void SetText(TextMeshProUGUI target, string value)
    {
        target.text = value;
    }
    public void SetScoreText(int value)
    {
        SetText(scoreText, value > 0 ? string.Format("{0:#,###}", value) : "0");
    }
    public void SetEndingScoreText(int value)
    {
        endingScoreText.text = defaultEndingText;
        SetText(endingScoreText, endingScoreText.text + string.Format("{0:#,###}", value));
    }
    public void SetCoinText(int value)
    {
        string text = value > 0 ? string.Format("{0:#,###}", value) : "0";
        text += "G";
        SetText(coinText, text);
    }
    public void SetPlayerHealthText(float value)
    {
        SetText(lifeText, ((int)value).ToString());
    }
    public void SetTimeText(float value)
    {
        SetText(timeText, TimerFormat(value));
    }
    public void SetPlayerDPSText()
    {
        float dps = GameObject.FindWithTag("Player").GetComponent<PlayerLevel>().GetPlayerDPS();
        SetText(playerDpsText, $"{dps:F2} dps");
    }
    string TimerFormat(float value)
    {
        return $"{Mathf.FloorToInt(value / 60f):D2}:{Mathf.FloorToInt(value % 60f):D2}";
    }
    private void PanelActive(GameObject panel, bool option)
    {
        panel.SetActive(option);
    }
    private void SettingMenuPanelActive(bool option)
    {
        PanelActive(settingMenuPanel, option);
    }
    private void InGameUIPanelActive(bool option)
    {
        PanelActive(inGameBottomUIPanel, option);
        PanelActive(inGameTopUIPanel, option);
    }
    private void MainMenuPanelActive(bool option)
    {
        PanelActive(mainMenuPanel, option);
    }
    private void GameOverPanelActive(bool option)
    {
        PanelActive(gameOverPanel, option);
    }
    public void ShopPanelActive(bool option)
    {
        joyStick.gameObject.SetActive(!option);
        PanelActive(shopMenuPanel, option);
    }
    public void OnLevelSceneLoaded()
    {
        MainMenuPanelActive(false);
        InGameUIPanelActive(true);
        GameOverPanelActive(false);
        joyStick.gameObject.SetActive(true);
        SetUpInGameTopUIPanelSettingButtonListener();
        SetUpSettingMenuPanelListener();
        SetUpShopMenuPanelExitButtonListener();
        SetPlayerDPSText();
    }
    public void OnMainMenuSceneLoaded()
    {
        joyStick.gameObject.SetActive(false);
        MainMenuPanelActive(true);
        InGameUIPanelActive(false);
        GameOverPanelActive(false);
        SetUpMainMenuPanelButtonListener();
    }
    public void OnGameOverSceneLoaded()
    {
        joyStick.gameObject.SetActive(false);
        SetEndingScoreText(ScoreKeeper.Instance.Score);
        MainMenuPanelActive(false);
        InGameUIPanelActive(false);
        GameOverPanelActive(true);
        SetUpGameOverPanelButtonListener();
    }
    /// <summary>
    /// MainMenuPanel 내에 위치한 버튼 오브젝트들을 찾아,
    /// 버튼 클릭 시 씬의 활성화 된 인스턴스의 메서드를 실행하도록 이벤트를 설정하는 함수.
    /// </summary>
    private void SetUpMainMenuPanelButtonListener()
    {
        Button quitButton = mainMenuPanel.transform.Find("Quit Button")?.GetComponent<Button>();
        Button gameStartButton = mainMenuPanel.transform.Find("Game Start Button")?.GetComponent<Button>();
        if (quitButton == null)
        {
            Debug.LogWarning("Quit Button not found in Main Menu Panel");
            return;
        }
        if (gameStartButton == null)
        {
            Debug.LogWarning("Game Start Button not found in Main Menu Panel");
            return;
        }
        quitButton.onClick.RemoveAllListeners();
        quitButton.onClick.AddListener(() =>
        {
            AudioPlayer.Instance.PlayButtonSelectClip();
            if (GameController.Instance != null)
                StartCoroutine(GameController.Instance.QuitGame());
            else
                Debug.LogWarning("GameController.Instance is null");
        });
        gameStartButton.onClick.RemoveAllListeners();
        gameStartButton.onClick.AddListener(() =>
        {
            AudioPlayer.Instance.PlayButtonSelectClip();
            if (fm != null)
                fm.SlideToScene(firstStageIdx);
            else
                Debug.LogWarning("SlideFadeManager not found on UIManager object");
        });
    }
    /// <summary>
    /// MainMenuPanel 내에 위치한 버튼 오브젝트들을 찾아,
    /// 버튼 클릭 시 씬의 활성화 된 인스턴스의 메서드를 실행하도록 이벤트를 설정하는 함수.
    /// </summary>
    private void SetUpGameOverPanelButtonListener()
    {
        Button restartButton = gameOverPanel.transform.Find("Restart Button")?.GetComponent<Button>();
        Button mainMenuButton = gameOverPanel.transform.Find("Main Menu Button")?.GetComponent<Button>();
        if (restartButton == null)
        {
            Debug.LogWarning("Restart Button not found in Game Over Panel");
            return;
        }
        if (mainMenuButton == null)
        {
            Debug.LogWarning("Main Menu Button not found in Game Over Panel");
            return;
        }
        restartButton.onClick.RemoveAllListeners();
        restartButton.onClick.AddListener(() =>
        {
            AudioPlayer.Instance.PlayButtonSelectClip();
            ScoreKeeper.Instance.ResetScore();
            GameController.Instance.ResetGameTime();
            if (fm != null)
                fm.SlideToScene(firstStageIdx);
            else
                Debug.LogWarning("SlideFadeManager not found on UIManager object");
        });
        mainMenuButton.onClick.RemoveAllListeners();
        mainMenuButton.onClick.AddListener(() =>
        {
            AudioPlayer.Instance.PlayButtonSelectClip();
            ScoreKeeper.Instance.ResetScore();
            GameController.Instance.ResetGameTime();
            if (fm != null)
                fm.SlideToScene(mainMenuIdx);
            else
                Debug.LogWarning("Slide Fade Manager not found on UI Manager object");
        });
    }

    private void SetUpInGameTopUIPanelSettingButtonListener()
    {
        Button settingButton = inGameTopUIPanel.transform.Find("Setting Button")?.GetComponent<Button>();
        if (settingButton == null)
        {
            Debug.LogWarning("Setting Button not found in InGameTopUIPanel");
            return;
        }
        settingButton.onClick.RemoveAllListeners();
        settingButton.onClick.AddListener(() =>
        {
            AudioPlayer.Instance.PlayButtonSelectClip();
            GameController.Instance.StopGameTime();
            SettingMenuPanelActive(true);
            joyStick.gameObject.SetActive(false);
            settingButton.enabled = false;
        });
    }
    private void SetUpSettingMenuPanelListener()
    {
        Button exitButton = settingMenuPanel.transform.Find("Exit Button")?.GetComponent<Button>();
        if (exitButton == null)
        {
            Debug.LogWarning("Exit Button not found in Setting Menu Panel");
            return;
        }
        exitButton.onClick.RemoveAllListeners();
        exitButton.onClick.AddListener(() =>
        {
            GameController.Instance.PlayGameTime();
            AudioPlayer.Instance.PlayButtonSelectClip();
            SettingMenuPanelActive(false);
            joyStick.gameObject.SetActive(true);
            inGameTopUIPanel.transform.Find("Setting Button").GetComponent<Button>().enabled = true;
        });
        GameObject settingLayout = settingMenuPanel.transform.Find("Setting Layout").gameObject;
        Slider BGMSlider = settingLayout.transform.Find("BGM").GetComponentInChildren<Slider>();
        if (BGMSlider == null)
        {
            Debug.LogWarning("BGM Slider not found in Setting Menu Panel");
            return;
        }
        BGMSlider.value = 1.0f;
        BGMSlider.onValueChanged.RemoveAllListeners();
        BGMSlider.onValueChanged.AddListener((value) =>
        {
            AudioPlayer.Instance.SetBGMVolume(value);
        });
        Slider SFXSlider = settingLayout.transform.Find("SFX")?.GetComponentInChildren<Slider>();
        if (SFXSlider == null)
        {
            Debug.LogWarning("SFX Slider not found in Setting Menu Panel");
            return;
        }
        SFXSlider.value = 1.0f;
        SFXSlider.onValueChanged.RemoveAllListeners();
        SFXSlider.onValueChanged.AddListener((value) =>
        {
            AudioPlayer.Instance.SetSFXVolume(value);
        });
    }

    private void SetUpShopMenuPanelExitButtonListener()
    {
        Button exitButton = shopMenuPanel.transform.Find("Exit Button")?.GetComponent<Button>();
        if (exitButton == null)
        {
            Debug.LogWarning("Exit Button not found in Shop Menu Panel");
            return;
        }
        exitButton.onClick.RemoveAllListeners();
        exitButton.onClick.AddListener(() =>
        {
            GameController.Instance.PlayGameTime();
            AudioPlayer.Instance.PlayButtonSelectClip();
            ShopPanelActive(false);
        });
    }
}
