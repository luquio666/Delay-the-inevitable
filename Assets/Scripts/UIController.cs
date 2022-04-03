using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public TextMeshProUGUI HintsLabel;
    public TextMeshProUGUI HeadBubbleLabel;
    public Image PeeFill;
    public GameObject GameoverScreen;
    public Button Retry;
    public Button NoWay;
    [Space]
    public Animator HintsLabelAnim;
    public Animator HeadBubbleLabelAnim;

    private void Awake()
    {
        Retry.onClick.AddListener(ButtonRetry);
        NoWay.onClick.AddListener(ButtonNoWay);
    }
    

    private void OnEnable()
    {
        GameEvents.OnSendHintsMsg += SendHintsMsg;
        GameEvents.OnSendHeadBubbleMsg += SendHeadBubbleMsg;
        GameEvents.OnClearHintsMsg += ClearHintsMsg;
        GameEvents.OnClearHeadBubbleMsg += ClearHeadBubbleMsg;
        GameEvents.OnShowGameoverScreen += ShowGameoverScreen;
    }

    private void OnDisable()
    {
        GameEvents.OnSendHintsMsg -= SendHintsMsg;
        GameEvents.OnSendHeadBubbleMsg -= SendHeadBubbleMsg;
        GameEvents.OnClearHintsMsg -= ClearHintsMsg;
        GameEvents.OnClearHeadBubbleMsg -= ClearHeadBubbleMsg;
        GameEvents.OnShowGameoverScreen -= ShowGameoverScreen;
    }

    private void ButtonRetry()
    { 
        // todo: restart game
    }
    private void ButtonNoWay()
    { 
        // todo: go to main menu
    }

    private void SendHintsMsg(string msg)
    {
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
        HintsLabel.text = string.Empty;
    }

    private void ClearHeadBubbleMsg()
    {
        HeadBubbleLabel.text = string.Empty;
        if (HeadBubbleLabelAnim != null)
            HeadBubbleLabelAnim.Play("HeadBubbleHide", 0, 0f);
    }

    private void ShowGameoverScreen()
    {
        GameoverScreen.SetActive(true);
    }
}