using System;
using System.Collections;
using UnityEngine;
using TMPro;

public class PromptVisualScript : MonoBehaviour
{
    private TextMeshProUGUI _myTextMeshProUGUI;
    private float _blinkSpeed = 1f;
    private float _minAlpha = 0.3f;

    void Awake()
    {
        _myTextMeshProUGUI = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        StartCoroutine(BlinkText());
    }
    
    private void OnDisable()
    {
        StopAllCoroutines();
    }

    IEnumerator BlinkText()
    {
        while (true)
        {
            float t = 0f;
            while (t < 1f)
            {
                t += Time.unscaledDeltaTime / _blinkSpeed;
                _myTextMeshProUGUI.color = new Color(1f, 1f, 1f, Mathf.Lerp(1f, _minAlpha, t));
                yield return null;
            }

            t = 0f;
            while (t < 1f)
            {
                t += Time.unscaledDeltaTime / _blinkSpeed;
                _myTextMeshProUGUI.color = new Color(1f, 1f, 1f, Mathf.Lerp(_minAlpha, 1f, t));
                yield return null;
            }
        }
    }
}