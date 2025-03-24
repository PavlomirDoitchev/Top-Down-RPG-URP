using Assets.Scripts.Player;
using Assets.Scripts.State_Machine.Player_State_Machine;
using UnityEngine;
using UnityEngine.AI;

public class TestNavMesh : MonoBehaviour
{
    private NavMeshAgent agent;
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }
   
    private void Update()
    {
        //agent.SetDestination(PlayerStats.Instance.gameObject.transform.position);
    }

}
