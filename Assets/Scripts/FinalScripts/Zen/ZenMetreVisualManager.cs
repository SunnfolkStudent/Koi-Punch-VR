using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;

public class ZenMetreVisualManager : MonoBehaviour
{
    [Header("Zen Metre Bar Images and materials")]
    public GameObject[] zenMetreBars;
    private List<Material> _zenMetreBarMaterials = new List<Material>();
    private int _fillAmountPropertyID;
    private VisualEffect _sparkleVFX;
    
    [Header("Zen Metre Values")]
    private float _maxZenMetreValue = 100f;
    
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
                // Assuming the child you want to access is the first child, modify this according to your hierarchy
                Transform child = zenMeterbar.transform.GetChild(0);
                
                _zenMetreBarMaterials.Add(child.GetComponent<Image>().material);
            }

            _fillAmountPropertyID = Shader.PropertyToID("_FillAmount");
            
            foreach (Material zenMetrebarMaterial in _zenMetreBarMaterials)
            {
                zenMetrebarMaterial.SetFloat(_fillAmountPropertyID, 0f);
            }

            _sparkleVFX = zenMetreBars[2].GetComponent<VisualEffect>();
            
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
            switch (workingZenLevel)
            {
                case 0:
                    _zenMetreBarMaterials[0].SetFloat(_fillAmountPropertyID, fillAmount);
                    break;
                case 1:
                    _zenMetreBarMaterials[1].SetFloat(_fillAmountPropertyID, fillAmount);
                    break;
                case 2:
                    _zenMetreBarMaterials[2].SetFloat(_fillAmountPropertyID, fillAmount);
                    break;
            }

            //Turns on/off the second zen bar
            TurnOnOrOffZenbar(1, workingZenLevel);
            //Turns on/off the third zen bar
            TurnOnOrOffZenbar(2, workingZenLevel);
        }
        catch (Exception e)
        {
            Debug.LogError("Exception: " + e + " occurred while updating Zen Bar.");
        }
    }
    
    private void TurnOnOrOffZenbar(int zenbarIndex, int workingZenLevel)
    {
        if (zenMetreBars[zenbarIndex].gameObject.activeSelf && workingZenLevel < 1 && ZenMetreManager.Instance.zenLevelCheckpoint < (zenbarIndex + 1))
        {
            _zenMetreBarMaterials[zenbarIndex].SetFloat(_fillAmountPropertyID, 0f);
            zenMetreBars[zenbarIndex].gameObject.SetActive(false);
        }
        else if (!zenMetreBars[zenbarIndex].gameObject.activeSelf && workingZenLevel >= 1)
        {
            zenMetreBars[zenbarIndex].gameObject.SetActive(true);
        }
    }
    
    private void ShowSparkles()
    {
        try
        {
            foreach (GameObject sparkle in _sparkleList)
            {
                sparkle.SetActive(true);
                StartSparklesVFX();
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
                StopSparklesVFX();
            }
        }
        catch (Exception)
        {
            Debug.LogError("No sparkles");
        }
    }
    
    private void StartSparklesVFX()
    {
        try
        {
            _sparkleVFX.Play();
        }
        catch (Exception)
        {
            Debug.LogError("No sparkles");
        }
    }
    
    private void StopSparklesVFX()
    {
        try
        {
            _sparkleVFX.Stop();
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
