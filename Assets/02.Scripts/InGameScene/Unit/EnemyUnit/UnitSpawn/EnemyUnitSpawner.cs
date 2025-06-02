using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyUnitSpawner : UnitSpawner<EnemyUnitData>
{


    public EnemyUnit PrevEnemyUnit;                           // ���� ���� ����


    // ��������
    Vector3 _spawnPos = new Vector3(19f, -3.1f, 0f);    // ���� ��ġ
    int _totalWeight;                                   // �� ����ġ

    bool _isOnSpawnCool;

    int _howManyUnitOpen;                         // ���� �رݵ� �� ���� ��

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

        // ������ �������� ������, ���� �� ������ ������ ������ �̳׶��� �����ϰ� �ִٸ�
        if (EnemySpawnManager.Instance.EnemyMineral>= Units[0].Cost&& _isOnSpawnCool && EnemySpawnManager.Instance.IsCanSpawnUnit)
        {
            EnemyUnitData randomEnemyUnitData = null;

            
            randomEnemyUnitData = ChooseRandomUnit(); 

               
            //  ������ ���� �Ҵ��� �ȵǸ�
            if(CanSpawn(randomEnemyUnitData) == false || randomEnemyUnitData == null)
            {
                StartCoroutine(C_SpawnCool()); // ���� �� ����
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

    // �� ���� �������ϸ� ���� �߰�
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


        // ������ ������ �� ������ �����ϰ� ��
        if (PrevEnemyUnit == null)
        {
            spawnedUnit.SetPrevUnit(spawnedUnit);
        }
        else
        {
            spawnedUnit.SetPrevUnit(PrevEnemyUnit);
        }

        // ���� ���ָ���Ʈ�� ����
        if (EnemySpawnManager.Instance.UnitList.SpawnedBattleUnit.Count ==0)
        {
            // ���� ���� ���� ���� ������ UnitAttackManager���� �����ϰ� �ְ� ��
            UnitAttackManager.Instance.SetEnemyFirstUnit(spawnedUnit);
        }

        EnemySpawnManager.Instance.UnitList.EnqueueUnitList(spawnedUnit);


        // ���� ���� ������ ����
        PrevEnemyUnit = spawnedUnit;


    }



}
