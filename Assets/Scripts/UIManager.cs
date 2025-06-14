using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public GameObject startPanel;
    public GameObject gameUI;

    public void ShowStartUI()
    {
        startPanel.SetActive(true);
        gameUI.SetActive(false);
    }

    public void ShowGameUI()
    {
        startPanel.SetActive(false);
        gameUI.SetActive(true);
    }

    public void ShowResultUI(string result)
    {
        startPanel.SetActive(false);
        gameUI.SetActive(false);
    }
}
