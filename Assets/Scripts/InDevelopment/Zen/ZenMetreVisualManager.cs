using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZenMetreVisualManager : MonoBehaviour
{
    public GameObject zenMetreBar;
    
    private float _oldZenMetreValue;
    private int _localScaleFactor = 10;
    
    // Start is called before the first frame update
    void Start()
    {
        zenMetreBar.transform.localScale = new Vector3(ZenMetreManager.Instance.zenMetreValue / _localScaleFactor, 1, 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (ZenMetreManager.Instance.zenMetreValue - _oldZenMetreValue > 0 || ZenMetreManager.Instance.zenMetreValue - _oldZenMetreValue < 0)
        {
            _oldZenMetreValue = ZenMetreManager.Instance.zenMetreValue;
            zenMetreBar.transform.localScale = new Vector3(ZenMetreManager.Instance.zenMetreValue / _localScaleFactor, 1, 1);
        }
    }
}
