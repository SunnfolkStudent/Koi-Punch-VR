using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SubtitleHandler : MonoBehaviour
{
    [Header("Subtitle Text GameObject")]
    public TextMeshProUGUI subtitleText;
    
    [Header("Fade")]
    public float fadeDuration = 1.0f;
    private CanvasGroup canvasGroup;

    void Start()
    {
        SubtitleEventManager.PlaySubtitle += ShowSubtitle;

        // Get the CanvasGroup component or add one if not present
        canvasGroup = subtitleText.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = subtitleText.gameObject.AddComponent<CanvasGroup>();
        }
        
        ShowSubtitle(VoiceLinesLoader.GetValueForKey("OnPunch"));
    }

    private void ShowSubtitle(string subtitle)
    {
        subtitleText.gameObject.SetActive(true);
        subtitleText.text = subtitle;
        
        // Start fading in
        StartCoroutine(FadeInSubtitle());

        // Schedule to hide after a certain duration
        StartCoroutine(HideSubtitleAfterSeconds(5f));
    }

    private IEnumerator FadeInSubtitle()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure that alpha is set to 1 at the end of the fading in
        canvasGroup.alpha = 1f;
    }

    private IEnumerator HideSubtitleAfterSeconds(float seconds)
    {
        yield return new WaitForSecondsRealtime(seconds);

        // Start fading out
        StartCoroutine(FadeOutSubtitle());
    }

    private IEnumerator FadeOutSubtitle()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure that alpha is set to 0 at the end of the fading out
        canvasGroup.alpha = 0f;

        // Hide the subtitle
        HideSubtitle();
    }

    private void HideSubtitle()
    {
        subtitleText.gameObject.SetActive(false);
    }
}
