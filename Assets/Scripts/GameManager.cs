using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum _sentiment
{
    nyuna = 0,
    ndain = 1,
    nminsang = 2,
    nsangdo = 3
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public float[] sentiment;
    public int talkingTimes = 0;
    public int maxPerson = -100;
    private void Start()
    {
        Instance = this;
    }
}
