using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationFuncts : MonoBehaviour
{
    public PlayerMove pm;

    void standup()
    {
        pm.gameObject.GetComponent<PlayerCanSee>().IsChair = false;
        pm.Ischair = false;
    }

    IEnumerator turn()
    {
        pm.gameObject.transform.rotation = Quaternion.Euler(0f, 180.0f, 0f);
        yield return null;
    }

    IEnumerator SitToUPCoroutine()
    {
        Vector3 pos = pm.gameObject.transform.position;
        float CountTime = 0f;
        while (CountTime < 0.7f)
        {
            pm.gameObject.transform.position = Vector3.Lerp(pos, pos + new Vector3(0f, 0.45f, 0.3f), CountTime / 0.7f);
            CountTime += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator StandToDOWNCoroutine()
    {
        Vector3 pos = pm.gameObject.transform.position;
        float CountTime = 0f;
        while (CountTime < 0.5f)
        {
            pm.gameObject.transform.position = Vector3.Lerp(pos, pos + new Vector3(0f, -0.45f, -0.3f), CountTime / 0.5f);
            CountTime += Time.deltaTime;
            yield return null;
        }
    }
}
