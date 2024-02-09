using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MenuEventManager : MonoBehaviour
{
    public static event Action ExplodeEvent;

    public static void ExplodeTransition()
    {
        if (ExplodeEvent != null)
        {
            ExplodeEvent();
        }
    }
}