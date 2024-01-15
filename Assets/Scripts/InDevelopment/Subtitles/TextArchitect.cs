using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using TMPro;

public class TextArchitect
{
    private TextMeshProUGUI tmproUI;
    private TextMeshPro tmproWorld;
    public TMP_Text tmpro => tmproUI != null ? tmproUI : tmproWorld;

    public string currentText => tmpro.text;
    public string targetText { get; private set; } = "";
    public string preText { get; private set; } = "";
    private int _preTextLength = 0;

    public string fullTargetText => preText + targetText;
    
    public enum BuildMethod { instant, typewriter, fade}
    public BuildMethod buildMethod = BuildMethod.fade;
    
    public Color textColor
    {
        get { return tmpro.color; }
        set { tmpro.color = value; }
    }
    
    private const float _speed = 1;
    public int charactersPerCycle
    {
        get
        {
            return _speed <= 2f ? _characterMultiplier :
                _speed <= 2.5f ? _characterMultiplier * 2 : _characterMultiplier * 3;
        }
    }

    private int _characterMultiplier = 1; 
    

    public TextArchitect(TextMeshProUGUI tmproUI)
    {
        this.tmproUI = tmproUI;
    }

    public TextArchitect(TextMeshPro tmproWorld)
    {
        this.tmproWorld = tmproWorld;
    }

    public Coroutine Build(string text)
    {
        preText = "";
        targetText = text;
        
        stop();

        _buildProcess = tmpro.StartCoroutine(Building());
        return _buildProcess;
    }
    
    public Coroutine Append(string text)
    {
        preText = tmpro.text;
        targetText = text;
        
        stop();

        _buildProcess = tmpro.StartCoroutine(Building());
        return _buildProcess;
    }

    private Coroutine _buildProcess = null;
    public bool isBuilding => _buildProcess != null;

    public void stop()
    {
        if (!isBuilding)
            return;
        
        tmpro.StopCoroutine(_buildProcess);
        _buildProcess = null;
    }

    IEnumerator Building()
    {
        Prepare();
        switch (buildMethod)
        {
            case BuildMethod.typewriter:
                yield return Build_Typewriter();
                break;
            case BuildMethod.fade:
                yield return Build_Fade();
                break;
        }
        OnComplete();
    }

    private void OnComplete()
    {
        _buildProcess = null;
    }

    private void Prepare()
    {
        switch(buildMethod)
        {
            case BuildMethod.instant:
                prepare_Instant();
                break;
            case BuildMethod.typewriter:
                prepare_Typewriter();
                break;
            case BuildMethod.fade:
                prepare_Fade();
                break;
        }
    }

    private void prepare_Instant()
    {
        tmpro.color = tmpro.color;
        tmpro.text = fullTargetText;
        tmpro.ForceMeshUpdate();
        tmpro.maxVisibleCharacters = tmpro.textInfo.characterCount;
    }

    private void prepare_Typewriter()
    {
        tmpro.color = tmpro.color;
        tmpro.maxVisibleCharacters = 0;
        tmpro.text = preText;

        if (preText != "")
        {
            tmpro.ForceMeshUpdate();
            tmpro.maxVisibleCharacters = tmpro.textInfo.characterCount;
        }

        tmpro.text += targetText;
        tmpro.ForceMeshUpdate();
    }

    private void prepare_Fade()
    {
        
    }

    private IEnumerator Build_Typewriter()
    {
        while (tmpro.maxVisibleCharacters < tmpro.textInfo.characterCount)
        {
            tmpro.maxVisibleCharacters += charactersPerCycle;

            yield return new WaitForSeconds(0.015f / _speed);
        }
    }

    private IEnumerator Build_Fade()
    {
        yield return null;
    }
}
