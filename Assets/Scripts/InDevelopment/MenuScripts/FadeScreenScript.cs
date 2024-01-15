using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeScreenScript : MonoBehaviour
{
    [SerializeField] private bool fadeOnStart = true;
    [SerializeField] private float _fadeDuration = 2;
    [SerializeField] private Color _fadeColor;
    private Renderer _rend;

    private void Start()
    {
        _rend = GetComponent<Renderer>();
        if(fadeOnStart)
            FadeIn();
    }

    public void FadeIn()
    {
        Fade(1,0);
    }

    public void FadeOut()
    {
        Fade(0,1);
    }
    
    public void Fade(float alphaIn, float alphaOut)
    {
        StartCoroutine(FadeRoutine(alphaIn, alphaOut));
    }

    public IEnumerator FadeRoutine(float alphaIn, float alphaOut)
    {
        float timer = 0;
        while (timer <= _fadeDuration)
        {
            Color newColor = _fadeColor;
            newColor.a = Mathf.Lerp(alphaIn, alphaOut, timer / _fadeDuration);
            
            _rend.material.SetColor("_Color", newColor);
            Debug.Log("Fading");
            
            timer += Time.deltaTime;
            yield return null;
        }
        
        Color newColor2 = _fadeColor;
        newColor2.a = alphaOut;
            
        _rend.material.SetColor("_Color", newColor2);
    }
}
