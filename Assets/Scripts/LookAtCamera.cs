using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    Camera _cam;
    void Start()
    {
        _cam = Camera.main;
    }

    void Update()
    {
        transform.forward = _cam.transform.position - transform.position;
    }
}
