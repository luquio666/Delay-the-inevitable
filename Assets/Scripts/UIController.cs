using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public GameObject HintsGO;
    public TextMeshProUGUI HintsLabel;
    public TextMeshProUGUI HeadBubbleLabel;
    public Image PeeFill;
    [Space]
    public GameObject GameoverScreen;
    public Button Retry;
    public Button NoWay;
    [Space]
    public GameObject GameWonScreen;
    public Button BtnWonRetry;
    public Button BtnWonNoWay;
    [Space]
    public GameObject MainMenu;
    public Button Play;
    public Button Exit;
    [Space]
    public Animator HintsLabelAnim;
    public Animator HeadBubbleLabelAnim;

    private void Awake()
    {
        Retry.onClick.AddListener(ButtonRetry);
        NoWay.onClick.AddListener(ButtonNoWay);
        BtnWonRetry.onClick.AddListener(ButtonRetry);
        BtnWonNoWay.onClick.AddListener(ButtonNoWay);

        Play.onClick.AddListener(ButtonPlay);
        Exit.onClick.AddListener(ButtonNoWay);
    }

    private void OnEnable()
    {
        PauseGame();

        GameEvents.OnSendHintsMsg += SendHintsMsg;
        GameEvents.OnSendHeadBubbleMsg += SendHeadBubbleMsg;
        GameEvents.OnClearHintsMsg += ClearHintsMsg;
        GameEvents.OnClearHeadBubbleMsg += ClearHeadBubbleMsg;
        GameEvents.OnGameOver += GameOver;
        GameEvents.OnGameWon += GameWon;
    }

    private void OnDisable()
    {
        GameEvents.OnSendHintsMsg -= SendHintsMsg;
        GameEvents.OnSendHeadBubbleMsg -= SendHeadBubbleMsg;
        GameEvents.OnClearHintsMsg -= ClearHintsMsg;
        GameEvents.OnClearHeadBubbleMsg -= ClearHeadBubbleMsg;
        GameEvents.OnGameOver -= GameOver;
        GameEvents.OnGameWon -= GameWon;
    }

    private void Update()
    {
        PeeFill.fillAmount = GameManager.Instance.PeeMeter._currentValue;
    }

    private void PauseGame()
    {
        Time.timeScale = 0f;
    }

    private void UnpauseGame()
    {
        Time.timeScale = 1f;
    }

    private void ButtonPlay()
    {
        MainMenu.SetActive(false);
        UnpauseGame();
    }

    private void ButtonRetry()
    {
        SceneManager.LoadScene(0);
    }

    private void ButtonNoWay()
    {
        Application.Quit();
    }

    private void SendHintsMsg(string msg)
    {
        if(!HintsGO.activeSelf) HintsGO.SetActive(true);
        HintsLabel.text = msg;
    }

    private void SendHeadBubbleMsg(string msg)
    {
        HeadBubbleLabel.text = msg;
        if (HeadBubbleLabelAnim != null)
            HeadBubbleLabelAnim.Play("HeadBubbleShow", 0, 0f);
    }

    private void ClearHintsMsg()
    {
        HintsGO.SetActive(false);
        HintsLabel.text = string.Empty;
    }

    private void ClearHeadBubbleMsg()
    {
        HeadBubbleLabel.text = string.Empty;
        if (HeadBubbleLabelAnim != null)
            HeadBubbleLabelAnim.Play("HeadBubbleHide", 0, 0f);
    }

    private void GameOver()
    {
        GameoverScreen.SetActive(true);
    }

    private void GameWon()
    {
        GameWonScreen.SetActive(true);
    }
}
