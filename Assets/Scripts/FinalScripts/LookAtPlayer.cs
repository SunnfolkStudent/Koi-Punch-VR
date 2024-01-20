using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    public Transform player;
    private Transform correctTransform;

    private void Start()
    {
        if (Camera.main != null) player = Camera.main.transform;
    }

    private void Update()
    {
        transform.LookAt(player);
    }
}