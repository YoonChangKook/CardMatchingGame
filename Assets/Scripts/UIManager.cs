using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    public GameObject startPanel;
    public GameObject istPanel;
    public GameObject descriptionPanel;
    public GameObject gameUI;
    public GameObject gameClearPanel;

    private void resetUI()
    {
        startPanel.SetActive(false);
        istPanel.SetActive(false);
        descriptionPanel.SetActive(false);
        gameUI.SetActive(false);
        gameClearPanel.SetActive(false);
    }

    public void ShowStartUI()
    {
        resetUI();
        startPanel.SetActive(true);
    }

    public void ShowIstPanel()
    {
        resetUI();
        istPanel.SetActive(true);
    }

    public void ShowDescriptionPanel()
    {
        resetUI();
        descriptionPanel.SetActive(true);
    }

    public void ShowGameUI()
    {
        resetUI();
        gameUI.SetActive(true);
    }

    public void ShowGameClearPanel()
    {
        resetUI();
        gameClearPanel.SetActive(true);
    }
}
