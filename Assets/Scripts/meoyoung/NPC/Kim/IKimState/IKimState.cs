using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IKimState
{
    void OnStateEnter(KimController controller);
    void OnStateUpdate();
    void OnStateExit();
}
