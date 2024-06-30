using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class PlayerState : MonoBehaviour
{
    private PlayerController _playerController;
    void Start()
    {
        _playerController = FindObjectOfType<PlayerController>();
    }

    void Update()
    {
        if (_playerController != null)
        {
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");

            if(moveHorizontal != 0 || moveVertical != 0) {
                _playerController.Walking(moveHorizontal, moveVertical);
            }
            else
            {
                _playerController.Idle();
            }
            _playerController._playerStateContext.UpdateState();
        }
        else
        {
            Debug.Log("PlayerController가 정상적이지 않습니다.");
        }

    }
}

