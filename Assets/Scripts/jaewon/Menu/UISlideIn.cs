using System.Collections;
using UnityEngine;

public class UISlideIn : MonoBehaviour
{
    public RectTransform uiElement; // 이동할 UI 요소
    public Vector3 endPosition = new Vector3(0, 0, 0); // 끝 위치 (화면 안)
    public float duration = 0.7f; // 이동 시간

    private void Start()
    {
        if (uiElement != null)
        {
            // 시작 위치로 설정
            uiElement.anchoredPosition = new Vector3(Screen.width, 0, 0);
        }
    }

    public IEnumerator SlideIn(RectTransform uiElement)
    {
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            // 시간에 따른 보간
            uiElement.anchoredPosition = Vector3.Lerp(new Vector3(Screen.width, 0, 0), endPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }

        // 최종 위치로 설정
        uiElement.anchoredPosition = endPosition;
    }
}
