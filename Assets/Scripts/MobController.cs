using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MobController : MonoBehaviour
{
    [SerializeField] private GameObject _playerGameObject;
    [SerializeField] private NavMeshAgent _navMeshAgent;
    [SerializeField] private List<Transform> _waypoints = new List<Transform>();
    [SerializeField] private LayerMask _wallLayerMask;
    [SerializeField] private float _agentBaseSpeed;
    [SerializeField] private float _agentSprintSpeed;
    [SerializeField] private float _patrolDelay;
    private Coroutine _patrolCoroutine;
    private Transform _tempPlayerPos;
    private int _waypointIndex;
    private bool _isPatrolCoroutineRunning = false;
    private bool _isChasingPlayer = false;
    
        
    private void Start()
    {
        _navMeshAgent.speed = _agentBaseSpeed;
        MoveToWaypoint();
    }

    private void Update()
    {
        if(!_isChasingPlayer)
        StartPatrol();
        else
        StartChase();

        if(_isChasingPlayer && Physics.Linecast(transform.position, _playerGameObject.transform.position, _wallLayerMask))
        StopChase();
    }

    private void StartPatrol()
    {
        if(!_isPatrolCoroutineRunning && _navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
        _patrolCoroutine = StartCoroutine(PatrolCoroutine());
    }

    private void StopPatrol()
    {
        if(_patrolCoroutine != null)
        StopCoroutine(_patrolCoroutine);

        _isPatrolCoroutineRunning = false;
    }

    private void StartChase()
    {
        _isChasingPlayer = true;
        StopPatrol();

        _navMeshAgent.speed = _agentSprintSpeed;
        _navMeshAgent.SetDestination(_playerGameObject.transform.position);
    }

    private void StopChase()
    {
        _isChasingPlayer = false;
        _navMeshAgent.speed = _agentBaseSpeed;
        _tempPlayerPos = _playerGameObject.transform;
        _navMeshAgent.SetDestination(_tempPlayerPos.position);
    }

    private void OnTriggerStay(Collider other) 
    {
        if(other.gameObject == _playerGameObject.gameObject && !Physics.Linecast(transform.position, _playerGameObject.transform.position, _wallLayerMask))
        StartChase();
    }

    private void OnTriggerExit(Collider other) 
    {
        if(other.gameObject == _playerGameObject.gameObject && _isChasingPlayer)
        StopChase();
    }

    private void DefineNextWaypoint()
    {
        _waypointIndex = _waypointIndex < _waypoints.Count - 1 ? ++_waypointIndex : _waypointIndex = 0;
    }

    private void MoveToWaypoint()
    {
        _navMeshAgent.SetDestination(_waypoints[_waypointIndex].position);
    }

    private IEnumerator PatrolCoroutine()
    {
        _isPatrolCoroutineRunning = true;
        yield return new WaitForSeconds(_patrolDelay);
        DefineNextWaypoint();
        MoveToWaypoint();
        _isPatrolCoroutineRunning = false;
    }
}
