using TMPro;
using UnityEngine;

public class ScoreFloat : MonoBehaviour
{
    [SerializeField] private TMP_Text floatingText;
    private void Start()
    {
        
        Destroy(gameObject, 4f);
    }
    
    
}

