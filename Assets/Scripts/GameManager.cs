using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI countdownText;
    public CardManager cardManager;
    public float previewDuration = 5f;  // ī�� �ո� �����ִ� �ð�
    public float fadeDuration = 1f;

    public GameObject countdownUI;

    private float currentTime;
    private bool gameEnded = false;
    public float timeLimit = 60f;

    public TextMeshProUGUI timerText;
    public Image progressFillImage; // �ð��� ���α׷��� �� �̹���
    public GameObject successOverlay;
    public GameObject failOverlay;
    public GameObject timerUI;

    void Start()
    {
        //StartGame();
    }

    public void disableUI()
    {
        // UI ��Ȱ��ȭ
        successOverlay.SetActive(false);
        failOverlay.SetActive(false);
        timerUI.SetActive(false);
    }

    public void StartGame()
    {
        cardManager.SpawnCards();
        StartCoroutine(PreviewAndStart());
        StartCoroutine(Countdown());
    }

    // ī�� �ո� �����ְ� 5�� �� �޸����� ��ȯ
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

        countdownUI.SetActive(false); // �ؽ�Ʈ �����

        // ���� ���� �ð��� ǥ��
        StartCoroutine(TimerRoutine());
    }

    IEnumerator TimerRoutine()
    {
        currentTime = timeLimit;
        timerUI.SetActive(true);

        float ratio;

        while (currentTime > 0)
        {
            // �ؽ�Ʈ ������Ʈ
            timerText.text = Mathf.CeilToInt(currentTime).ToString();

            // Fill ������Ʈ
            ratio = currentTime / timeLimit;
            progressFillImage.fillAmount = ratio;

            currentTime -= Time.deltaTime;

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

        ratio = currentTime / timeLimit;
        progressFillImage.fillAmount = ratio;

        if (!gameEnded)
        {
            EndGame(false);
        }
    }

    public void CheckAllMatched()
    {
        if (cardManager.AreAllCardsMatched() && !gameEnded)
        {
            EndGame(true);
        }
    }

    private void EndGame(bool success)
    {
        gameEnded = true;
        cardManager.canToggle = false;
        if (success)
            successOverlay.SetActive(true);
        else
            failOverlay.SetActive(true);
    }
}