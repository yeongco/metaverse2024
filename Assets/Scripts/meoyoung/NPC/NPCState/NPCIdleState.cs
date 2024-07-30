using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCIdleState : MonoBehaviour, INPCState
{
    private NPCController _npcController;
    private float idleDuration = 3f;
    private float idleTimer;
    public void OnStateEnter(NPCController npcController)
    {
        if (!_npcController)
            _npcController = npcController;
        idleTimer = 0f;
        _npcController._navMeshAgent.isStopped = true; // NPC 움직임 멈춤
    }
    // Idle State가 된지 3초가 지나면 walkState로 전이
    public void OnStateUpdate()
    {
        //Debug.Log("NPC Idle");

        idleTimer += Time.deltaTime;
        if (idleTimer >= idleDuration)
        {
            _npcController.ChangeState(_npcController._walkState);
        }
    }

    public void OnStateExit()
    {

    }

}
