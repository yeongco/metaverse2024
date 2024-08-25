using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoPassZone : MonoBehaviour
{
    public Transform playerCamera;
    public float interactionDistance = 5f;
    public Canvas canvas;
    public Text interactionText;
    public string message = "거긴 아직 지나갈 수 없는 곳이야";
    private bool isInteracting = false;

    // 플레이어가 현재 이 오브젝트와 일정 거리에 있는 상태에서 E키를 누르면 텍스트 표시
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !isInteracting && PlayerCanSee.instance.closestObject == null)
        {
            RaycastHit hit;
            if (Physics.Raycast(playerCamera.position, playerCamera.forward, out hit, interactionDistance))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    StartCoroutine(ShowMessage(message));
                }
            }
        }
    }

    // 키보드로 메세지 보여주는 연출
    IEnumerator ShowMessage(string message)
    {
        isInteracting = true;
        interactionText.text = "";
        canvas.gameObject.SetActive(true);

        foreach (char c in message)
        {
            interactionText.text += c;
            yield return new WaitForSeconds(0.05f); // 타이핑 속도 조절
        }

        yield return new WaitForSeconds(1f); // 메시지를 일정 시간 동안 보여줌
        canvas.gameObject.SetActive(false);
        isInteracting = false;
    }

}
