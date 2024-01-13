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
    
    [Header("Max Zen Metre Value")]
    private float _maxZenMetreValue = 100f;
    
    [Header("Sparkle List")]
    private List<GameObject> _sparkleList = new List<GameObject>();
    
    [Header("Prompt Text")]
    public TextMeshProUGUI promptText;
    
    [Header("Testing, please ignore")]
    private float _oldZenMetreValue;

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
        _zenMetreBarLevel1Material = zenMetreBarLevel1.material;
        _zenMetreBarLevel2Material = zenMetreBarLevel2.material;
        _zenMetreBarLevel3Material = zenMetreBarLevel3.material;
        
        _zenMetreBarLevel1Material.SetFloat("_FillAmount", 0);
        _zenMetreBarLevel2Material.SetFloat("_FillAmount", 0);
        _zenMetreBarLevel3Material.SetFloat("_FillAmount", 0);
        
        _sparkleList = GameObject.FindGameObjectsWithTag("Sparkle").ToList();
        HideSparkles();
        
        HidePromptText();
    }
    
    //Update is for testing purposes only
    void Update()
    {
        if (ZenMetreManager.Instance.zenMetreValue - _oldZenMetreValue > 0 || ZenMetreManager.Instance.zenMetreValue - _oldZenMetreValue < 0)
        {
            _oldZenMetreValue = ZenMetreManager.Instance.zenMetreValue;
            UpdateZenBar(ZenMetreManager.Instance.zenLevel, ZenMetreManager.Instance.zenMetreValue);
            ZenMetreManager.Instance.CheckForMaxZen();
        }
    }
    
    public void UpdateZenBar(int workingZenLevel, float zenMetreValue)
    {
        // Calculate fill amount based on the current Zen value
        float fillAmount = zenMetreValue / _maxZenMetreValue;

        // Ensure the fillAmount is within the range of 0 to 1
        fillAmount = Mathf.Clamp01(fillAmount);

        // Update the _FillAmount property in the shader
        if (workingZenLevel == 1)
        {
            _zenMetreBarLevel1Material.SetFloat("_FillAmount", fillAmount);
        }
        else if (workingZenLevel == 2)
        {
            _zenMetreBarLevel2Material.SetFloat("_FillAmount", fillAmount);
        }
        else if (workingZenLevel == 3)
        {
            _zenMetreBarLevel3Material.SetFloat("_FillAmount", fillAmount);
        }
        else
        {
            Debug.LogError("Zen level is not 1, 2 or 3");
        }
    }
    
    public void ShowSparkles()
    {
        foreach (GameObject sparkle in _sparkleList)
        {
            sparkle.SetActive(true);
        }
    }
    
    public void HideSparkles()
    {
        foreach (GameObject sparkle in _sparkleList)
        {
            sparkle.SetActive(false);
        }
    }
    
    public void ShowPromptText(string textToBePrompted)
    {
        promptText.gameObject.SetActive(true);
        promptText.text = textToBePrompted;
    }
    
    public void HidePromptText()
    {
        promptText.gameObject.SetActive(false);
    }
}
