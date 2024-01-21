using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideLevelsSign : MonoBehaviour
{
    void Start()
    {
        if (PlayerPrefs.GetInt("LevelsAvailable") == 0)
        {
            Destroy(gameObject);
        }
    }
}
