using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdCircular : MonoBehaviour
{
    [SerializeField] private Transform origio;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float radius;
    [SerializeField] private float degree;
    

    // Update is called once per frame
    void Update()
    {
        float x = origio.position.x + Mathf.Cos(degree) * radius;
        float y = origio.position.y;
        float z = origio.position.z + Mathf.Sin(degree) * radius;

        transform.position = new Vector3(x, y, z);

        degree += moveSpeed * Time.deltaTime;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        print("Collision");
        Destroy(gameObject);
    }
}
