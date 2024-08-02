using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;

public class NPCController : MonoBehaviour
{
    public Animator anim;
    public GameObject target;

    [HideInInspector]
    public CharacterController _characterController;
    [HideInInspector]
    public NavMeshAgent _navMeshAgent;

    [Header("Movement")]
    public float moveSpeed = 5f;
    public float gravity = 20f;
    public bool availableMov = true;
    public float CurrentSpeed
    {
        get; set;
    }

    public Vector3 CurrentDirection
    {
        get; set;
    }

    public INPCState CurrentState
    {
        get; set;
    }

    public INPCState _idleState, _walkState, _lootatState, _nodState, _goodState, _badState, _thinkState;


    // NPC가 전이될 수 있는 상태에 대한 스크립트를 불러온 후, 초기 상태를 Idle State로 변환
    private void Start()
    {
        _idleState = gameObject.AddComponent<NPCIdleState>();
        _walkState = gameObject.AddComponent<NPCWalkState>();
        _lootatState = gameObject.AddComponent<NPCLootAtState>();
        _nodState = gameObject.AddComponent<NPCNodState>();
        _thinkState = gameObject.AddComponent<NPCThinkState>();
        _goodState = gameObject.AddComponent<NPCGoodState>();
        _badState = gameObject.AddComponent<NPCBadState>();

        _characterController = GetComponent<CharacterController>();
        _navMeshAgent = GetComponent<NavMeshAgent>();

        CurrentState = _idleState;
        ChangeState(CurrentState);
    }

    private void Update()
    {
            UpdateState();
    }

    public void ChangeState(INPCState playerState)
    {
        if (CurrentState != null)
            CurrentState.OnStateExit();
        CurrentState = playerState;
        CurrentState.OnStateEnter(this);
    }

    public void UpdateState()
    {
        if (CurrentState != null)
        {
            CurrentState.OnStateUpdate();
        }
    }

    public CharacterController GetCharacterController()
    {
        return _characterController;
    }

}
