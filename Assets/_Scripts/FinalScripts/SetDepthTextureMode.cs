using UnityEngine;
 
[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class SetDepthTextureMode : MonoBehaviour
{
    public bool EnableDepthTexture;
 
    void SetDepthTextureModes()
    {
        Camera cam = GetComponent<Camera>();
        if (EnableDepthTexture)
            cam.depthTextureMode |= DepthTextureMode.Depth;
        else
            cam.depthTextureMode &= ~DepthTextureMode.Depth;
    }
 
    void OnEnable()
    {
        SetDepthTextureModes();
    }
 
#if UNITY_EDITOR
    void OnValidate()
    {
        SetDepthTextureModes();
    }
#endif
}