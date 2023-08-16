using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DiglettTower : Tower
{
    [Header("Diglett")]
    [SerializeField] private DecalProjector _decalPrjector;
    [SerializeField] private DiglettStates _currentState = DiglettStates.Idle;

    private float attackTimer;
    private Vector3 _startPos;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        _decalPrjector.size = new Vector3(_towerData.towerRange * 2, _towerData.towerRange * 2, _decalPrjector.size.z);
        _startPos = transform.position;
    }

    void FixedUpdate()
    {
        FindTarget();
    }

    // Update is called once per frame
    void Update()
    {
        switch (_currentState)
        {
            case DiglettStates.Idle:
                PerformIdle();
                break;
            case DiglettStates.GoToGround:
                GoToGround();
                break;
            case DiglettStates.PopUpAndAttack:
                PopUpAndAttack();
                break;
            case DiglettStates.GetBackToPlace:
                GoBackToPlace();
                break;
            default:
                break;
        }
    }

    private void PerformIdle()
    {
        attackTimer += Time.deltaTime;
        if (attackTimer >= _towerData.towerAttackInterveal && GameManager.Instance.inWave)
        {
            attackTimer = 0;

            if(_target != null)
            {
                Vector3 dir = _target.transform.position - transform.position;
                if (Physics.Raycast(transform.position, dir, _layerMask))
                {
                    _currentState = DiglettStates.GoToGround;
                }
            }
        }
    }

    private void GoToGround()
    {
        if (_target == null)
        {
            _currentState = DiglettStates.GetBackToPlace;
            return;
        }

        if(transform.position.y >= -1f)
            transform.position -= Vector3.up * Time.deltaTime;
        else
        {
            transform.position = new Vector3(_target.transform.position.x,transform.position.y,_target.transform.position.z) + _target.transform.forward;
            _currentState = DiglettStates.PopUpAndAttack;
        }
    }

    private void PopUpAndAttack()
    {
        if (_target == null)
        {
            _currentState = DiglettStates.GetBackToPlace;
            return;
        }

        if (transform.position.y <= 0.5f)
            transform.position += Vector3.up * Time.deltaTime * 2f;
        else
            _currentState = DiglettStates.GetBackToPlace;

    }

    private void GoBackToPlace()
    {
        if (Vector3.Distance(transform.position, _startPos) > 0.1f)
        {
            Vector3 dir = (_startPos - transform.position).normalized;
            transform.position += dir * Time.deltaTime * 10;
            transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
        }
        else
        {
            GetComponent<TowerRotator>().LookAtNearestPath();
            _currentState = DiglettStates.Idle;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (_currentState != DiglettStates.PopUpAndAttack) return;

        var enemy = other.GetComponent<Enemy>();
        if (enemy)
            enemy.TakeDmg(_towerData.towerDmg);

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_currentState != DiglettStates.PopUpAndAttack) return;

        var enemy = collision.gameObject.GetComponent<Enemy>();
        if (enemy)
            enemy.TakeDmg(_towerData.towerDmg);
    }

    public enum DiglettStates
    {
        Idle,
        GoToGround,
        PopUpAndAttack,
        GetBackToPlace
    }

}
