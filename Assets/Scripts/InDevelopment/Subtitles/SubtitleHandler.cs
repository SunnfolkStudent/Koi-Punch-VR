using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SubtitleHandler : MonoBehaviour
{
    private TextMeshProUGUI subtitleText;
    
    // Start is called before the first frame update
    void Start()
    {
        SubtitleEventManager.PlaySubtitle += ShowSubtitle;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void ShowSubtitle(string subtitle)
    {
        subtitleText.gameObject.SetActive(true);
        subtitleText.text = subtitle;
    }
    
    private void hideSubtitle()
    {
        subtitleText.gameObject.SetActive(false);
    }
}
