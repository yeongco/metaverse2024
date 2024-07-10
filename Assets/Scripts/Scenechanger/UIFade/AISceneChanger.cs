using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AISceneChanger : MonoBehaviour
{
    private Animator animator;
    public string nextScene; //다음 씬
    private AsyncOperation op; //로딩할 씬이 들어가있을 곳

    [SerializeField]
    private bool IsAIloading = false;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(LoadAI());
    }

    IEnumerator LoadAI()
    {
        yield return null;
        op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;    //로딩이 다 되더라도 일단 0.9에서 멈춤 1이면 시작
        while (!op.isDone)
        {
            yield return null;

            //어느정도 로딩이 됐으며 ai도 준비가 됐다면
            if (op.progress >= 0.9f)
            {
                if (IsAIloading)
                {
                    animator.SetTrigger("LoadingDone"); //로딩 종료시키고 실행준비
                    yield break;
                }
            }
        }
    }

    public void LoadAIScene()
    {
        op.allowSceneActivation = true;
    }

}
