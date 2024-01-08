using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackFieldScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DeathTimer());
    }
    
    private IEnumerator DeathTimer()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        ZenMetreManager.Instance.AddAttackFieldZen(transform.localScale.magnitude);
    }
}
