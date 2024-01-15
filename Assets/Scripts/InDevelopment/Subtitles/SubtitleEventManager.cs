using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SubtitleEventManager
{
    public delegate void SubtitleEvents(string subtitle);
    public static event SubtitleEvents PlaySubtitle;
}
