using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IYunaState
{
    void OnStateEnter(YunaController controller);
    void OnStateUpdate();
    void OnStateExit();
}
