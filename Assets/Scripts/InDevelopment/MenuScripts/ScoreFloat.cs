using TMPro;
using UnityEngine;
using TMPro;

public class ScoreFloat : MonoBehaviour
{
    [SerializeField] private TMP_Text floatingText;
    private void Start()
    {
        
        Destroy(gameObject, 4f);
    }
    
    
}

