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
    public Vector3 Oceanposition = new Vector3(4f, 2f, -6f);
    public Vector3 Oceanangle = new Vector3(0f, -30f, 0f);
    
    public static bool normal = true;
    public static bool sky = false;
    public static bool ocean = false;


    public static Action viewsky;
    public static Action viewquad;
    public static Action viewocean;

    private void Awake()
    {
        viewsky = () => { ViewSky(); };
        viewquad = () => { ViewQuad(); };
        viewocean = () => { ViewOcean(); };
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
        else if (sky)
        {
            ViewSky();
        }
        else if (ocean)
        {
            ViewOcean();
        }
    }

    void ViewQuad()
    {
        Quaternion currentangle = transform.rotation;
        Quaternion targetangle = Quaternion.Euler(Angle);

        Vector3 capsule = cc.transform.position;
        transform.position = Vector3.Lerp(transform.position, capsule + Quad, Time.deltaTime * Lerppercentage);
        transform.rotation = Quaternion.Lerp(currentangle, targetangle, Time.deltaTime * Lerppercentage);
    }

    void ViewSky()
    {
        Vector3 capsule = cc.transform.position;
        transform.position = Vector3.Lerp(transform.position, capsule + Skyposition, Time.deltaTime * Lerppercentage);
        transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, Skyangle, Time.deltaTime * Lerppercentage);
    }

    void ViewOcean()
    {
        Quaternion currentangle = transform.rotation;
        Quaternion targetangle = Quaternion.Euler(Oceanangle);

        Vector3 capsule = cc.transform.position;
        transform.position = Vector3.Lerp(transform.position, capsule + Oceanposition, Time.deltaTime * Lerppercentage);
        transform.rotation = Quaternion.Lerp(currentangle, targetangle, Time.deltaTime * Lerppercentage);
    }
}
