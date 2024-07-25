using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBadState : MonoBehaviour, INPCState
{
    private NPCController _npcController;

    public void OnStateEnter(NPCController npcController)
    {
        if (!_npcController)
            _npcController = npcController;

        _npcController._navMeshAgent.isStopped = true; // NPC øÚ¡˜¿” ∏ÿ√„
    }
    public void OnStateUpdate()
    {
        Debug.Log("NPC Bad Q");
        if (Input.GetKeyDown(KeyCode.Q))
        {
            _npcController.ChangeState(_npcController._lootatState);
        }
    }

    public void OnStateExit()
    {

    }

}

