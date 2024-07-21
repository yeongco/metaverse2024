using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INPCState
{
    void OnStateEnter(NPCController controller);
    void OnStateUpdate();
    void OnStateExit();
}
