using System.Collections;
using Unity.VisualScripting;
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

    private void OnEnable()
    {
        IsCanMove = true;
        SetData();
        isCanAttack = true;
    }

    private void Start()
    {
        _unitAnim = GetComponent<Animator>();
        _unitSize = EnemyUnitSize.EnemyUnitSizes[(int)UnitType];
        

    }

    public bool IsFaceOppositeUnit()
    {
        if (UnitAttackManager.Instance.PlayerFirstUnit == null)
        {
            return false;
        }
        else
        {
            return ((UnitAttackManager.Instance.PlayerFirstUnit.gameObject.transform.parent.gameObject.transform.position.x + UnitAttackManager.Instance.PlayerFirstUnit.ThisUnitSize.x / 2)
                - (transform.parent.gameObject.transform.position.x - _unitSize.x / 2) >= 0);
        }


    }

    private void Update()
    {

        if (IsFaceOppositeUnit() == false && _attackCoroutine == null && IsCanMove)
        {
            if (_prevUnit == this ||
                  (
                  (_prevUnit.transform.position.x + _prevUnitSize.x / 2)
                  - (transform.position.x - _unitSize.x / 2) <= 0)
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
        else if (IsFaceOppositeUnit() && _attackCoroutine == null)
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
    }

    IEnumerator C_AttackRoutine()
    {
        isCanAttack = false;
        _unitAnim.SetTrigger("2_Attack");
        yield return new WaitForSeconds(_attackDelay/2);
        UnitAttackManager.Instance.PlayerFirstUnit.GetDamage(_attackForce);
        yield return new WaitForSeconds(_attackDelay/2);
        isCanAttack = true;
        _attackCoroutine = null;
    }


    public void SetPrevUnit(EnemyUnit prevUnit)
    {
        _prevUnit = prevUnit;
        _prevUnitSize = EnemyUnitSize.EnemyUnitSizes[(int)_prevUnit.UnitType];
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
        PoolManager.Instance.Release(UnitType.ToString(), gameObject.transform.parent.gameObject);
        EnemySpawnManager.Instance.UnitList.DequeueUnitList(); 
        
        if (EnemySpawnManager.Instance.UnitList.SpawnedBattleUnit.Count == 0)
        {

            UnitAttackManager.Instance.EnemyFirstUnit = null;
        }
        else
        {

            UnitAttackManager.Instance.EnemyFirstUnit = EnemySpawnManager.Instance.UnitList.SpawnedBattleUnit.Peek();
            UnitAttackManager.Instance.EnemyFirstUnit.SetPrevUnit(UnitAttackManager.Instance.EnemyFirstUnit);
        }
    }


}