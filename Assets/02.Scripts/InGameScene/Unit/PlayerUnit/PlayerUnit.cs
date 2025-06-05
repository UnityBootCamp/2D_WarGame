using System;
using System.Collections;
using UnityEngine;


public class PlayerUnit : Unit<PlayerUnitData>
{
    // Properties
    public Vector3 ThisUnitSize => _unitSize;

    // Fields
    // 유닛의 기본데이터
    public PlayerUnitType UnitType;

    public PlayerUnit _prevUnit;     // 현재유닛의 선행 유닛 참조

    public bool isCanAttack;
    public bool isAttacking;

    Coroutine _attackCoroutine;      // 공격 애니메이션 코루틴
    Coroutine _getDamageCoroutine;   // 피격 애니메이션 코루틴

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

        // 궁수와 마법사 유닛이 상대보다 1유닛거리 떨어져 있다면
        if (UnitType == PlayerUnitType.Archer || UnitType == PlayerUnitType.Wizard)
        {
            PlayerUnitAction(IsOneUnitDistance);
        }

        // 궁수 마법사가 아니라면
        else
        {
            PlayerUnitAction(IsFaceOppositeUnit);
        }
           
    }


    // Methods
    public void PlayerUnitAction(Func<bool> isOppositeUnitInRange)
    {

        // 상대 유닛과 닿아있지 않다면
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

    // 유닛이 상대유닛과 접해있는가
    private bool IsFaceOppositeUnit()
    {
        float playerUnitRightBound = (transform.parent.gameObject.transform.position.x + _unitSize.x / 2);
        float enemyUnitLeftBound;

        // 적의 선봉 유닛이 없으면, 상대의 본진을 최우선 공격대상으로 설정
        if (UnitAttackManager.Instance.EnemyFirstUnit == null)
        {
            enemyUnitLeftBound = UnitAttackManager.Instance.EnemyHome.transform.position.x - UnitAttackManager.Instance.EnemyHome.EnemyHomeSize.x;
        }
        else
        {
            enemyUnitLeftBound = (UnitAttackManager.Instance.EnemyFirstUnit.gameObject.transform.parent.gameObject.transform.position.x - UnitAttackManager.Instance.EnemyFirstUnit.ThisUnitSize.x / 2);
        }

        // 플레이어 선봉 유닛이 이 유닛이라면
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

    // 유닛이 상대유닛과 1유닛거리 안에 있는가
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


        // 플레이어 선봉 유닛이 이 유닛이라면,
        if (UnitAttackManager.Instance.PlayerFirstUnit == this)
        {
            // 기사 유닛의 사이즈만큼의 사거리 안에 적 선봉 유닛이 들어올 경우 true. 0.1f는 오차범위
            if (enemyUnitLeftBound - (playerUnitRightBound + PlayerUnitSize.PlayerUnitSizes[(int)PlayerUnitType.Knight].x + 0.1f) <= 0)
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

        // 궁수 유닛일 경우
        if (UnitType == PlayerUnitType.Archer)
        {
            // 적의 선봉 유닛이 있으면 선봉유닛에게 화살 발사
            if (UnitAttackManager.Instance.EnemyFirstUnit != null)
            {
                UnitAttackManager.Instance.LongRangeAttack.ShootArrow
                    (_attackDelay, transform.parent.transform.position, UnitAttackManager.Instance.EnemyFirstUnit.gameObject.transform.parent.gameObject.transform.position);
            }
            // 적의 선봉 유닛이 없으면 적의 본진에 화살 발사
            else if (UnitAttackManager.Instance.EnemyFirstUnit == null)
            {
                UnitAttackManager.Instance.LongRangeAttack.ShootArrow
                    (_attackDelay, transform.parent.transform.position, UnitAttackManager.Instance.EnemyHome.transform.position);
            }
        }
        // 마법사 유닛일 경우
        else if (UnitType == PlayerUnitType.Wizard)
        {
            // 적의 선봉 유닛이 있으면 선봉유닛 머리 위에 메테오 소환
            if (UnitAttackManager.Instance.EnemyFirstUnit != null)
            {
                UnitAttackManager.Instance.LongRangeAttack.SummonMeteor
                    (_attackDelay, transform.parent.transform.position, UnitAttackManager.Instance.EnemyFirstUnit.gameObject.transform.parent.gameObject.transform.position);
            }
            // 적의 선봉 유닛이 없으면 적의 본진 위에 메테오 소환
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

        // 원래 색으로 복귀
        for (int i = 0; i < _spriteRenderers.Length; i++)
        {
            _spriteRenderers[i].color = _originalColors[i];
        }

        _getDamageCoroutine = null;
    }




}