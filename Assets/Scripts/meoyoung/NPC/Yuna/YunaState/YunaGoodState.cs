using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YunaGoodState : MonoBehaviour, IYunaState
{
    private YunaController _yunaController;

    public void OnStateEnter(YunaController yunaController)
    {
        if (!_yunaController)
            _yunaController = yunaController;

        _yunaController.anim.SetBool("Good", true);
    }

    // Q키를 누르면 lookatState로 전이
    public void OnStateUpdate()
    {
        Debug.Log("NPC Good Q");
        if (Input.GetKeyDown(KeyCode.Q))
        {
            _yunaController.ChangeState(_yunaController._lookatState);
        }
    }

    public void OnStateExit()
    {
        _yunaController.anim.SetBool("Good", false);
    }

}
