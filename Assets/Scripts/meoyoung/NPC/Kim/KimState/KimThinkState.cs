using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KimThinkState : MonoBehaviour, IKimState
{
    private KimController _kimController;

    public void OnStateEnter(KimController kimController)
    {
        if (!_kimController)
            _kimController = kimController;

        //_kimController.anim.SetBool("Think", true);
    }

    // Q키를 누르면 goodState로 전이.
    // E키를 누르면 badState로 전이
    public void OnStateUpdate()
    {
        //Debug.Log("NPC Think Q E");
        /*if (Input.GetKeyDown(KeyCode.Q))
        {
            _kimController.ChangeState(_kimController._goodState);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            _kimController.ChangeState(_kimController._badState);
        }*/
    }

    public void OnStateExit()
    {
       // _kimController.anim.SetBool("Think", false);
    }

}
