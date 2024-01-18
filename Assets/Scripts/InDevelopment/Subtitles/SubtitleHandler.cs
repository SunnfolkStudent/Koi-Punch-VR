using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SubtitleHandler : MonoBehaviour
{
    [Header("Subtitle Text GameObject")]
    public TextMeshProUGUI subtitleText;
    
    [Header("Fade")]
    private const float _fadeDuration = 1.0f;
    private CanvasGroup _canvasGroup;

    void Start()
    {
        SubtitleEventManager.PlaySubtitle += ShowSubtitle;

        // Get the CanvasGroup component or add one if not present
        _canvasGroup = subtitleText.GetComponent<CanvasGroup>();
        if (_canvasGroup == null)
        {
            _canvasGroup = subtitleText.gameObject.AddComponent<CanvasGroup>();
        }

        //SubtitleEventManager.PlaySubtitle.Invoke(VoiceLinesLoader.GetValueForKey("OnGameStart"));
    }

    private void ShowSubtitle(string subtitle)
    {
        subtitleText.gameObject.SetActive(true);
        subtitleText.text = subtitle;
        
        
        StopAllCoroutines();
        
        // Fade in
        StartCoroutine(FadeInSubtitle());

        // Make subtitles hide after a certain duration
        StartCoroutine(HideSubtitleAfterSeconds(5f));
    }

    private IEnumerator FadeInSubtitle()
    {
        float elapsedTime = 0f;
        while (elapsedTime < _fadeDuration)
        {
            _canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsedTime / _fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure that alpha is set to 1 at the end of the fading in
        _canvasGroup.alpha = 1f;
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
        while (elapsedTime < _fadeDuration)
        {
            _canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / _fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure that alpha is set to 0 at the end of the fading out
        _canvasGroup.alpha = 0f;

        // Hide the subtitle
        HideSubtitle();
    }

    private void HideSubtitle()
    {
        subtitleText.gameObject.SetActive(false);
    }
}
