using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseDirectionAnimation : MonoBehaviour
{

    public Animator animator;

    private Vector3 initialMousePosition;
    private bool isGKeyPressed;

    void Start()
    {
        // 시작할 때 마우스 커서를 숨김
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // g 키를 누를 때 마우스 커서를 화면 중앙으로 이동시키고 표시
        // 'A' 키가 눌리면 일반 메뉴 열기
        if (Input.GetKeyDown(KeyCode.G))
        {
            isGKeyPressed = true;
            initialMousePosition = new Vector3(Screen.width / 2, Screen.height / 2, 0);
            Cursor.SetCursor(null, initialMousePosition, CursorMode.Auto);
            Cursor.lockState = CursorLockMode.Locked;
        }
        if(Input.GetKey(KeyCode.G)) 
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        // g 키를 뗄 때 마우스 방향 계산 및 애니메이션 재생
        if (Input.GetKeyUp(KeyCode.G) && isGKeyPressed)
        {
            isGKeyPressed = false;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            Vector3 finalMousePosition = Input.mousePosition;
            Vector3 direction = finalMousePosition - initialMousePosition;
            PlayAnimationBasedOnDirection(direction);
        }
    }

    private void PlayAnimationBasedOnDirection(Vector3 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        if (angle >= -45 && angle < 45)
        {
            // 오른쪽 방향 애니메이션
            animator.SetTrigger("Dance");
            Debug.Log("Dance");
        }
        else if (angle >= 45 && angle < 135)
        {
            // 위쪽 방향 애니메이션
            animator.SetTrigger("Enjoy");
            Debug.Log("Enjoy");
        }
        else if (angle >= -135 && angle < -45)
        {
            // 아래쪽 방향 애니메이션
            animator.SetTrigger("Angry");
            Debug.Log("Angry");
        }
        else
        {
            // 왼쪽 방향 애니메이션
            animator.SetTrigger("Sad");
            Debug.Log("Sad");
        }
    }
}
