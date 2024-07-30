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
    public Light sun;
    public Light moon;


    private void Start()
    {
        GameSecond = 86400 / (Onedaymunites * 60);
        NowSecond = startHour * 60 * 60;
        sunriseSecond = sunriseHour * 60 * 60;
        sunsetSecond = sunsetHour * 60 * 60;
        if (NowSecond >= sunriseSecond && NowSecond < sunsetSecond)
        {
            RenderSettings.skybox.SetFloat("_Blend", 0);
            sun.intensity = 0.7f;
            moon.intensity = 0.0f;
        }
        else
        {
            RenderSettings.skybox.SetFloat("_Blend", 1);
            sun.intensity = 0.0f;
            moon.intensity = 0.6f;
        }
        StartCoroutine(Timer());
        StartCoroutine(SunMove());

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
        if (NowSecond >= sunriseSecond && NowSecond < sunsetSecond)
            StartCoroutine(SunMove());
    }

    IEnumerator SunRise()
    {
        float duration = 10.0f;
        float elapsedTime = 0.0f;

        while (elapsedTime < duration)
        {
            RenderSettings.skybox.SetFloat("_Blend", Mathf.Lerp(1.0f, 0.0f, elapsedTime / duration));
            sun.intensity = Mathf.Lerp(0.0f, 0.7f, elapsedTime / duration);
            moon.intensity = Mathf.Lerp(0.6f, 0.0f, elapsedTime / duration);
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
            sun.intensity = Mathf.Lerp(0.7f, 0.0f, elapsedTime / duration);
            moon.intensity = Mathf.Lerp(0.0f, 0.6f, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        RenderSettings.skybox.SetFloat("_Blend", 1.0f);
    }

    IEnumerator SunMove()
    {
        if (NowSecond >= sunriseSecond && NowSecond < sunsetSecond)
        {
            float dayProgress = (NowSecond - sunriseSecond) / (sunsetSecond - sunriseSecond);
            float NowRotation = Mathf.Lerp(0.0f, 180.0f, dayProgress);
            sun.transform.eulerAngles = new Vector3(NowRotation, 30.0f, 0.0f);
            float limit_time = (sunsetSecond - NowSecond) / GameSecond + 10.0f;
            float elapsedTime = 0.0f;
            while (elapsedTime < limit_time)
            {
                float targetRotation = Mathf.Lerp(NowRotation, 180.0f, elapsedTime / limit_time);
                sun.transform.eulerAngles = new Vector3(targetRotation, 30.0f, 0.0f);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            // 태양을 부드럽게 회전시킴
        }
    }
}
