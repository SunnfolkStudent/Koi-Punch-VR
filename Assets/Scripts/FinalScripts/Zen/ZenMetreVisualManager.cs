using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ZenMetreVisualManager : MonoBehaviour
{
    public static ZenMetreVisualManager Instance;
    
    [Header("Zen Metre Bar Images and materials")]
    public Image zenMetreBarLevel1;
    public Image zenMetreBarLevel2;
    public Image zenMetreBarLevel3;
    private Material _zenMetreBarLevel1Material;
    private Material _zenMetreBarLevel2Material;
    private Material _zenMetreBarLevel3Material;
    
    [Header("Zen Metre Values")]
    private float _maxZenMetreValue = 100f;
    private int _oldZenLevel;
    
    [Header("Sparkle List")]
    private List<GameObject> _sparkleList = new List<GameObject>();
    
    [Header("Prompt Text")]
    public TextMeshProUGUI promptText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        try
        {
            _zenMetreBarLevel1Material = zenMetreBarLevel1.material;
            _zenMetreBarLevel2Material = zenMetreBarLevel2.material;
            _zenMetreBarLevel3Material = zenMetreBarLevel3.material;
        
            _zenMetreBarLevel1Material.SetFloat("_FillAmount", 0);
            _zenMetreBarLevel2Material.SetFloat("_FillAmount", 0);
            _zenMetreBarLevel3Material.SetFloat("_FillAmount", 0);
        }
        catch (Exception e)
        {
            Debug.LogError("Exception: " + e + " occurred while initializing Zen Metre Bar.");
        }
        
        
        _sparkleList = GameObject.FindGameObjectsWithTag("Sparkle").ToList();
        HideSparkles();
        
        HidePromptText();
    }
    
    public void UpdateZenBar(int workingZenLevel, float zenMetreValue)
    {
        try
        {
            // Calculate fill amount based on the current Zen value
            float fillAmount = zenMetreValue / _maxZenMetreValue;

            // Ensure the fillAmount is within the range of 0 to 1
            fillAmount = Mathf.Clamp01(fillAmount);

            // Update the _FillAmount property in the shader
            if (workingZenLevel == 0)
            {
                _zenMetreBarLevel1Material.SetFloat("_FillAmount", fillAmount);
            }
            else if (workingZenLevel == 1)
            {
                zenMetreBarLevel2.gameObject.SetActive(true);
                _zenMetreBarLevel2Material.SetFloat("_FillAmount", fillAmount);
                if (zenMetreValue <= 0)
                {
                    zenMetreBarLevel2.gameObject.SetActive(false);
                }
            }
            else if (workingZenLevel == 2)
            {
                zenMetreBarLevel3.gameObject.SetActive(true);
                _zenMetreBarLevel3Material.SetFloat("_FillAmount", fillAmount);
                if (zenMetreValue <= 0)
                {
                    zenMetreBarLevel3.gameObject.SetActive(false);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Exception: " + e + " occurred while updating Zen Bar.");
        }
        
    }
    
    public void ShowSparkles()
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
    
    public void HideSparkles()
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
    
    public void ShowPromptText(string textToBePrompted)
    {
        try
        {
            promptText.gameObject.SetActive(true);
            promptText.text = textToBePrompted;
        }
        catch (Exception)
        {
            Debug.LogError("No prompt text");
        }
    }
    
    public void HidePromptText()
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
