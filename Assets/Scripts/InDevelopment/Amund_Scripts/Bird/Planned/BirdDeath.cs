using UnityEngine;

public class BirdDeath : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        print("Collision");
        //Play VFX
        Destroy(gameObject);
    }
}
