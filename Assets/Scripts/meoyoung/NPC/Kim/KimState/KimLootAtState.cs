using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KimLootAtState : MonoBehaviour, IKimState
{
    private KimController _kimController;
    private float lookTimer;
    public void OnStateEnter(KimController kimController)
    {
        if (!_kimController)
            _kimController = kimController;

        _kimController.anim.SetBool("LootAt", true);

        lookTimer = 0;
    }

    //target에 집중하는 lookat 상태. target에게 3초동안 집중한 후 idleState로 전이
    // space키를 누르면 nod 상태로 전이
    public void OnStateUpdate()
    {
        //Debug.Log("NPC LootAt Q");
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _kimController.ChangeState(_kimController._nodState);
        }

        if (lookTimer >= 3f)
        {
            _kimController.ChangeState(_kimController._idleState);
        }
        else
        {
            lookTimer += Time.deltaTime;
        }
    }

    public void OnStateExit()
    {
        _kimController.anim.SetBool("LootAt", false);
    }
}
