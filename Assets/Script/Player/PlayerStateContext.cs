using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateContext
{
    public IPlayerState CurrentState
    {
        get; set;
    }

    private readonly PlayerController _playerController;

    public PlayerStateContext(PlayerController playerController)
    {
        _playerController = playerController;
    }


    public void ChangeState(IPlayerState playerState)
    {
        if (CurrentState != null)
            CurrentState.OnStateExit();
        CurrentState = playerState;
        CurrentState.OnStateEnter(_playerController);
    }

    public void UpdateState()
    {
        if (CurrentState != null)
        {
            CurrentState.OnStateUpdate();
        }
    }
}