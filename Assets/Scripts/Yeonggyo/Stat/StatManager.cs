using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UIElements;

public class StatManager : MonoBehaviour
{
    static Dictionary<string, short> stats = new Dictionary<string, short>();

    void Start()
    {
        stats.Add("TalkCount", 0);
        stats.Add("Days", 0);
        stats.Add("WalkCount", 0);
        stats.Add("RealTime", 0);
        stats.Add("SkyTime", 0);
        stats.Add("BeachTime", 0);
        stats.Add("EmotionCount", 0);
    }

    public void AddCounter(string name)
    {
        stats[name] += 1;
    }
}
