using UnityEngine;
using UnityEngine.VFX;

public class ChargeVFX : MonoBehaviour
{
    [SerializeField] private VisualEffect _chargeVFXLeft;
    [SerializeField] private VisualEffect _chargeVFXRight;
    
    // Start is called before the first frame update
    void Start()
    {
        InternalZenEventManager.startChargeVfx += PlayChargeVFX;
        InternalZenEventManager.stopChargeVfx += StopChargeVFX;
    }
    
    public void PlayChargeVFX()
    {
        _chargeVFXLeft.Play();
        _chargeVFXRight.Play();
    }
    
    public void StopChargeVFX()
    {
        _chargeVFXLeft.Stop();
        _chargeVFXRight.Stop();
    }
}
