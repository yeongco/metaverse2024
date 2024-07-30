using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class PlayerCanSee : MonoBehaviour
{
    public float detectionAngle = 40f;
    public float detectionDistance = 10f;
    public Color detectionZoneColor = new Color(1, 0, 0, 0.3f); // 투명한 빨간색

    private GameObject closestObject = null;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            DetectAndMoveObject();
        }
    }

    // 플레이어의 감지 반경에 들어온 모든 오브젝트를 hitColliders에 저장
    // 각각의 hitColliders 마다 NPC라는 태그를 가진 오브젝트중 거리가 가까운 오브젝트를 closestObject에 저장
    // closestObject의 State를 lookat state로 전환
    void DetectAndMoveObject()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionDistance);
        float closestDistance = Mathf.Infinity;

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("NPC"))
            {
                Vector3 directionToObject = hitCollider.transform.position - transform.position;
                float angle = Vector3.Angle(transform.forward, directionToObject);

                if (angle < detectionAngle)
                {
                    float distanceToObject = directionToObject.magnitude;
                    if (distanceToObject < closestDistance)
                    {
                        closestDistance = distanceToObject;
                        closestObject = hitCollider.gameObject;
                        GetClosestObject();
                    }
                }
            }
        }

        if (closestObject != null)
        {
            NPCController _npcController = closestObject.GetComponent<NPCController>();
            _npcController.ChangeState(_npcController._lootatState);
        }
    }

    // 플레이어의 탐지 반경을 화면에 출력함
    // 런타임 이후는 출력되지 않음
    void OnDrawGizmos()
    {
        Gizmos.color = detectionZoneColor;
        Vector3 forward = transform.forward * detectionDistance;

        for (float angle = -detectionAngle; angle <= detectionAngle; angle += 1f)
        {
            Vector3 direction = Quaternion.Euler(0, angle, 0) * forward;
            Gizmos.DrawRay(transform.position, direction);
        }
    }


    // Log창에 플레이어에게 탐지된 NPC 출력
    void GetClosestObject()
    {
        Debug.Log(closestObject);
    }
}
