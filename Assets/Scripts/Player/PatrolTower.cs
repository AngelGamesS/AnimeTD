using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolTower : Tower
{
    [Header("Patrol")]
    public float RotationSpeed = 1;
    public float CircleRadius = 1;

    public float ElevationOffset = 0;

    [SerializeField] private PatrolStates _currentState = PatrolStates.Idle;
    private Vector3 _startPos;
    private Vector3 _positionOffset;
    private float _angle;
    private float _idleTimer = 0;

    protected override void Start()
    {
        base.Start();
        _startPos = transform.position;
    }

    private void Update()
    {
        switch (_currentState)
        {
            case PatrolStates.Idle:
                PerformIdle();
                break;
            case PatrolStates.GoToPatrol:
                GoToPatrol();
                break;
            case PatrolStates.Patrol:
                break;
            case PatrolStates.Attack:
                Attack();
                break;
            case PatrolStates.GetBackToPlace:
                MoveBackToStartPos();
                break;
            default:
                break;
        }
    }

    private void MoveBackToStartPos()
    {
        _animator.CrossFade("Pidgey flying",0.1f);
        if (Vector3.Distance(transform.position, _startPos) > 0.1f)
        {
            Vector3 dir = (_startPos - transform.position).normalized;
            transform.position += dir * Time.deltaTime * RotationSpeed * 6;
            transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
        }
        else
        {
            GetComponent<TowerRotator>().LookAtNearestPath();
            _currentState = PatrolStates.Idle;
        }

    }

    private void Attack()
    {
        if (_target != null)
            MoveToTargetAndAttack();
        else
            _currentState = PatrolStates.GetBackToPlace;
    }

    private void MoveToTargetAndAttack()
    {
        _animator.CrossFade("Pidgey dive", 0.1f);
        Vector3 dir = (_target.transform.position - transform.position).normalized;
        transform.position += dir * Time.deltaTime * RotationSpeed * 10;
        transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
        if(Vector3.Distance(transform.position, _target.transform.position) < 0.2f)
        {
            PerformAttack();
        }
    }

    private void PerformAttack()
    {
        _animator.Play("Pidgey attack");

        Invoke("InvokedGetBackToPlace", 0.2f);
    }

    private void InvokedGetBackToPlace()
    {
        _currentState = PatrolStates.GetBackToPlace;
    }

    private void GoToPatrol()
    {
        _animator.Play("Pidgey flying");
        _positionOffset.Set(
            Mathf.Cos(_angle) * CircleRadius,
            ElevationOffset,
            Mathf.Sin(_angle) * CircleRadius
        );
        Vector3 dir = ((_startPos + _positionOffset) - transform.position).normalized;

        transform.position += dir * Time.deltaTime * RotationSpeed * 3;
        transform.rotation = Quaternion.LookRotation(dir, Vector3.up);

        if (Vector3.Distance(transform.position, _startPos + _positionOffset) < 0.2f)
            _currentState = PatrolStates.Patrol;
    }

    private void PerformIdle()
    {
        _animator.CrossFade("Pidgey idle", 0.1f);
        _idleTimer += Time.deltaTime;
        if(_idleTimer >= _towerData.towerAttackInterveal && GameManager.Instance.inWave)
        {
            _idleTimer = 0;
            _currentState = PatrolStates.GoToPatrol;
        }
    }

    private void LateUpdate()
    {
        if(_currentState == PatrolStates.Patrol)
        {
            _positionOffset.Set(
                Mathf.Cos(_angle) * CircleRadius,
                ElevationOffset,
                Mathf.Sin(_angle) * CircleRadius
            );
            Vector3 dir = ((_startPos + _positionOffset) - transform.position).normalized;
            transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
            transform.position = _startPos + _positionOffset;
            _angle += Time.deltaTime * RotationSpeed;

            if (_target != null)
                _currentState = PatrolStates.Attack;
            else
                FindTarget();
        }
    }

    public enum PatrolStates
    {
        Idle,
        GoToPatrol,
        Patrol,
        Attack,
        GetBackToPlace
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_currentState != PatrolStates.Attack) return;

        var enemy = other.GetComponent<Enemy>();
        if (enemy)
            enemy.TakeDmg(_towerData.towerDmg);

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_currentState != PatrolStates.Attack) return;

        var enemy = collision.gameObject.GetComponent<Enemy>();
        if (enemy)
            enemy.TakeDmg(_towerData.towerDmg);
    }
}
