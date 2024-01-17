using UnityEngine;

public class ScoreFloat : MonoBehaviour
{
    //[SerializeField] private float destroyAfterTime;
    private void Start()
    {
        Destroy(gameObject, 4f);
    }
}

