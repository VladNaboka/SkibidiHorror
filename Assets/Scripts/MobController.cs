using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MobController : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _navMeshAgent;
    [SerializeField] private List<Transform> _waypoints = new List<Transform>();
    [SerializeField] private float _agentBaseSpeed;
    [SerializeField] private float _agentSprintSpeed;
    [SerializeField] private float _patrolDelay;
    private Coroutine _patrolCoroutine;
    private int _waypointIndex;
    private bool _isPatrolling = false;
    private bool _isChasingPlayer = false;
    
        
    private void Start()
    {
        _navMeshAgent.speed = _agentBaseSpeed;
        MoveToWaypoint();
    }

    private void Update()
    {
        if(!_isChasingPlayer)
        Patrol();
    }

    private void Patrol()
    {
        _patrolCoroutine = !_isPatrolling && _navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance ? StartCoroutine(PatrolCoroutine()) : null;
    }

    private void DefineNextWaypoint()
    {
        _waypointIndex = _waypointIndex < _waypoints.Count - 1 ? ++_waypointIndex : _waypointIndex = 0;
    }

    private void MoveToWaypoint()
    {
        _navMeshAgent.SetDestination(_waypoints[_waypointIndex].position);
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(other == FindObjectOfType<PlayerController>())
        {
            
        }
    }

    private IEnumerator PatrolCoroutine()
    {
        _isPatrolling = true;
        yield return new WaitForSeconds(_patrolDelay);
        DefineNextWaypoint();
        MoveToWaypoint();
        _isPatrolling = false;
    }
}
