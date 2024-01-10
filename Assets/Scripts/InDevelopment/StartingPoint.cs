using UnityEngine;
using Unity.XR.CoreUtils;

public class StartingPoint : MonoBehaviour
{
    public Transform head;
    public Transform origin;
    public Transform target;
    
    [ContextMenu("Test Starting Pos")]
    public void StartingPos()
    {
        Vector3 offset = head.position - origin.position;
        origin.position = target.position;
    }
    void Start()
    {
       StartingPos(); 
    }
}
