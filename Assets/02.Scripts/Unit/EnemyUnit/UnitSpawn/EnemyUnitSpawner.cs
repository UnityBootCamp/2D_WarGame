using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyUnitSpawner : UnitSpawner<EnemyUnitData>
{


    EnemyUnit _prevEnemyUnit;                           // 선행 유닛 참조


    // 생성관련
    Vector3 _spawnPos = new Vector3(19f, -3.1f, 0f);    // 생성 위치
    int _totalWeight;                                   // 총 가중치
    float _spawnCoolDown;                               // 유닛 생성 쿨
    float _currentCoolDown;                             // 현재 누적시간

    bool _isOnSpawnCool;

    protected override void Start()
    {
        base.Start();

        _isOnSpawnCool = true;
        _spawnCoolDown = 1.2f;

        for (int i = 0; i < _units.Count; i++)
        {
            PoolManager.Instance.CreatePool(_units[i].UnitType.ToString(), () => Instantiate(_units[i].UnitPrefab, _spawnPos, Quaternion.identity));
        }

        for(int i =0; i<_units.Count; i++)
        {
            _totalWeight += _units[i].Weight;
        }

    }

    private void Update()
    {
        if (GameManager.Instance.IsGameOver)
            return;

        // 지정된 스폰쿨이 지났고, 가장 싼 유닛을 생산할 정도의 미네랄을 소유하고 있다면
        if (EnemySpawnManager.Instance.EnemyMineral>= Units[0].Cost&& _isOnSpawnCool && EnemySpawnManager.Instance.IsCanSpawnUnit)
        {
            EnemyUnitData randomEnemyUnitData = null;

            
            randomEnemyUnitData = ChooseRandomUnit(); 

               
            //  적절한 유닛 할당이 안되면
            if(CanSpawn(randomEnemyUnitData) == false || randomEnemyUnitData == null)
            {
                StartCoroutine(C_SpawnCool()); // 스폰 쿨 적용
                return;
            }

            EnemySpawnManager.Instance.UnitList.UnitsCount[(int)randomEnemyUnitData.UnitType]++;


            EnemySpawnManager.Instance.EnemyMineral -= randomEnemyUnitData.Cost;
            EnemySpawnManager.Instance.EnemySpawnQueue.UnitEnqueue(randomEnemyUnitData);
            _currentCoolDown = 0;
        }

    }

    IEnumerator C_SpawnCool()
    {
        _isOnSpawnCool = false;
        yield return new WaitForSeconds(2f);
        _isOnSpawnCool = true;
    }

    public bool CanSpawn(EnemyUnitData enemyUnitData)
    {
        if(enemyUnitData.Cost<= EnemySpawnManager.Instance.EnemyMineral)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public EnemyUnitData ChooseRandomUnit()
    {
        int currentWeight=0;
        int randInt = Random.Range(0, _totalWeight);

        foreach(var unit in Units)
        {
            currentWeight += unit.Weight;
            if(randInt<currentWeight)
            {
                return unit;
            }
        }
        return Units[0];
    }

    public void Spawn(EnemyUnitType unitType)
    {
        EnemyUnit spawnedUnit = PoolManager.Instance.Get(unitType.ToString()).transform.GetChild(0).GetComponent<EnemyUnit>();

        // 생성된 유닛이 앞 유닛을 참조하게 함
        if (_prevEnemyUnit == null)
        {
            spawnedUnit.SetPrevUnit(spawnedUnit);
        }
        else
        {
            spawnedUnit.SetPrevUnit(_prevEnemyUnit);
        }

        // 생성 유닛리스트에 삽입
        if (EnemySpawnManager.Instance.UnitList.SpawnedBattleUnit.Count ==0)
        {
            // 현재 적의 가장 선봉 유닛을 UnitAttackManager에서 참조하고 있게 함
            UnitAttackManager.Instance.SetEnemyFirstUnit(spawnedUnit);
        }

        EnemySpawnManager.Instance.UnitList.EnqueueUnitList(spawnedUnit);


        // 이전 생성 유닛을 갱신
        _prevEnemyUnit = spawnedUnit;


    }



}
