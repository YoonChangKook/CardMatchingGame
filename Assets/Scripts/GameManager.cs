using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public UIManager uiManager;
    public TextMeshProUGUI countdownText;
    public CardManager cardManager;
    public float previewDuration = 5f;  // 카드 앞면 보여주는 시간
    public float fadeDuration = 1f;

    private float currentTime;
    private bool gameEnded = false;

    public TextMeshProUGUI timerText;
    public GameObject previewOverlay;
    public GameObject countdownUI;
    public GameObject timerUI;

    void Start()
    {
        //StartGame();
    }

    public void disableUI()
    {
        // UI 비활성화
        timerUI.SetActive(false);
    }

    public void StartGame()
    {
        cardManager.SpawnCards();
        StartCoroutine(PreviewAndStart());
        StartCoroutine(Countdown());
    }

    // 카드 앞면 보여주고 5초 후 뒷면으로 전환
    private IEnumerator PreviewAndStart()
    {
        yield return new WaitForSeconds(previewDuration + fadeDuration);
        cardManager.HideAllCards();
    }

    IEnumerator Countdown()
    {
        countdownUI.SetActive(true);

        float currentTime = previewDuration + fadeDuration;

        while (currentTime > 0)
        {
            if (currentTime > 0.5f)
            {
                countdownText.text = Mathf.CeilToInt(Mathf.Min(currentTime, 5)).ToString();
            }
            else
            {
                countdownText.text = "START!";
            }

            yield return new WaitForSeconds(0.1f);
            currentTime -= 0.1f;
        }

        countdownUI.SetActive(false); // 텍스트 숨기기

        // 게임 시작 시간초 표시
        StartCoroutine(TimerRoutine());
    }

    IEnumerator TimerRoutine()
    {
        currentTime = 0;
        timerUI.SetActive(true);

        while (true)
        {
            // 텍스트 업데이트
            timerText.text = currentTime.ToString("F3");

            currentTime += Time.deltaTime;

            if (cardManager.AreAllCardsMatched() && !gameEnded)
            {
                EndGame(true);
            }
            
            if (!gameEnded)
            {
                yield return null;
            }
            else
            {
                break;
            }
        }

        // 게임 끝나면 Clear 화면 노출
        uiManager.ShowGameClearPanel();
    }

    private void EndGame(bool success)
    {
        gameEnded = true;
        cardManager.canToggle = false;
    }

    public void initialize()
    {
        gameEnded = false;
        StopAllCoroutines();
        previewOverlay.SetActive(true);
        countdownUI.SetActive(true);
        timerUI.SetActive(false);
        cardManager.ClearCards();
        cardManager.canToggle = true;
        uiManager.ShowStartUI();
    }
}