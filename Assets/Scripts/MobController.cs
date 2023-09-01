using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class MobController : MonoBehaviour
{
    [SerializeField] private SmoothLookAt _smoothLookAt;
    [SerializeField] private NavMeshAgent _navMeshAgent;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private Animator _anim;
    [SerializeField] private GameObject _playerGameObject;
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

    public event Action<Transform> OnPlayerCaught;

    private void Start()
    {
        _navMeshAgent.speed = _agentBaseSpeed;
        MoveToWaypoint();
    }

    private void Update()
    {
        if(!_isChasingPlayer)
        StartPatrol();
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
        StopPatrol();
        
        _isChasingPlayer = true;
        _navMeshAgent.speed = _agentSprintSpeed;
        _navMeshAgent.SetDestination(_playerGameObject.transform.position);
        _anim.SetTrigger("Run");

        if(!_audioSource.isPlaying)
        {
            _audioSource.volume = 0.5f;
            _audioSource.Play();
        }
    }

    private void StopChase()
    {
        _isChasingPlayer = false;
        _navMeshAgent.speed = _agentBaseSpeed;
        _tempPlayerPos = _playerGameObject.transform;
        _navMeshAgent.SetDestination(_tempPlayerPos.position);

        if(_audioSource.isPlaying)
            _audioSource.DOFade(0f, 0.5f).OnComplete(() => _audioSource.Stop());
    }

    private bool IsPlayerInSight()
    {
        return !Physics.Linecast(transform.position, _playerGameObject.transform.position, _wallLayerMask);
    }
     
    private void OnTriggerStay(Collider other) 
    {
        if(other.gameObject == _playerGameObject.gameObject && IsPlayerInSight())
        StartChase();
        
        if(_isChasingPlayer && !IsPlayerInSight())
        StopChase();
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
        _anim.SetTrigger("Walk");
    }

    public void CatchPlayer()
    {
        _navMeshAgent.velocity = Vector3.zero;
        _navMeshAgent.isStopped = true;
        _smoothLookAt.StartRotating(_playerGameObject.transform);
        OnPlayerCaught?.Invoke(this.transform);
        _anim.SetTrigger("Jumpscare");
    }

    private IEnumerator PatrolCoroutine()
    {
        _isPatrolCoroutineRunning = true;
        _anim.SetTrigger("Idle");
        yield return new WaitForSeconds(_patrolDelay);
        DefineNextWaypoint();
        MoveToWaypoint();
        _isPatrolCoroutineRunning = false;
    }
}
