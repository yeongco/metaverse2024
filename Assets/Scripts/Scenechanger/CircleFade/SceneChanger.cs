using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using static System.TimeZoneInfo;

public class SceneChanger : MonoBehaviour
{
    public Animator animator;
    public float fadetime = 1.0f;
    string sceneToLoad;
    public void StartTransition(string scene_name)
    {
        sceneToLoad = scene_name;
        StartCoroutine(LoadLevel());
        
    }

    IEnumerator LoadLevel()
    {
        animator.SetTrigger("close");
        yield return new WaitForSeconds(fadetime);

        SceneManager.LoadScene(sceneToLoad);
    }
    // CloseTransition 애니메이션이 끝날 때 호출됨
    public void OnCloseTransitionComplete()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
