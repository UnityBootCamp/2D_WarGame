using System;
using System.Collections;
using UnityEngine;



public class EnemyUnit : Unit<EnemyUnitData>
{

    // ������ �⺻������
    public EnemyUnitType UnitType;
    public Vector3 ThisUnitSize => _unitSize;



    // �ٷ� �� ���� ����
    public EnemyUnit _prevUnit;

    bool isCanAttack;

    Coroutine _attackCoroutine;
    Coroutine _getDamageCoroutine;   // �ǰ� �ִϸ��̼� �ڷ�ƾ

    

    private void OnEnable()
    {
        IsCanMove = true;
        SetData();
        isCanAttack = true;
        _attackCoroutine = null;
    }

    private void Start()
    {
        _unitAnim = GetComponent<Animator>();
        _unitSize = EnemyUnitSize.EnemyUnitSizes[(int)UnitType];
        

    }


    private void Update()
    {
        if (GameManager.Instance.IsGameOver)
            return;

     

        // �ü��� ������ ������ ��뺸�� 1���ְŸ� ������ �ִٸ�
        if (UnitType == EnemyUnitType.Skeleton)
        {
            EnemyUnitAction(IsOneUnitDistance);
        }

        // �ü� �����簡 �ƴ϶��
        else
        {
            EnemyUnitAction(IsFaceOppositeUnit);
        }

       

    }

    public bool IsFaceOppositeUnit()
    {
        float enemyUnitLeftBound = (transform.parent.gameObject.transform.position.x - _unitSize.x / 2);
        float playerUnitRightBound;

        if (UnitAttackManager.Instance.PlayerFirstUnit == null)
        {
            playerUnitRightBound = UnitAttackManager.Instance.PlayerHome.transform.position.x + UnitAttackManager.Instance.PlayerHome.PlayerHomeSize.x;
        }
        else
        {
            playerUnitRightBound = (UnitAttackManager.Instance.PlayerFirstUnit.gameObject.transform.parent.gameObject.transform.position.x + UnitAttackManager.Instance.PlayerFirstUnit.ThisUnitSize.x / 2);
        }

        // �� ���� ������ �� �����̶��,
        if (UnitAttackManager.Instance.EnemyFirstUnit == this)
        {
            // 0.1f�� ��������
            if (playerUnitRightBound - (enemyUnitLeftBound - 0.1f) >= 0)
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
            return false;
            
        }
    }

    private bool IsOneUnitDistance()
    {
        float enemyUnitLeftBound = (transform.parent.gameObject.transform.position.x - _unitSize.x / 2);
        float playerUnitRightBound;


        // �÷��̾� ���� ������ ������, �÷��̾� ������ �ֿ켱 ���ݴ������ ����
        if (UnitAttackManager.Instance.PlayerFirstUnit == null)
        {
            playerUnitRightBound = UnitAttackManager.Instance.PlayerHome.transform.position.x + UnitAttackManager.Instance.PlayerHome.PlayerHomeSize.x;
        }
        else
        {
            playerUnitRightBound = (UnitAttackManager.Instance.PlayerFirstUnit.gameObject.transform.parent.gameObject.transform.position.x + UnitAttackManager.Instance.PlayerFirstUnit.ThisUnitSize.x / 2);
        }


        // �� ���� ������ �� �����̶��,
        if (UnitAttackManager.Instance.EnemyFirstUnit == this)
        {
            // ��� ������ �����ŭ�� ��Ÿ� �ȿ� �� ���� ������ ���� ��� true. 0.1f�� ��������
            if (playerUnitRightBound - (enemyUnitLeftBound - EnemyUnitSize.EnemyUnitSizes[(int)EnemyUnitType.DemonKnight].x + 0.1f) >= 0)
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
            if (playerUnitRightBound - (enemyUnitLeftBound - _prevUnitSize.x - 0.1f) >= 0)
            {
                return true;
            }
            else
            {

                return false;
            }
        }
    }

    private void EnemyUnitAction(Func<bool> isOppositeUnitInRange)
    {
        if (isOppositeUnitInRange() == false && _attackCoroutine == null && IsCanMove)
        {
            if (_prevUnit == this ||
                  (
                  (_prevUnit.transform.parent.transform.position.x + _prevUnitSize.x / 2)
                  - (transform.parent.transform.position.x - _unitSize.x / 2) <= 0)
                  )
            {
                transform.parent.Translate(Vector3.left * _moveSpeed * Time.deltaTime);
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

            if (isCanAttack)
            {
                _attackCoroutine = StartCoroutine(C_AttackRoutine());
            }
            else
            {
                return;
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
            if(_getDamageCoroutine !=null)
            {
                StopCoroutine(_getDamageCoroutine);
                _getDamageCoroutine = null;

            }
            _getDamageCoroutine =  StartCoroutine(C_GetDamageCoroutine());

        }
    }

    IEnumerator C_AttackRoutine()
    {
        isCanAttack = false;
        _unitAnim.SetTrigger("2_Attack");
        yield return new WaitForSeconds(_attackDelay/2);

        if (UnitAttackManager.Instance.PlayerFirstUnit != null)
        {
            UnitAttackManager.Instance.PlayerFirstUnit.GetDamage(_attackForce);
        }
        else
        {
            UnitAttackManager.Instance.PlayerHome.GetDamage(_attackForce);
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


    public void SetPrevUnit(EnemyUnit prevUnit = null)
    {
        _prevUnit = prevUnit;

        if (_prevUnit == null)
        {
            _prevUnitSize = _unitSize; 
        }
        else
        {
            _prevUnitSize = EnemyUnitSize.EnemyUnitSizes[(int)_prevUnit.UnitType];
        }
    }

    void SetData()
    {
        UnitType = _unitData.UnitType;
        _hp = _unitData.Hp * GameManager.Instance.DifficultyData.DifficultyCoefficient;                     // ü��
        _attackForce = _unitData.AttackForce * GameManager.Instance.DifficultyData.DifficultyCoefficient;   // ���ݷ�
        _moveSpeed = _unitData.MoveSpeed;
        _attackDelay = _unitData.AttackDelay;
    }


    public void OnDeath()
    {
        GameManager.Instance.TotalKill++;
        PlayerSpawnManager.Instance.Mineral += EnemySpawnManager.Instance.EnemyUnitSpawner.Units[(int)UnitType].Cost / 4;
        EnemySpawnManager.Instance.UnitList.UnitsCount[(int)UnitType] = Mathf.Max(EnemySpawnManager.Instance.UnitList.UnitsCount[(int)UnitType] - 1, 0);
        StartCoroutine(C_DeathCoroutine());
    }

    IEnumerator C_DeathCoroutine()
    {
        _unitAnim.SetBool("3_Death", true);
        yield return new WaitForSeconds(0.5f);

        PoolManager.Instance.Release(UnitType.ToString(), gameObject.transform.parent.gameObject);
        EnemySpawnManager.Instance.UnitList.DequeueUnitList();

        if (EnemySpawnManager.Instance.UnitList.SpawnedBattleUnit.Count == 0)
        {
            EnemySpawnManager.Instance.EnemyUnitSpawner.PrevEnemyUnit = null;
            UnitAttackManager.Instance.EnemyFirstUnit = null;
        }
        else
        {

            UnitAttackManager.Instance.EnemyFirstUnit = EnemySpawnManager.Instance.UnitList.SpawnedBattleUnit.Peek();
            UnitAttackManager.Instance.EnemyFirstUnit.SetPrevUnit(UnitAttackManager.Instance.EnemyFirstUnit);
        }

    }
}