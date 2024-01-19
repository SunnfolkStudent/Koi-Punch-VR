using UnityEngine;


public class SpecialAttackScript : MonoBehaviour
{
    [Header("Timers for charging punch")]
    public static readonly float timeToCharge = 10f;
    private float _zenLostPerSecond;
    
    [Header("Tracking charge state")]
    public static bool chargingPunch;
    public static bool punchCharged;
    
    [Header("Static variables for special attack")]
    public static float punchForce;


    void Start()
    {
        _zenLostPerSecond = 300f / timeToCharge;
    }
    
    void Update()
    {
        //IF BUTTON WAS PRESSED
        //SET PROMPT TO OFF
        //SET CHARGING PUNCH TO TRUE
        if (ZenMetreManager.Instance.zenAttackActive && HapticManager.zenChargeing && !chargingPunch)
        {
            Debug.Log("Charging Punch");
            ZenMetreManager.Instance.zenAttackActive = true;
            InternalZenEventManager.hidePromptText.Invoke();
            chargingPunch = true;
            HapticManager.zenCharge = true;
        }
        
        //IF BUTTON WAS RELEASED
        //SET CHARGING PUNCH TO FALSE
        //SET PUNCH CHARGED TO TRUE
        //CALCULATE PUNCH FORCE
        if (!HapticManager.zenChargeing && chargingPunch)
        {
            chargingPunch = false;
            punchCharged = true;
            ZenMetreManager.Instance.zenAttackActive = false;
            HapticManager.zenCharge = false;
            CalculatePunchForce();
        }
        
        if (chargingPunch && !punchCharged)
        {
            ZenMetreManager.Instance.zenMetreValue -= _zenLostPerSecond * Time.unscaledDeltaTime;
            
            if (ZenMetreManager.Instance.zenMetreValue <= 0 && ZenMetreManager.Instance.zenLevel != 0)
            {
                ZenMetreManager.Instance.zenLevel--;
                ZenMetreManager.Instance.zenMetreValue = 100f;
            }
            
            else if (ZenMetreManager.Instance.zenMetreValue <= 0 && ZenMetreManager.Instance.zenLevel == 0)
            {
                chargingPunch = false;
                punchCharged = true;
                ZenMetreManager.Instance.zenAttackActive = false;
                ZenMetreManager.Instance.zenMetreValue = 0f;
                CalculatePunchForce();
            }
            
            InternalZenEventManager.updateVisualZenBar.Invoke();
        }
    }
    
    private void CalculatePunchForce()
    {
        punchForce = (((3 - ZenMetreManager.Instance.zenLevel) * 100) - ZenMetreManager.Instance.zenMetreValue);
        ZenMetreManager.Instance.zenLevel = 0;
        ZenMetreManager.Instance.zenMetreValue = 0f;
        InternalZenEventManager.updateVisualZenBar.Invoke();
        InternalZenEventManager.hideSparkles.Invoke();
    }
}
