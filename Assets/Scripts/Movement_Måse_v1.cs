using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement_Måse_v1 : MonoBehaviour
{
    public Transform target; // Target object to rotate around
    public float speed = 2f; // Speed of movement
    public float radius = 1f; // Radius of the circlar path
    public float angle = 0f; // Current angle of the object
    
    // Update is called once per frame
    void Update()
    {
        
        // Calculate the new position of the object using mathf.sin() and mathf.cos() functions

        float x = target.position.x + Mathf.Cos(angle) * radius;
        float y = target.position.y;
        float z = target.position.z + Mathf.Sin(angle) * radius;


        //Update the position of the new object

        transform.position = new Vector3(x, 15, z);

        //Increment the angle to move the object along the circular path

        angle += speed * Time.deltaTime;


    }
}
