using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class PlayerCanSee : MonoBehaviour
{
    public float detectionAngle = 40f;
    public float detectionDistance = 10f;
    public Color detectionZoneColor = new Color(1, 0, 0, 0.3f); // 투명한 빨간색


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("E key pressed");
            DetectAndMoveObject();
        }
    }

    void DetectAndMoveObject()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionDistance);
        GameObject closestObject = null;
        float closestDistance = Mathf.Infinity;

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("NPC"))
            {
                Debug.Log("NPC object detected");
                Vector3 directionToObject = hitCollider.transform.position - transform.position;
                float angle = Vector3.Angle(transform.forward, directionToObject);

                if (angle < detectionAngle)
                {
                    float distanceToObject = directionToObject.magnitude;
                    if (distanceToObject < closestDistance)
                    {
                        closestDistance = distanceToObject;
                        closestObject = hitCollider.gameObject;
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
}
