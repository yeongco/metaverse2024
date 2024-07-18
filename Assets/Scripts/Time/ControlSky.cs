using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.Serialization;

public class ControlSky : MonoBehaviour
{
    [Header("시간 설정")]
    [SerializeField]
    float NowSecond;
    public float Onedaymunites = 10;
    public float startHour = 12;
    public float sunriseHour = 6;
    public float sunsetHour = 18;
    [SerializeField]
    float GameSecond;
    [SerializeField]
    float sunriseSecond;
    [SerializeField]
    float sunsetSecond;

    private void Start()
    {
        GameSecond = 86400 / (Onedaymunites * 60);
        NowSecond = startHour * 60 * 60;
        sunriseSecond = sunriseHour * 60 * 60;
        sunsetSecond = sunsetHour * 60 * 60;
        RenderSettings.skybox.SetFloat("_Blend", 0);
        StartCoroutine(Timer());

    }

    IEnumerator Timer()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            NowSecond += GameSecond;
            if (NowSecond >= 86400)
                NowSecond -= 86400;
        }
    }

    void Update()
    {
        if (NowSecond >= sunriseSecond && NowSecond < sunriseSecond + GameSecond)
        {
            StopCoroutine(SunRise());
            StartCoroutine(SunRise());
        }
        else if (NowSecond >= sunsetSecond && NowSecond < sunsetSecond + GameSecond)
        {
            StopCoroutine(SunSet());
            StartCoroutine(SunSet());
        }
        RenderSettings.skybox.SetFloat("_Rotation", Time.time);
    }

    IEnumerator SunRise()
    {
        float duration = 10.0f;
        float elapsedTime = 0.0f;

        while (elapsedTime < duration)
        {
            RenderSettings.skybox.SetFloat("_Blend", Mathf.Lerp(1.0f, 0.0f, elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        RenderSettings.skybox.SetFloat("_Blend", 0.0f);
    }

    IEnumerator SunSet()
    {
        float duration = 10.0f;
        float elapsedTime = 0.0f;

        while (elapsedTime < duration)
        {
            RenderSettings.skybox.SetFloat("_Blend", Mathf.Lerp(0.0f, 1.0f, elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        RenderSettings.skybox.SetFloat("_Blend", 1.0f);
    }
}
