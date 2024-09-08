using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;

public class TestClick : MonoBehaviour
{
    private SceneChanger Scenechanger;
    public string scene_name;

    private void Start()
    {
        Scenechanger = GameObject.Find("Circle").GetComponent<SceneChanger>();
    }
    private void OnMouseUpAsButton()
    {
        Debug.Log("click!");
        Scenechanger.StartTransition(scene_name);
    }
}
