using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShatteredPieces : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody _rb;
    private void Start()
    {
        _rb = gameObject.GetComponent<Rigidbody>();
        Destroy(gameObject,5f);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (_rb == null) return;
        if(other.gameObject.CompareTag("Ground")) return;
        
        _rb.AddForce(other.rigidbody.velocity * 250);
        
        /*if(other.gameObject.CompareTag("RightFist") || other.gameObject.CompareTag("LeftFist"))
        {
            _rb.AddForce(other.rigidbody.velocity * 250);
        }
        else
        {
            _rb.AddForce(other.rigidbody.velocity * 200);
        }*/
    }
    
    
}
