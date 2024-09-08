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
    public static Action<GameObject> viewtalk;

    private void Awake()
    {
        viewsky = () => { ViewSky(); };
        viewquad = () => { ViewQuad(); };
        viewocean = () => { ViewOcean(); };
        viewtalk = (GameObject g) => { ViewTalk(g); };
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
        if (normal) //평소 쿼터뷰
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

    // Coroutine to adjust the camera over Time.deltaTime duration
    public IEnumerator ViewTalk(GameObject closestObject)
    {
        if (closestObject == null)
        {
            Debug.LogWarning("No closest object detected. Cannot adjust camera.");
            yield break;
        }

        // 플레이어와 NPC 사이의 중간 포지션을 계산
        Vector3 middlePoint = (cc.gameObject.transform.position + closestObject.transform.position) / 2;

        // 카메라가 바라볼 방향을 계산
        Vector3 directionToLook = (cc.gameObject.transform.position - closestObject.transform.position).normalized;

        // Y축 회전을 -45도에서 45도 사이로 제한
        float targetYAngle = Mathf.Clamp(Quaternion.LookRotation(directionToLook).eulerAngles.y, -45f, 45f);
        Vector3 targetAngle = new Vector3(30f, targetYAngle, this.Angle.z);
        Quaternion targetRotation = Quaternion.Euler(targetAngle);

        // 카메라의 초기 위치와 회전 저장
        Vector3 initialPosition = this.transform.position;
        Quaternion initialRotation = this.transform.rotation;

        // 카메라의 목표 위치 계산
        float cameraDistance = 4f; // 카메라가 떨어질 거리
        Vector3 cameraOffset = -targetAngle.normalized * cameraDistance; // 중간 지점으로부터 떨어진 위치 계산
        Vector3 targetPosition = middlePoint + cameraOffset; // 목표 위치

        // 부드러운 전환을 위한 보간
        float elapsedTime = 0f;
        while (elapsedTime < 1f)
        {
            // 보간 계수 계산
            float t = elapsedTime / 1f;

            // 카메라의 위치와 회전을 보간
            this.transform.position = Vector3.Lerp(initialPosition, targetPosition, t);
            this.transform.localRotation = Quaternion.Slerp(initialRotation, targetRotation, t);

            // 시간 경과 업데이트
            elapsedTime += Time.deltaTime;

            // 다음 프레임까지 대기
            yield return null;
        }

        // 최종 위치와 회전을 정확히 설정
        this.transform.position = targetPosition;
        this.transform.rotation = targetRotation;
    }





}
