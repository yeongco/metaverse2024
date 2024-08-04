using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCThinkState : MonoBehaviour, INPCState
{
    private NPCController _npcController;

    public void OnStateEnter(NPCController npcController)
    {
        if (!_npcController)
            _npcController = npcController;

        _npcController.anim.SetBool("Think", true);
        _npcController._navMeshAgent.isStopped = true; // NPC 움직임 멈춤
    }

    // Q키를 누르면 goodState로 전이.
    // E키를 누르면 badState로 전이
    public void OnStateUpdate()
    {
        //Debug.Log("NPC Think Q E");
        if (Input.GetKeyDown(KeyCode.Q))
        {
            _npcController.ChangeState(_npcController._goodState);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            _npcController.ChangeState(_npcController._badState);
        }
    }

    public void OnStateExit()
    {
        _npcController.anim.SetBool("Think", false);
    }

}
