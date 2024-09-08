using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YunaLootAtState : MonoBehaviour, IYunaState
{
    private YunaController _yunaController;
    private float lookTimer;
    public void OnStateEnter(YunaController yunaController)
    {
        if (!_yunaController)
            _yunaController = yunaController;

        _yunaController.anim.SetBool("Lookat", true);

        lookTimer = 0;
    }

    //target에 집중하는 lookat 상태. target에게 3초동안 집중한 후 idleState로 전이
    // space키를 누르면 nod 상태로 전이
    public void OnStateUpdate()
    {
        //Debug.Log("NPC LootAt Q");
        /*if (Input.GetKeyDown(KeyCode.Space))
        {
            _yunaController.ChangeState(_yunaController._nodState);
        }*/

        if (lookTimer >= 3f)
        {
            _yunaController.ChangeState(_yunaController._idleState);
        }
        else
        {
            lookTimer += Time.deltaTime;
        }
    }

    public void OnStateExit()
    {
        _yunaController.anim.SetBool("Lookat", false);
    }
}
