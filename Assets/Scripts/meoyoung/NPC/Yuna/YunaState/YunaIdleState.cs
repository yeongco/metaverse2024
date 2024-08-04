using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YunaIdleState : MonoBehaviour, IYunaState
{
    private YunaController _yunaController;

    public void OnStateEnter(YunaController yunaController)
    {
        if (!_yunaController)
            _yunaController = yunaController;
    }
    // Idle State가 된지 3초가 지나면 walkState로 전이
    public void OnStateUpdate()
    {
        
    }

    public void OnStateExit()
    {

    }

}
