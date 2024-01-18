using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereCast : MonoBehaviour
{
    public Vector3 cubePosition;
    public Vector3 cubePosition2;
    
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0.00001f;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        
        float distanceToObstacle = 0;

        // Cast a sphere wrapping character controller 10 meters forward
        // to see if it is about to hit anything.
        if (Physics.SphereCast(gameObject.transform.position, 1f, cubePosition, out hit, 0.1f))
        {
            distanceToObstacle = hit.distance;
            Debug.Log(distanceToObstacle + "1");
        }
        
        if (Physics.SphereCast(gameObject.transform.position, 1f, -cubePosition, out hit, 0.1f))
        {
            distanceToObstacle = hit.distance;
            Debug.Log(distanceToObstacle + "2");
        }
        
        if (Physics.SphereCast(gameObject.transform.position, 1f, cubePosition2, out hit, 0.1f))
        {
            distanceToObstacle = hit.distance;
            Debug.Log(distanceToObstacle + "3");
        }
        
        if (Physics.SphereCast(gameObject.transform.position, 1f, -cubePosition2, out hit, 0.1f))
        {
            distanceToObstacle = hit.distance;
            Debug.Log(distanceToObstacle + "4");
        }
        
        
        Debug.DrawRay(transform.position, cubePosition * 0.1f, Color.red);
    }
}
