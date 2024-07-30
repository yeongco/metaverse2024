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
        _npcController._navMeshAgent.isStopped = true; // NPC ¿òÁ÷ÀÓ ¸ØÃã
    }
    public void OnStateUpdate()
    {
        //Debug.Log("NPC Idle");

        /*// E Å° ÀÔ·Â °¨Áö
        if (Input.GetKeyDown(KeyCode.E))
        {
            _npcController.ChangeState(_npcController._lootatState);
        }*/

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
