using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KimGoodState : MonoBehaviour, IKimState
{
    private KimController _kimController;

    public void OnStateEnter(KimController kimController)
    {
        if (!_kimController)
            _kimController = kimController;

       // _kimController.anim.SetBool("Good", true);
    }

    // Q키를 누르면 lookatState로 전이
    public void OnStateUpdate()
    {
        /*Debug.Log("NPC Good Q");
        if (Input.GetKeyDown(KeyCode.Q))
        {
            _kimController.ChangeState(_kimController._lookatState);
        }*/
    }

    public void OnStateExit()
    {
        //_kimController.anim.SetBool("Good", false);
    }

}
