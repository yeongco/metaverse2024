using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateDiary : MonoBehaviour
{
    public STTS_save save;
    // Start is called before the first frame update
    void OnEnable()
    {
        StartCoroutine(save.SendRequestToGPT(save.currentString));
    }

}
