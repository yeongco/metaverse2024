using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float Speed = 1.0f;
    public float CurrentSpeed
    {
        get; set;
    }

    public Vector3 CurrentDirection
    {
        get; private set;
    }

    private IPlayerState _idleState, _walkState;

    public PlayerStateContext _playerStateContext;

    private void Start()
    {
        _playerStateContext = new PlayerStateContext(this);

        _idleState = gameObject.AddComponent<PlayerIdleState>();
        _walkState = gameObject.AddComponent<PlayerWalkState>();

        _playerStateContext.ChangeState(_idleState);
    }

    public void Walking(float x, float y)
    {
        CurrentDirection = new Vector3(x, 0.0f, y);
        _playerStateContext.ChangeState(_walkState);
    }
    public void Idle()
    {
        _playerStateContext.ChangeState(_idleState);
    }
}
