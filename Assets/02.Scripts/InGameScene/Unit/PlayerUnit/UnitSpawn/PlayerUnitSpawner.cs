using UnityEngine;

public class PlayerUnitSpawner : UnitSpawner<PlayerUnitData>
{
    // ���� ������ �������� �ƴ��� �Ǻ��� ���� �ʵ�
    public int MaxFarmingUnitCount;         // �ϲ� ����


    Vector3 _spawnPos = new Vector3(-15f, -3.1f, 0f);
    public PlayerUnit PrevUnit;

    protected override void Start()
    {
        base.Start();
        MaxFarmingUnitCount = 10;
        PlayerSpawnManager.Instance.UpdateFarmingUnitResourceUI();
        PlayerSpawnManager.Instance.UpdateUnitResourceUI();
        

        for (int i=0; i<_units.Count; i++)
        {
            PoolManager.Instance.CreatePool(_units[i].UnitType.ToString(),  () => Instantiate(_units[i].UnitPrefab, _spawnPos, Quaternion.identity));
        }



    }

    

    public void Spawn(PlayerUnitType unitType) 
    {
        // ��ΰ� �ƴ� ������ �����Ŀ� �ڽ��� �տ� �ִ� ������ ������ ����
        if (unitType != PlayerUnitType.Farmer)
        {
            PlayerUnit spawnedUnit = PoolManager.Instance.Get(unitType.ToString()).transform.GetChild(0).GetComponent<PlayerUnit>();
            spawnedUnit.transform.parent.transform.position = _spawnPos;


            if (PrevUnit == null)
            {
                spawnedUnit.SetPrevUnit(spawnedUnit);
            }
            else
            {
                spawnedUnit.SetPrevUnit(PrevUnit);
            }


            if (PlayerSpawnManager.Instance.UnitList.SpawnedBattleUnit.Count ==0)
            {
                // ���� ���� ���� ���� ������ UnitAttackManager���� �����ϰ� �ְ� ��
                UnitAttackManager.Instance.SetPlayerFirstUnit(spawnedUnit);
            }

            // ���� ���ָ���Ʈ�� ����
            PlayerSpawnManager.Instance.UnitList.EnqueueUnitList(spawnedUnit);


            PrevUnit = spawnedUnit;
        }
        // ���
        else
        {
            PlayerFarmingUnit spawnedUnit = PoolManager.Instance.Get(unitType.ToString()).transform.GetChild(0).GetComponent<PlayerFarmingUnit>();

        }

        GameManager.Instance.TotalSpawn++;

    }

    



}
