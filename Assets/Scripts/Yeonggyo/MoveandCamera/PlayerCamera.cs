using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public CharacterController cc;
    public GameObject player;
    public float Lerppercentage = 1f;
    public Vector3 Quad = new Vector3(0f, 10f, -10f);
    public Vector3 Angle = new Vector3(30f, 0f, 0f);
    public Vector3 Skyposition = new Vector3(0f, 7f, 0f);
    public Vector3 Skyangle = new Vector3(30f, 0f, 0f);
    public Vector3 Oceanposition = new Vector3(4f, 2f, -6f);
    public Vector3 Oceanangle = new Vector3(0f, -30f, 0f);


    public static short state = 0;  // 0 : 일반 쿼터뷰 / 1 : 하늘보기 / 2 : 바다보기 / 3 : 대화중
    public static bool before = false;

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
        if (state == 0) //평소 쿼터뷰
        {
            ViewQuad();
        }
        else if (state == 1)
        {
            ViewSky();
        }
        else if (state == 2)
        {
            ViewOcean();
        }
    }

    void ViewQuad()
    {
        // 현재 회전과 목표 회전 설정
        Quaternion currentAngle = transform.rotation;
        Quaternion targetAngle = Quaternion.Euler(Angle);

        // 플레이어의 현재 위치를 기반으로 카메라 목표 위치 설정
        Vector3 capsule = player.transform.position;
        Vector3 targetPosition = capsule + Quad;

        // 보간 비율을 정하기 위해 t값을 계산 (Time.deltaTime을 통해 변화량을 점진적으로 증가)
        float t = Lerppercentage * Time.deltaTime;

        // 카메라의 위치와 회전을 보간
        transform.position = Vector3.Lerp(transform.position, targetPosition, t);
        transform.rotation = Quaternion.Lerp(currentAngle, targetAngle, t);

        // 목표 위치와 각도에 거의 도달했을 때 보간을 멈추도록 함
        if (Vector3.Distance(transform.position, targetPosition) < 0.01f && Quaternion.Angle(currentAngle, targetAngle) < 0.01f)
        {
            // 목표 위치와 각도로 바로 설정
            transform.position = targetPosition;
            transform.rotation = targetAngle;
        }
    }

    void ViewOcean()
    {
        // 현재 회전과 목표 회전 설정
        Quaternion currentAngle = transform.rotation;
        Quaternion targetAngle = Quaternion.Euler(Oceanangle);

        // 플레이어의 현재 위치를 기반으로 카메라 목표 위치 설정
        Vector3 capsule = player.transform.position;
        Vector3 targetPosition = capsule + Oceanposition;

        // 보간 비율을 정하기 위해 t값을 계산 (Time.deltaTime을 통해 변화량을 점진적으로 증가)
        float t = Lerppercentage * Time.deltaTime;

        // 카메라의 위치와 회전을 보간
        transform.position = Vector3.Lerp(transform.position, targetPosition, t);
        transform.rotation = Quaternion.Lerp(currentAngle, targetAngle, t);

        // 목표 위치와 각도에 거의 도달했을 때 보간을 멈추도록 함
        if (Vector3.Distance(transform.position, targetPosition) < 0.01f && Quaternion.Angle(currentAngle, targetAngle) < 0.01f)
        {
            // 목표 위치와 각도로 바로 설정
            transform.position = targetPosition;
            transform.rotation = targetAngle;
        }
    }

    void ViewSky()
    {
        // 현재 회전과 목표 회전 설정
        Quaternion currentAngle = transform.rotation;
        Quaternion targetAngle = Quaternion.Euler(Skyangle);

        // 플레이어의 현재 위치를 기반으로 카메라 목표 위치 설정
        Vector3 capsule = player.transform.position;
        Vector3 targetPosition = capsule + Skyposition;

        // 보간 비율을 정하기 위해 t값을 계산 (Time.deltaTime을 통해 변화량을 점진적으로 증가)
        float t = Lerppercentage * Time.deltaTime;

        // 카메라의 위치와 회전을 보간
        transform.position = Vector3.Lerp(transform.position, targetPosition, t);
        transform.rotation = Quaternion.Lerp(currentAngle, targetAngle, t);

        // 목표 위치와 각도에 거의 도달했을 때 보간을 멈추도록 함
        if (Vector3.Distance(transform.position, targetPosition) < 0.01f && Quaternion.Angle(currentAngle, targetAngle) < 0.01f)
        {
            // 목표 위치와 각도로 바로 설정
            transform.position = targetPosition;
            transform.rotation = targetAngle;
        }
    }

    // Coroutine to adjust the camera over Time.deltaTime duration
    public IEnumerator ViewTalk(GameObject closestObject, float cameraDistance)
    {
        if (closestObject == null)
        {
            Debug.LogWarning("No closest object detected. Cannot adjust camera.");
            yield break;
        }

        // 플레이어와 NPC 사이의 중간 지점을 계산
        Vector3 middlePoint = (cc.gameObject.transform.position + closestObject.transform.position) / 2;

        // 카메라가 바라볼 방향을 계산
        Vector3 directionToLook = (closestObject.transform.position - cc.gameObject.transform.position).normalized;

        // Y축 회전을 -45도에서 45도 사이로 제한
        float targetYAngle = Mathf.Clamp(Quaternion.LookRotation(directionToLook).eulerAngles.y, -45f, 45f);
        Quaternion targetRotation = Quaternion.Euler(Angle.x, targetYAngle, Angle.z);

        // 카메라의 초기 위치와 회전 저장
        Vector3 initialPosition = transform.position;
        Quaternion initialRotation = transform.rotation;

        // 카메라의 목표 위치 계산
        Vector3 cameraOffset = targetRotation * Vector3.forward * cameraDistance; // 중간 지점으로부터 떨어진 위치 계산
        Vector3 targetPosition = middlePoint - cameraOffset; // 목표 위치에 약간의 오프셋 추가

        // 부드러운 전환을 위한 보간
        float elapsedTime = 0f;
        while (elapsedTime < 1f)
        {
            if (state != 3)
                yield break;
            // 보간 계수 계산
            float t = elapsedTime / 1f;

            // 카메라의 위치와 회전을 보간
            transform.position = Vector3.Lerp(initialPosition, targetPosition, t);
            transform.rotation = Quaternion.Slerp(initialRotation, targetRotation, t);

            // 시간 경과 업데이트
            elapsedTime += Time.deltaTime;

            // 다음 프레임까지 대기
            yield return null;
        }

        // 최종 위치와 회전을 정확히 설정
        transform.position = targetPosition;
        transform.rotation = targetRotation;
    }

}
