using System;
using System.Collections;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;



public class PlayerUnit : Unit<PlayerUnitData>
{

    // ������ �⺻������
    public PlayerUnitType UnitType;
    public Vector3 ThisUnitSize => _unitSize;


    // �ٷ� �� ���� ����
    public PlayerUnit _prevUnit;

    GameObject _oppositeUnit;


    public bool isCanAttack;
    public bool isAttacking;

    public Coroutine _attackCoroutine;


    private void OnEnable()
    {
        IsCanMove = true;
        SetData();
        isCanAttack = true;
    }

    private void Start()
    {
        _unitAnim = GetComponent<Animator>();
        _unitSize = PlayerUnitSize.PlayerUnitSizes[(int)UnitType];
        
    }

    public void PlayerUnitAction(Func<bool> isOppositeUnitInRange)
    {
        
        // ��� ���ְ� ������� �ʴٸ�
        if (isOppositeUnitInRange() == false && _attackCoroutine == null && IsCanMove)
        {
            if (_prevUnit == this ||
                (
                (_prevUnit.transform.position.x - _prevUnitSize.x / 2)
                - (transform.position.x + _unitSize.x / 2) >= 0)
                )
            {
                transform.parent.Translate(Vector3.right * _moveSpeed * Time.deltaTime);
                _unitAnim.SetBool("1_Move", true);
            }

            else
            {
                _unitAnim.SetBool("1_Move", false);
                StartCoroutine(C_MoveCool());
            }
        }
        else if (isOppositeUnitInRange() && _attackCoroutine == null)
        {
            _unitAnim.SetBool("1_Move", false);

            if (isCanAttack && _attackCoroutine == null)
            {
                _attackCoroutine = StartCoroutine(C_AttackRoutine());
            }
            else
            {
                return;
            }
        }
    }

   
    private void Update()
    {
        if (GameManager.Instance.IsGameOver)
            return;

        // �ü��� ������ ������ ��뺸�� 1���ְŸ� ������ �ִٸ�
        if (UnitType == PlayerUnitType.Archer || UnitType == PlayerUnitType.Wizard)
        {
            PlayerUnitAction(IsOneUnitDistance);
        }

        // �ü� �����簡 �ƴ϶��
        else
        {
            PlayerUnitAction(IsFaceOppositeUnit);
        }
           
    }

    // ������ ������ְ� �����ִ°�
    private bool IsFaceOppositeUnit()
    {
        float playerUnitRightBound = (transform.parent.gameObject.transform.position.x + _unitSize.x / 2);
        float enemyUnitLeftBound;

        // ���� ���� ������ ������, ����� ������ �ֿ켱 ���ݴ������ ����
        if (UnitAttackManager.Instance.EnemyFirstUnit == null)
        {
            enemyUnitLeftBound = UnitAttackManager.Instance.EnemyHome.transform.position.x - UnitAttackManager.Instance.EnemyHome.EnemyHomeSize.x;
        }
        else
        {
            enemyUnitLeftBound = (UnitAttackManager.Instance.EnemyFirstUnit.gameObject.transform.parent.gameObject.transform.position.x - UnitAttackManager.Instance.EnemyFirstUnit.ThisUnitSize.x / 2);
        }

        // �÷��̾� ���� ������ �� �����̶��
        if (UnitAttackManager.Instance.PlayerFirstUnit == this)
        {
            if (enemyUnitLeftBound - (playerUnitRightBound + 0.1f) <= 0)
            {
                return true;
            }
            else
            {

                return false;
            }
        }
        else
        {
            return false;
        }
    }

