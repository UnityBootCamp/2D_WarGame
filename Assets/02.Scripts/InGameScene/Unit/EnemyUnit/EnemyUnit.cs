using System;
using System.Collections;
using UnityEngine;



public class EnemyUnit : Unit<EnemyUnitData>
{

    // 유닛의 기본데이터
    public EnemyUnitType UnitType;
    public Vector3 ThisUnitSize => _unitSize;



    // 바로 앞 유닛 관련
    public EnemyUnit _prevUnit;

    bool isCanAttack;

    Coroutine _attackCoroutine;
    Coroutine _getDamageCoroutine;   // 피격 애니메이션 코루틴

    

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

     

        // 궁수와 마법사 유닛이 상대보다 1유닛거리 떨어져 있다면
        if (UnitType == EnemyUnitType.Skeleton)
        {
            EnemyUnitAction(IsOneUnitDistance);
        }

        // 궁수 마법사가 아니라면
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

        // 적 선봉 유닛이 이 유닛이라면,
        if (UnitAttackManager.Instance.EnemyFirstUnit == this)
        {
            // 0.1f는 오차범위
            if (playerUnitRightBound - (enemyUnitLeftBound - 0.1f) >= 0)
            {
                return true;
            }
            else
            {

                return false;
            }
        }
        // 선봉 유닛이 따로 있다면,
        else
        {
            return false;
            
        }
    }

    private bool IsOneUnitDistance()
    {
        float enemyUnitLeftBound = (transform.parent.gameObject.transform.position.x - _unitSize.x / 2);
        float playerUnitRightBound;


        // 플레이어 선봉 유닛이 없으면, 플레이어 본진을 최우선 공격대상으로 지정
        if (UnitAttackManager.Instance.PlayerFirstUnit == null)
        {
            playerUnitRightBound = UnitAttackManager.Instance.PlayerHome.transform.position.x + UnitAttackManager.Instance.PlayerHome.PlayerHomeSize.x;
        }
        else
        {
            playerUnitRightBound = (UnitAttackManager.Instance.PlayerFirstUnit.gameObject.transform.parent.gameObject.transform.position.x + UnitAttackManager.Instance.PlayerFirstUnit.ThisUnitSize.x / 2);
        }


        // 적 선봉 유닛이 이 유닛이라면,
        if (UnitAttackManager.Instance.EnemyFirstUnit == this)
        {
            // 기사 유닛의 사이즈만큼의 사거리 안에 적 선봉 유닛이 들어올 경우 true. 0.1f는 오차범위
            if (playerUnitRightBound - (enemyUnitLeftBound - EnemyUnitSize.EnemyUnitSizes[(int)EnemyUnitType.DemonKnight].x + 0.1f) >= 0)
            {
                return true;
            }
            else
            {

                return false;
            }
        }
        // 선봉 유닛이 따로 있다면,
        else
        {
            // 선봉유닛의 사이즈만큼의 사거리 안에 적 선봉 유닛이 들어올 경우 true. 0.1f는 오차범위
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

        // 원래 색으로 복귀
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
        _hp = _unitData.Hp * GameManager.Instance.DifficultyData.DifficultyCoefficient;                     // 체력
        _attackForce = _unitData.AttackForce * GameManager.Instance.DifficultyData.DifficultyCoefficient;   // 공격력
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