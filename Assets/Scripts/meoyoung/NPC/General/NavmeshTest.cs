using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavmeshTest : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] NavMeshAgent _navMeshAgent;

    // Update is called once per frame
    void Update()
    {
        _navMeshAgent.SetDestination(target.position); 
    }
}
