using UnityEditor.EditorTools;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    [ContextMenu("SetRandomRot")]
    void SetRandomRot()
    {
        var trans = transform.eulerAngles;

        trans.y = Random.Range(0, 360);

        transform.eulerAngles = new Vector3(trans.x, trans.y, trans.z);
    }

}