using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ZenMetreVisualManager : MonoBehaviour
{
    [Header("Zen Metre Bar Images and materials")]
    public GameObject[] zenMetreBars;
    private List<Image> _zenMetreBarImages = new List<Image>();
    private List<Material> _zenMetreBarMaterials = new List<Material>();
    private int _fillAmountPropertyID;
    
    [Header("Zen Metre Values")]
    private float _maxZenMetreValue = 100f;
    private int _oldZenLevel;
    
    [Header("Sparkle List")]
    private List<GameObject> _sparkleList = new List<GameObject>();
    
    [Header("Prompt Text")]
    public TextMeshProUGUI promptText;
    public static string promptTextToBeShown;

    // Start is called before the first frame update
    void Start()
    {
        #region ---Initialization---
        try
        {
            foreach (GameObject zenMeterbar in zenMetreBars)
            {
                _zenMetreBarImages.Add(zenMeterbar.GetComponent<Image>());
            }

            _fillAmountPropertyID = Shader.PropertyToID("_FillAmount");
            
            foreach (Image zenMetreBarImage in _zenMetreBarImages)
            {
                zenMetreBarImage.material.SetFloat(_fillAmountPropertyID, 0f);
                _zenMetreBarMaterials.Add(zenMetreBarImage.material);
            }
            
            zenMetreBars[1].gameObject.SetActive(false);
            zenMetreBars[2].gameObject.SetActive(false);
        }
        catch (Exception e)
        {
            Debug.LogError("Exception: " + e + " occurred while initializing Zen Metre Bar.");
        }
        
        
        _sparkleList = GameObject.FindGameObjectsWithTag("Sparkle").ToList();
        HideSparkles();

        promptTextToBeShown = "Hold all buttons to charge!";
        HidePromptText();
        #endregion
        
        #region ---Event Subscriptions---
        InternalZenEventManager.updateVisualZenBar += UpdateZenBar;
        InternalZenEventManager.showSparkles += ShowSparkles;
        InternalZenEventManager.hideSparkles += HideSparkles;
        InternalZenEventManager.showPromptText += ShowPromptText;
        InternalZenEventManager.hidePromptText += HidePromptText;
        #endregion
    }
    
    private void UpdateZenBar()
    {
        float zenMetreValue = ZenMetreManager.Instance.zenMetreValue;
        int workingZenLevel = ZenMetreManager.Instance.zenLevel;
        
        try
        {
            // Calculate fill amount based on the current Zen value
            float fillAmount = zenMetreValue / _maxZenMetreValue;

            // Ensure the fillAmount is within the range of 0 to 1
            fillAmount = Mathf.Clamp01(fillAmount);

            // Update the _FillAmount property in the shader
            if (workingZenLevel == 0)
            {
                //_zenMetreBarMaterials[0].SetFloat(_fillAmountPropertyID, fillAmount);
                _zenMetreBarMaterials[0].SetFloat("_FillAmount", fillAmount);
            }
            else if (workingZenLevel == 1)
            {
                _zenMetreBarMaterials[1].SetFloat(_fillAmountPropertyID, fillAmount);
            }
            else if (workingZenLevel == 2)
            {
                _zenMetreBarMaterials[2].SetFloat(_fillAmountPropertyID, fillAmount);
            }

            if (workingZenLevel < 3 && zenMetreBars[2].gameObject.activeSelf)
            {
                _zenMetreBarMaterials[2].SetFloat(_fillAmountPropertyID, 0f);
                zenMetreBars[2].gameObject.SetActive(false);
            }
            else if (workingZenLevel > 3 && !zenMetreBars[2].gameObject.activeSelf)
            {
                zenMetreBars[2].gameObject.SetActive(true);
            }

            if (workingZenLevel < 2 && zenMetreBars[1].gameObject.activeSelf && ZenMetreManager.Instance.zenLevelCheckpoint < 2)
            {
                _zenMetreBarMaterials[1].SetFloat(_fillAmountPropertyID, 0f);
                zenMetreBars[1].gameObject.SetActive(false);
            }
            else if (workingZenLevel > 2 && !zenMetreBars[1].gameObject.activeSelf)
            {
                zenMetreBars[1].gameObject.SetActive(true);
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Exception: " + e + " occurred while updating Zen Bar.");
        }
        
    }
    
    private void ShowSparkles()
    {
        try
        {
            foreach (GameObject sparkle in _sparkleList)
            {
                sparkle.SetActive(true);
            }
        }
        catch (Exception)
        {
            Debug.LogError("No sparkles");
        }
    }
    
    private void HideSparkles()
    {
        try
        {
            foreach (GameObject sparkle in _sparkleList)
            {
                sparkle.SetActive(false);
            }
        }
        catch (Exception)
        {
            Debug.LogError("No sparkles");
        }
    }
    
    private void ShowPromptText()
    {
        try
        {
            promptText.gameObject.SetActive(true);
            promptText.text = promptTextToBeShown;
        }
        catch (Exception)
        {
            Debug.LogError("No prompt text");
        }
    }
    
    private void HidePromptText()
    {
        try
        {
            promptText.gameObject.SetActive(false);
        }
        catch (Exception)
        {
            Debug.LogError("No prompt text");
        }
    }
}
