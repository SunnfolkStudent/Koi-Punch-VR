using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMenus : BreakOnHit
{
    [SerializeField] private GameObject newMenuParent;

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "Player")
        {
            Debug.Log("collision detected");

            HittingSign();
            // make all other signs break

            // make new signs appear, instantiate
            Instantiate(newMenuParent);
        }
    }
}
