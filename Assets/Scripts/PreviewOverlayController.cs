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

        // 1�� ���� ����
        yield return new WaitForSeconds(holdTime);

        float elapsed = 0f;

        // ���� ����, ���� ��� �Ѿ
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