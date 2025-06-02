using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyUnitSpawner : UnitSpawner<EnemyUnitData>
{


    public EnemyUnit PrevEnemyUnit;                           // 선행 유닛 참조


    // 생성관련
    Vector3 _spawnPos = new Vector3(19f, -3.1f, 0f);    // 생성 위치
    int _totalWeight;                                   // 총 가중치

    bool _isOnSpawnCool;

    int _howManyUnitOpen;                         // 현재 해금된 적 유닛 수

    protected override void Start()
    {
        base.Start();

        _howManyUnitOpen = 3;
        _isOnSpawnCool = true;

        for (int i = 0; i < _units.Count; i++)
        {
            PoolManager.Instance.CreatePool(_units[i].UnitType.ToString(), () => Instantiate(_units[i].UnitPrefab, _spawnPos, Quaternion.identity));
        }

        for(int i =0; i<_howManyUnitOpen; i++)
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
            
        }

    }

    IEnumerator C_SpawnCool()
    {
        _isOnSpawnCool = false;
        yield return new WaitForSeconds(2f);
        _isOnSpawnCool = true;
    }

    // 적 본진 레벨업하면 유닛 추가
    public void AddUnit()
    {
        _howManyUnitOpen++;
        _totalWeight += _units[_howManyUnitOpen - 1].Weight;
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

        for(int i =0; i<_howManyUnitOpen; i++)
        {
            currentWeight += Units[i].Weight;
            if (randInt < currentWeight)
            {
                return Units[i];
            }
        }
        return null;

    }

    public void Spawn(EnemyUnitType unitType)
    {
        EnemyUnit spawnedUnit = PoolManager.Instance.Get(unitType.ToString()).transform.GetChild(0).GetComponent<EnemyUnit>();
        spawnedUnit.transform.parent.transform.position = _spawnPos;


        // 생성된 유닛이 앞 유닛을 참조하게 함
        if (PrevEnemyUnit == null)
        {
            spawnedUnit.SetPrevUnit(spawnedUnit);
        }
        else
        {
            spawnedUnit.SetPrevUnit(PrevEnemyUnit);
        }

        // 생성 유닛리스트에 삽입
        if (EnemySpawnManager.Instance.UnitList.SpawnedBattleUnit.Count ==0)
        {
            // 현재 적의 가장 선봉 유닛을 UnitAttackManager에서 참조하고 있게 함
            UnitAttackManager.Instance.SetEnemyFirstUnit(spawnedUnit);
        }

        EnemySpawnManager.Instance.UnitList.EnqueueUnitList(spawnedUnit);


        // 이전 생성 유닛을 갱신
        PrevEnemyUnit = spawnedUnit;


    }



}
