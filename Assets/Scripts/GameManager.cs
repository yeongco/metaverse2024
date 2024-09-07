using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public float[] sentiment;
    private void Start()
    {
        Instance = this;
    }
}
