using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalkState : MonoBehaviour, IPlayerState
{
    private PlayerController _playerController;

    public void OnStateEnter(PlayerController playerController)
    {
        if (!_playerController)
            _playerController = playerController;
        _playerController.CurrentSpeed = _playerController.Speed;
    }


    public void OnStateUpdate()
    {
        if (_playerController)
        {
            Vector3 movement = _playerController.CurrentDirection;
            _playerController.transform.Translate(movement * _playerController.CurrentSpeed * Time.deltaTime);
        }
    }
    public void OnStateExit()
    {

    }

 
}

