using UnityEngine;
using System.Collections;

public class PreviewOverlayController : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public float holdTime = 1f;
    public float fadeDuration = 1f;

    public GameManager gameManager;

    void Start()
    {
        //StartCoroutine(FadeOutSequence());
        //StartOverlay();
    }

    public void StartOverlay()
    {
        gameManager.disableUI();
        StartCoroutine(FadeOutSequence());
    }

    IEnumerator FadeOutSequence()
    {
        initCanvasAlpha();

        // 1초 동안 유지
        yield return new WaitForSeconds(holdTime);

        float elapsed = 0f;

        // 게임 시작, 게임 뷰로 넘어감
        gameManager.StartGame();

        while (elapsed < fadeDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            canvasGroup.alpha = alpha;
            elapsed += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 0f;
        gameObject.SetActive(false);
    }

    public void initCanvasAlpha()
    {
        canvasGroup.alpha = 1f;
    }
}