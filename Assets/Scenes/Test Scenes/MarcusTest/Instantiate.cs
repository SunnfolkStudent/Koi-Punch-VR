using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instantiate : MonoBehaviour
{
    public GameObject cube;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnAfterWait());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    IEnumerator SpawnAfterWait()
    {
        yield return new WaitForSecondsRealtime(1);
        Instantiate(cube, new Vector3(-3, 0, 0), Quaternion.identity);
    }
}
