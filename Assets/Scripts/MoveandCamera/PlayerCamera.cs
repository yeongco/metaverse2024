using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public CharacterController cc;
    public float Lerppercentage = 1f;
    public Vector3 Quad = new Vector3(0f, 10f, -10f);
    public Vector3 Angle = new Vector3(30f, 0f, 0f);
    public Vector3 Skyposition = new Vector3(0f, 7f, 0f);
    public Vector3 Skyangle = new Vector3(30f, 0f, 0f);
    
    public static bool normal = true;

    public static Action viewsky;
    public static Action viewquad;

    private void Awake()
    {
        viewsky = () => { ViewSky(); };
        viewquad = () => { ViewQuad(); };
    }


    void Start()
    {
        Vector3 capsule = cc.transform.position;
        transform.position = capsule + Quad;
        transform.eulerAngles = Angle;
    }

    // Update is called once per frame
    void Update()
    {
        if (normal) //∆Úº“ ƒı≈Õ∫‰
        {
            ViewQuad();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("enter");

    }

    void OnTriggerExit(Collider other)
    {
        Debug.Log("exit");
        normal = true;
    }

    void ViewQuad()
    {
        Vector3 capsule = cc.transform.position;
        transform.position = Vector3.Lerp(transform.position, capsule + Quad, Time.deltaTime * Lerppercentage);
        transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, Angle, Time.deltaTime * Lerppercentage);
    }

    void ViewSky()
    {
        normal = false;
        Vector3 capsule = cc.transform.position;
        transform.position = Vector3.Lerp(transform.position, capsule + Skyposition, Time.deltaTime * Lerppercentage);
        transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, Skyangle, Time.deltaTime * Lerppercentage);
    }
}
