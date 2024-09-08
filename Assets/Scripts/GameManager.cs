using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public float[] sentiment;
    public int talkingTimes = 0;
    private void Start()
    {
        Instance = this;
    }
}
