using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YunaBadState : MonoBehaviour, IYunaState
{
    private YunaController _yunaController;

    public void OnStateEnter(YunaController yunaController)
    {
        if (!_yunaController)
            _yunaController = yunaController;

        _yunaController.anim.SetBool("Bad", true);
    }
    public void OnStateUpdate()
    {
        Debug.Log("NPC Bad Q");
        if (Input.GetKeyDown(KeyCode.Q))
        {
            _yunaController.ChangeState(_yunaController._lookatState);
        }
    }

    public void OnStateExit()
    {
        _yunaController.anim.SetBool("Bad", false);
    }

}

