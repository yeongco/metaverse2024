using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class ending : MonoBehaviour
{
    public Text endingText;
    private void OnEnable()
    {
        FadeController.Instance.JustFade();
        FadeController.Instance.StartFadeOut();
        endingText.text = GameManager.Instance.result;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("EÅ¬¸¯");
            SceneManager.LoadScene("Main");
        }
    }


}
