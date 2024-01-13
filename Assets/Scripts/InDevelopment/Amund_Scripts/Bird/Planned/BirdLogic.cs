using UnityEngine;
using UnityEngine.Splines;

public class BirdLogic : MonoBehaviour
{
    [SerializeField] private GameObject NewBird;
    private BirdRandomPathing _switchPath;
    
    private void Start()
    {
        _switchPath = gameObject.GetComponent<BirdRandomPathing>();
    }

    private void Update()
    {
        if (_switchPath.ElapsedTime >= _switchPath.Duration)
        {
            Switch();
        }
    }
    
    private void Switch()
    {
        NewBird.SetActive(true);
        gameObject.SetActive(false);
    }
    
}
