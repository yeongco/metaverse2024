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
    }
    public void OnStateUpdate()
    {
        Debug.Log("NPC Idle");
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
