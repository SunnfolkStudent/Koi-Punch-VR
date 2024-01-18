using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlapSphereTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0.00001f;
    }

    // Update is called once per frame
    void Update()
    {
        Collider[] hitColliders = Physics.OverlapSphere(gameObject.transform.position, 1);
        foreach (var hitCollider in hitColliders)
        {
            Debug.Log("1");
        }
    }
}
