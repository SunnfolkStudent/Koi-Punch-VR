using UnityEngine;
using VHierarchy.Libs;

public class BirdDeath : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        print("Collision");
        //Play VFX
        gameObject.Destroy();
    }
}
