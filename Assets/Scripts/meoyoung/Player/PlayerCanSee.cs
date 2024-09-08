using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class PlayerCanSee : MonoBehaviour
{
    public static PlayerCanSee instance;
    public float detectionAngle = 40f;
    public float detectionDistance = 10f;
    public Color detectionZoneColor = new Color(1, 0, 0, 0.3f); // 투명한 빨간색
    public GameObject STTS;
    public GameObject closestObject = null;
    private PlayerMove playerMove;
    public GameObject STTSChatUI;
    public GameObject custom;
    public PlayerCamera playerCamera;

    private void Start()
    {
        if (instance != null)
        {
            GameObject.Destroy(this);
        }
        else
        {
            instance = this;
        }
        playerMove = this.gameObject.GetComponent<PlayerMove>();
    }
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
            if(hitCollider.name == "Fountain")
            {
                custom.SetActive(true);
                Time.timeScale = 0f;
                Cursor.lockState = CursorLockMode.None; // 커서 락 해제
                Cursor.visible = true; // 커서 보이게 설정
            }

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
                        //카메라 설정 추가
                       /* PlayerCamera.sky = false;
                        PlayerCamera.ocean = false;
                        PlayerCamera.normal = false;
                        StartCoroutine(playerCamera.ViewTalk(closestObject));
                        
                        PlayerCamera.viewtalk(closestObject);*/

                        GetClosestObject();
                    }
                }
            }
        }

        if (closestObject != null)
        {
            Debug.Log(closestObject.gameObject.name);
            //closestObject.GetComponent<NPCController>().ChangeState(closestObject.GetComponent<NPCController>()._lootatState);
            //STTS.gameObject.SetActive(true);
            SetMovementUnAvailable();
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
    //대화 상대가 감지되면 플레이어의 움직임을 대화 종료까지 금지,NPC는 상태를 idle로 고정, 각종 움직임에 관여하는 스크립트, 컴포넌트 비활성화
    void SetMovementUnAvailable()
    {
        playerMove.moveAvailable = false;
        playerMove.dir = new Vector3(0, 0, 0);
        this.gameObject.GetComponent<CharacterController>().enabled = false;

        if (closestObject.name == "nsangdo")
        {
            closestObject.gameObject.GetComponent<KimController>().CurrentState = gameObject.GetComponent<KimIdleState>();
            closestObject.gameObject.GetComponent<KimController>().enabled = false;
        }
        else if (closestObject.name == "nyuna")
        {
            closestObject.gameObject.GetComponent<YunaController>().CurrentState = gameObject.GetComponent<YunaIdleState>();
            closestObject.gameObject.GetComponent<YunaController>().enabled = false;
        }
        else
        {
            closestObject.gameObject.GetComponent<NavMeshAgent>().isStopped = true; // NPC 움직임 멈춤
            closestObject.gameObject.GetComponent<NPCController>().CurrentState = gameObject.GetComponent<NPCIdleState>();
            closestObject.gameObject.GetComponent<NPCController>().enabled = false;
        }

        StartCoroutine(RotateTowardsTarget(closestObject, this.gameObject));
        StartCoroutine(RotateTowardsTarget(this.gameObject, closestObject));
        //카메라 설정 추가
/*        PlayerCamera.sky = false;
        PlayerCamera.ocean = false;
        PlayerCamera.normal = false;
        StartCoroutine(playerCamera.ViewTalk(closestObject));

        PlayerCamera.viewtalk(closestObject);*/
        STTS.SetActive(true);
        STTSChatUI.SetActive(true);
        this.gameObject.GetComponentInChildren<Animator>().SetBool("IsWalk", false);
    }
    private IEnumerator RotateTowardsTarget(GameObject a, GameObject b)
    {
        while (true)
        {
            // 타겟 방향 계산
            Vector3 c = (b.transform.position - a.transform.position).normalized;
            Vector3 direction = new Vector3(c.x, 0, c.z).normalized;
            // 회전할 각도 계산
            Quaternion lookRotation = Quaternion.LookRotation(direction);

            // 현재 회전과 목표 회전 사이의 각도 차이 계산
            float angleDifference = Quaternion.Angle(a.transform.rotation, lookRotation);

            // 각도 차이가 임계값 이하인지 확인
            if (angleDifference < 5)
            {
                // 임계값 이하이면 회전을 멈춤
                yield break;
            }

            // 점진적으로 회전
            a.transform.rotation = Quaternion.Slerp(a.transform.rotation, lookRotation, Time.deltaTime * 7);

            // 한 프레임 대기
            yield return null;
        }
    }
}
