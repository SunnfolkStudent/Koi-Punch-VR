using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class SortOrder : MonoBehaviour
{
    public int SortingOrder = 570;
    public Renderer VfxRenderer = null;
 
    private void Awake()
    {
        VfxRenderer = GetComponent<Renderer>();
    }
 
    private void OnValidate()
    {
        if (VfxRenderer)
            VfxRenderer.sortingOrder = SortingOrder;
    }
}

