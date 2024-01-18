using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShatteredPieces : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody rb;
    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        Destroy(gameObject,5f);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (rb == null) return;
        if(other.gameObject.CompareTag("Ground")) return;
        rb.AddForce(other.rigidbody.velocity*500);
    }
    
    
}
