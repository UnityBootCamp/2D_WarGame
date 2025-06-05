using System;
using System.Collections;
using UnityEngine;


public class PlayerUnit : Unit<PlayerUnitData>
{
    // Properties
    public Vector3 ThisUnitSize => _unitSize;

    // Fields
    // ������ �⺻������
    public PlayerUnitType UnitType;

    public PlayerUnit _prevUnit;     // ���������� ���� ���� ����

    public bool isCanAttack;
    public bool isAttacking;

    Coroutine _attackCoroutine;      // ���� �ִϸ��̼� �ڷ�ƾ
    Coroutine _getDamageCoroutine;   // �ǰ� �ִϸ��̼� �ڷ�ƾ

    const float UPGRADE_COEFFICIENT = 0.1f;


    // UnityLifeCycle
    private void OnEnable()
    {
        SetData();
        IsCanMove = true;
        isCanAttack = true;
        _attackCoroutine = null;
    }

    private void Start()
    {
        _unitAnim = GetComponent<Animator>();
        _unitSize = PlayerUnitSize.PlayerUnitSizes[(int)UnitType];
        
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


    // Methods
    public void PlayerUnitAction(Func<bool> isOppositeUnitInRange)
    {

        // ��� ���ְ� ������� �ʴٸ�
        if (isOppositeUnitInRange() == false && _attackCoroutine == null && IsCanMove)
        {
            if (_prevUnit == this ||
                (
                (_prevUnit.transform.parent.transform.position.x - _prevUnitSize.x / 2)
                - (transform.parent.transform.position.x + _unitSize.x / 2) >= 0)
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

    public void GetDamage(float value)
    {
        _hp -= value;
        if (_hp <= 0)
        {
            OnDeath();
        }
        else
        {
            if(_getDamageCoroutine != null)
            {
                StopCoroutine(_getDamageCoroutine);
                _getDamageCoroutine = null;
            }
            _getDamageCoroutine = StartCoroutine(C_GetDamageCoroutine());
        }
    }

    public void SetPrevUnit(PlayerUnit prevUnit = null)
    {
        _prevUnit = prevUnit;

        if (_prevUnit == null)
        {
            _prevUnitSize = _unitSize; 
        }
        else
        {
            _prevUnitSize = PlayerUnitSize.PlayerUnitSizes[(int)_prevUnit.UnitType];
        }
    }


    void SetData()
    {
        UnitType = _unitData.UnitType;
        _hp = _unitData.Hp *(1+UPGRADE_COEFFICIENT*PlayerSpawnManager.Instance.PlayerUnitSpawner.HealthUpgradeValue);
        _attackForce = _unitData.AttackForce * (1 + UPGRADE_COEFFICIENT * PlayerSpawnManager.Instance.PlayerUnitSpawner.AttackUpgradeValue);
        _moveSpeed = _unitData.MoveSpeed;
        _attackDelay = _unitData.AttackDelay;
    }


    public void OnDeath()
    {
        EnemySpawnManager.Instance.EnemyMineral += PlayerSpawnManager.Instance.PlayerUnitSpawner.Units[(int)UnitType].Cost / 4;
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
            PlayerSpawnManager.Instance.PlayerUnitSpawner.PrevUnit = null;
            UnitAttackManager.Instance.PlayerFirstUnit = null;
        }
        else
        {
            UnitAttackManager.Instance.PlayerFirstUnit = PlayerSpawnManager.Instance.UnitList.SpawnedBattleUnit.Peek();
            UnitAttackManager.Instance.PlayerFirstUnit.SetPrevUnit(UnitAttackManager.Instance.PlayerFirstUnit);
        }

    }
    IEnumerator C_AttackRoutine()
    {
        isCanAttack = false;

        // �ü� ������ ���
        if (UnitType == PlayerUnitType.Archer)
        {
            // ���� ���� ������ ������ �������ֿ��� ȭ�� �߻�
            if (UnitAttackManager.Instance.EnemyFirstUnit != null)
            {
                UnitAttackManager.Instance.LongRangeAttack.ShootArrow
                    (_attackDelay, transform.parent.transform.position, UnitAttackManager.Instance.EnemyFirstUnit.gameObject.transform.parent.gameObject.transform.position);
            }
            // ���� ���� ������ ������ ���� ������ ȭ�� �߻�
            else if (UnitAttackManager.Instance.EnemyFirstUnit == null)
            {
                UnitAttackManager.Instance.LongRangeAttack.ShootArrow
                    (_attackDelay, transform.parent.transform.position, UnitAttackManager.Instance.EnemyHome.transform.position);
            }
        }
        // ������ ������ ���
        else if (UnitType == PlayerUnitType.Wizard)
        {
            // ���� ���� ������ ������ �������� �Ӹ� ���� ���׿� ��ȯ
            if (UnitAttackManager.Instance.EnemyFirstUnit != null)
            {
                UnitAttackManager.Instance.LongRangeAttack.SummonMeteor
                    (_attackDelay, transform.parent.transform.position, UnitAttackManager.Instance.EnemyFirstUnit.gameObject.transform.parent.gameObject.transform.position);
            }
            // ���� ���� ������ ������ ���� ���� ���� ���׿� ��ȯ
            else if (UnitAttackManager.Instance.EnemyFirstUnit == null)
            {
                UnitAttackManager.Instance.LongRangeAttack.SummonMeteor(_attackDelay, transform.parent.transform.position, UnitAttackManager.Instance.EnemyHome.transform.position);
            }
        }


        _unitAnim.SetTrigger("2_Attack");
        SoundManager.Instance.PlayerUnitAttack();

        yield return new WaitForSeconds(_attackDelay / 2);

        if (UnitAttackManager.Instance.EnemyFirstUnit != null)
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

    IEnumerator C_GetDamageCoroutine()
    {
        foreach (var spriteRenderer in _spriteRenderers)
        {
            spriteRenderer.color = Color.red;
        }

        yield return new WaitForSeconds(0.1f);

        // ���� ������ ����
        for (int i = 0; i < _spriteRenderers.Length; i++)
        {
            _spriteRenderers[i].color = _originalColors[i];
        }

        _getDamageCoroutine = null;
    }




}