    // ������ ������ְ� 1���ְŸ� �ȿ� �ִ°�
    private bool IsOneUnitDistance()
    {
        float playerUnitRightBound = (transform.parent.gameObject.transform.position.x + _unitSize.x / 2);
        float enemyUnitLeftBound;

        if (UnitAttackManager.Instance.EnemyFirstUnit == null)
        {
            enemyUnitLeftBound = UnitAttackManager.Instance.EnemyHome.transform.position.x - UnitAttackManager.Instance.EnemyHome.EnemyHomeSize.x;
        }
        else
        {
            enemyUnitLeftBound = (UnitAttackManager.Instance.EnemyFirstUnit.gameObject.transform.parent.gameObject.transform.position.x - UnitAttackManager.Instance.EnemyFirstUnit.ThisUnitSize.x / 2);
        }


        // �÷��̾� ���� ������ �� �����̶��,
        if (UnitAttackManager.Instance.PlayerFirstUnit == this)
        {
            // ��� ������ �����ŭ�� ��Ÿ� �ȿ� �� ���� ������ ���� ��� true. 0.1f�� ��������
            if (enemyUnitLeftBound - (playerUnitRightBound + PlayerUnitSize.PlayerUnitSizes[(int)PlayerUnitType.Knight].x + 0.1f) <= 0)
            {
                return true;
            }
            else
            {

                return false;
            }
        }
        // ���� ������ ���� �ִٸ�,
        else
        {
            // ���������� �����ŭ�� ��Ÿ� �ȿ� �� ���� ������ ���� ��� true. 0.1f�� ��������
            if (enemyUnitLeftBound - (playerUnitRightBound + _prevUnitSize.x + 0.1f) <= 0)
            {
                return true;
            }
            else
            {

                return false;
            }
        }

    }


    IEnumerator C_AttackRoutine()
    {
        isCanAttack = false;
        
        _unitAnim.SetTrigger("2_Attack");
        yield return new WaitForSeconds(_attackDelay/2);

        if(UnitAttackManager.Instance.EnemyFirstUnit != null)
        {
            UnitAttackManager.Instance.EnemyFirstUnit.GetDamage(_attackForce);
        }
        else
        {
            UnitAttackManager.Instance.EnemyHome.GetDamage(_attackForce);
        }

        yield return new WaitForSeconds(_attackDelay / 2);
        isCanAttack = true;
        _attackCoroutine = null;
    }

    public void GetDamage(float value)
    {
        _hp -= value;

        if (_hp <= 0)
        {
            OnDeath();
        }
    }



    public void SetPrevUnit(PlayerUnit prevUnit)
    {
        _prevUnit = prevUnit;
        _prevUnitSize = PlayerUnitSize.PlayerUnitSizes[(int)_prevUnit.UnitType];
    }

    void SetData()
    {
        UnitType = _unitData.UnitType;
        _hp = _unitData.Hp;
        _attackForce = _unitData.AttackForce;
        _moveSpeed = _unitData.MoveSpeed;
        _attackDelay = _unitData.AttackDelay;
    }


    public void OnDeath()
    {
        PlayerSpawnManager.Instance.UnitList.UnitsCount[(int)UnitType] = Mathf.Max(PlayerSpawnManager.Instance.UnitList.UnitsCount[(int)UnitType] - 1, 0);
        PlayerSpawnManager.Instance.UpdateUnitResourceUI();
        StartCoroutine(C_DeathCoroutine());
    }

    IEnumerator C_DeathCoroutine()
    {
        _unitAnim.SetBool("3_Death", true);
        yield return new WaitForSeconds(0.5f);
        PoolManager.Instance.Release(UnitType.ToString(), gameObject.transform.parent.gameObject);
        PlayerSpawnManager.Instance.UnitList.DequeueUnitList();
        if (PlayerSpawnManager.Instance.UnitList.SpawnedBattleUnit.Count == 0)
        {

            UnitAttackManager.Instance.PlayerFirstUnit = null;
        }
        else
        {
            UnitAttackManager.Instance.PlayerFirstUnit = PlayerSpawnManager.Instance.UnitList.SpawnedBattleUnit.Peek();
            UnitAttackManager.Instance.PlayerFirstUnit.SetPrevUnit(UnitAttackManager.Instance.PlayerFirstUnit);
        }

    }


}