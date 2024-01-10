using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class RumbleTest : MonoBehaviour
{
   public XRBaseController leftController, rightController;

   public float intensity = 0.2f;
   public float duration;

   
   [ContextMenu("Send Haptics")]
   public void SendHaptics()
   {
      leftController.SendHapticImpulse(intensity, duration);
      rightController.SendHapticImpulse(intensity, duration);
   }
   
   [ContextMenu("Stop Haptics")]
   public void StopHaptics()
   {
      leftController.SendHapticImpulse(intensity, duration);
      rightController.SendHapticImpulse(intensity, duration);
   }
}
