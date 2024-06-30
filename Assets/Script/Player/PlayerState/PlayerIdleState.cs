using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : MonoBehaviour, IPlayerState
{
    private PlayerController _playerController;
    public void OnStateEnter(PlayerController playerController)
    {
        if (!_playerController)
            _playerController = playerController;
        _playerController.CurrentSpeed = 0;


    }
    public void OnStateUpdate()
    {

    }

    public void OnStateExit()
    {

    }
}
