using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KimIdleState : MonoBehaviour, IKimState
{
    private KimController _kimController;

    public void OnStateEnter(KimController kimController)
    {
        if (!_kimController)
            _kimController = kimController;
    }
    // Idle State가 된지 3초가 지나면 walkState로 전이
    public void OnStateUpdate()
    {
        
    }

    public void OnStateExit()
    {

    }

}
