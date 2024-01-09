using System.Collections;
using System.Collections.Generic;
using Unity.XR.OpenVR;
using UnityEngine;

public class SpecialAttackScript : MonoBehaviour
{
    public bool chargingPunch;
    public bool punchCharged;
    private float _timeToCharge = 5;
    private float _chargeTimer;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (chargingPunch)
        {
            _chargeTimer -= Time.deltaTime;
            if (_chargeTimer <= 0)
            {
                chargingPunch = false;
                punchCharged = true;
            }
        }
        else if (_chargeTimer < _timeToCharge)
        {
            _chargeTimer = _timeToCharge;
        }
    }
}
