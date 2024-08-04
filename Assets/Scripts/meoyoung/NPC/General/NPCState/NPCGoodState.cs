using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCGoodState : MonoBehaviour, INPCState
{
    private NPCController _npcController;

    public void OnStateEnter(NPCController npcController)
    {
        if (!_npcController)
            _npcController = npcController;

        _npcController.anim.SetBool("Good", true);
        _npcController._navMeshAgent.isStopped = true; // NPC 움직임 멈춤
    }

    // Q키를 누르면 lookatState로 전이
    public void OnStateUpdate()
    {
        Debug.Log("NPC Good Q");
        if (Input.GetKeyDown(KeyCode.Q))
        {
            _npcController.ChangeState(_npcController._lootatState);
        }
    }

    public void OnStateExit()
    {
        _npcController.anim.SetBool("Good", false);
    }

}
