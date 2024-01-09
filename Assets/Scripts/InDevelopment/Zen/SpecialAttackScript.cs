using UnityEngine;


public class SpecialAttackScript : MonoBehaviour
{
    [Header("Timers for charging punch")]
    private float _chargeTimer;
    private float _timeToCharge = 10f;
    private float _zenLostPerSecond;
    
    [Header("Bools for charging punch")]
    public bool chargingPunch;
    public static bool punchCharged;
    public static float punchForce;


    void Start()
    {
        _chargeTimer = _timeToCharge;
        
        _zenLostPerSecond = 300f / _timeToCharge;
    }
    
    void Update()
    {
        if (chargingPunch)
        {
            ZenMetreManager.Instance.zenMetreValue -= _zenLostPerSecond * Time.unscaledDeltaTime;
            
            if (ZenMetreManager.Instance.zenMetreValue <= 0 && ZenMetreManager.Instance.zenLevel != 1)
            {
                ZenMetreManager.Instance.zenLevel--;
                ZenMetreManager.Instance.zenMetreValue = 100f;
            }
            
            else if (ZenMetreManager.Instance.zenMetreValue <= 0 && ZenMetreManager.Instance.zenLevel == 1)
            {
                chargingPunch = false;
                punchCharged = true;
                ZenMetreManager.Instance.zenAttackActive = false;
                ZenMetreManager.Instance.zenMetreValue = 0f;
                CalculatePunchForce();
            }
            
            ZenMetreVisualManager.Instance.UpdateZenBar(ZenMetreManager.Instance.zenLevel, ZenMetreManager.Instance.zenMetreValue);
        }
    }
    
    private void CalculatePunchForce()
    {
        punchForce = (((4 - ZenMetreManager.Instance.zenLevel) * 100) - ZenMetreManager.Instance.zenMetreValue);
        ZenMetreVisualManager.Instance.UpdateZenBar(1, 0f);
        ZenMetreVisualManager.Instance.UpdateZenBar(2, 0f);
        ZenMetreVisualManager.Instance.UpdateZenBar(3, 0f);
    }
}
