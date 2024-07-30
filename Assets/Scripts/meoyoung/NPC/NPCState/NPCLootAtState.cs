using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCLootAtState : MonoBehaviour, INPCState
{
    private NPCController _npcController;
    private float lookTimer;
    public void OnStateEnter(NPCController npcController)
    {
        if (!_npcController)
            _npcController = npcController;

        _npcController._navMeshAgent.isStopped = true; // NPC 움직임 멈춤
        _npcController.anim.SetBool("LootAt", true);

        lookTimer = 0;
    }

    //target을 바라보는 lookat 상태. target을 3초동안 쳐다본 후 walkState로 전이
    // q키를 누르면 nod 상태로 전이
    public void OnStateUpdate()
    {
        //Debug.Log("NPC LootAt Q");
        if (Input.GetKeyDown(KeyCode.Q))
        {
            _npcController.ChangeState(_npcController._nodState);
        }


        if (lookTimer >= 3f)
        {
            _npcController.ChangeState(_npcController._walkState);
        }
        else
        {
            Vector3 direction = (_npcController.target.transform.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
            lookTimer += Time.deltaTime;
        }
    }

    public void OnStateExit()
    {
        _npcController._navMeshAgent.isStopped = false; // NPC 움직임 재개
        _npcController.anim.SetBool("LootAt", false);
    }
}
