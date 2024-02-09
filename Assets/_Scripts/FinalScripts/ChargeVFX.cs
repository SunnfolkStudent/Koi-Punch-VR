using UnityEngine;
using UnityEngine.VFX;

public class ChargeVFX : MonoBehaviour
{
    [SerializeField] private VisualEffect _chargeVFXLeft;
    [SerializeField] private VisualEffect _chargeVFXRight;
    
    // Start is called before the first frame update
    private void Start()
    {
        InternalZenEventManager.startChargeVfx += PlayChargeVFX;
        InternalZenEventManager.stopChargeVfx += StopChargeVFX;
        
        StopChargeVFX();
    }

    private void PlayChargeVFX()
    {
        Debug.Log("Playing Charge VFX");
        _chargeVFXLeft.Play();
        _chargeVFXRight.Play();
    }

    private void StopChargeVFX()
    {
        _chargeVFXLeft.Stop();
        _chargeVFXRight.Stop();
    }
}
