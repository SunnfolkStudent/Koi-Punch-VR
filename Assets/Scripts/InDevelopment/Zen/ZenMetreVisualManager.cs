using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZenMetreVisualManager : MonoBehaviour
{
    public Image zenMetreBarLevel1;
    public Image zenMetreBarLevel2;
    public Image zenMetreBarLevel3;
    private Material _zenMetreBarLevel1Material;
    private Material _zenMetreBarLevel2Material;
    private Material _zenMetreBarLevel3Material;
    
    private float _maxZenMetreValue = 100f;
    private float _oldZenMetreValue;
    
    // Start is called before the first frame update
    void Start()
    {
        _zenMetreBarLevel1Material = zenMetreBarLevel1.material;
        _zenMetreBarLevel2Material = zenMetreBarLevel2.material;
        _zenMetreBarLevel3Material = zenMetreBarLevel3.material;
        
        _zenMetreBarLevel1Material.SetFloat("_FillAmount", 0);
        _zenMetreBarLevel2Material.SetFloat("_FillAmount", 0);
        _zenMetreBarLevel3Material.SetFloat("_FillAmount", 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (ZenMetreManager.Instance.zenMetreValue - _oldZenMetreValue > 0 || ZenMetreManager.Instance.zenMetreValue - _oldZenMetreValue < 0)
        {
            _oldZenMetreValue = ZenMetreManager.Instance.zenMetreValue;
            UpdateZenBar(ZenMetreManager.Instance.zenMetreValue);
        }
    }
    
    public void UpdateZenBar(float currentZenValue)
    {
        // Calculate fill amount based on the current Zen value
        float fillAmount = currentZenValue / _maxZenMetreValue;

        // Ensure the fillAmount is within the range of 0 to 1
        fillAmount = Mathf.Clamp01(fillAmount);

        // Update the _FillAmount property in the shader
        if (ZenMetreManager.Instance.zenLevel == 1)
        {
            _zenMetreBarLevel1Material.SetFloat("_FillAmount", fillAmount);
        }
        else if (ZenMetreManager.Instance.zenLevel == 2)
        {
            _zenMetreBarLevel2Material.SetFloat("_FillAmount", fillAmount);
        }
        else if (ZenMetreManager.Instance.zenLevel == 3)
        {
            _zenMetreBarLevel3Material.SetFloat("_FillAmount", fillAmount);
        }
        else
        {
            Debug.LogError("Zen level is not 1, 2 or 3");
        }
    }
}
