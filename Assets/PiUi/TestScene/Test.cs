using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DirectionalMenu : MonoBehaviour
{
    [SerializeField]
    PiUIManager piUi; // PiUI 매니저 참조
    private PiUI directionalMenu; // 방향 메뉴 참조
    private bool isGKeyPressed; // g 키가 눌렸는지 여부를 나타내는 부울 변수
    private Vector3 initialMousePosition; // 초기 마우스 위치

    // 초기화 시 실행
    void Start()
    {
        // 메뉴를 쉽게 가져오기 위해 초기화 시 메뉴 참조 설정
        directionalMenu = piUi.GetPiUIOf("Directional Menu");
    }

    // 매 프레임마다 호출
    void Update()
    {
        // g 키를 누를 때
        if (Input.GetKeyDown(KeyCode.G))
        {
            isGKeyPressed = true;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            initialMousePosition = new Vector3(Screen.width / 2, Screen.height / 2, 0);
            Cursor.SetCursor(null, initialMousePosition, CursorMode.Auto);
            Cursor.lockState = CursorLockMode.Locked;

            // 메뉴 열기
            piUi.ChangeMenuState("Directional Menu", new Vector2(Screen.width / 2f, Screen.height / 2f));
        }

        // g 키를 뗄 때
        if (Input.GetKeyUp(KeyCode.G) && isGKeyPressed)
        {
            isGKeyPressed = false;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            Vector3 finalMousePosition = Input.mousePosition;
            Vector3 direction = finalMousePosition - initialMousePosition;
            PlayAnimationBasedOnDirection(direction);

            // 메뉴 닫기
            piUi.ChangeMenuState("Directional Menu");
        }
    }

    // 방향에 따라 애니메이션 재생
    private void PlayAnimationBasedOnDirection(Vector3 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        if (angle >= -45 && angle < 45)
        {
            // 오른쪽 방향 애니메이션
            Debug.Log("Right Animation");
        }
        else if (angle >= 45 && angle < 135)
        {
            // 위쪽 방향 애니메이션
            Debug.Log("Up Animation");
        }
        else if (angle >= -135 && angle < -45)
        {
            // 아래쪽 방향 애니메이션
            Debug.Log("Down Animation");
        }
        else
        {
            // 왼쪽 방향 애니메이션
            Debug.Log("Left Animation");
        }
    }
}
