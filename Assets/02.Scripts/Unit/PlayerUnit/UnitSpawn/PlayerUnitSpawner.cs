using UnityEngine;

public class PlayerUnitSpawner : UnitSpawner<PlayerUnitData>
{
    // 유닛 스폰이 가능한지 아닌지 판별을 위한 필드
    public int MaxFarmingUnitCount;         // 일꾼 유닛


    Vector3 _spawnPos = new Vector3(-15f, -3.1f, 0f);
    PlayerUnit _prevUnit;

    protected override void Start()
    {
        base.Start();

        MaxFarmingUnitCount = 5;

        for (int i=0; i<_units.Count; i++)
        {
            PoolManager.Instance.CreatePool(_units[i].UnitType.ToString(),  () => Instantiate(_units[i].UnitPrefab, _spawnPos, Quaternion.identity));
        }
    }

    public void Spawn(PlayerUnitType unitType) 
    {
        // 농부가 아닌 유닛은 생성후에 자신의 앞에 있는 유닛의 참조를 저장
        if (unitType != PlayerUnitType.Farmer)
        {
            PlayerUnit spawnedUnit = PoolManager.Instance.Get(unitType.ToString()).transform.GetChild(0).GetComponent<PlayerUnit>();

            if (_prevUnit == null)
            {
                spawnedUnit.SetPrevUnit(spawnedUnit);
            }
            else
            {
                spawnedUnit.SetPrevUnit(_prevUnit);
            }


            if (PlayerSpawnManager.Instance.UnitList.SpawnedBattleUnit.Count ==0)
            {
                // 현재 적의 가장 선봉 유닛을 UnitAttackManager에서 참조하고 있게 함
                UnitAttackManager.Instance.SetPlayerFirstUnit(spawnedUnit);
            }

            // 생성 유닛리스트에 삽입
            PlayerSpawnManager.Instance.UnitList.EnqueueUnitList(spawnedUnit);


            _prevUnit = spawnedUnit;
        }
        // 농부
        else
        {
            PlayerFarmingUnit spawnedUnit = PoolManager.Instance.Get(unitType.ToString()).transform.GetChild(0).GetComponent<PlayerFarmingUnit>();

        }

        GameManager.Instance.TotalSpawn++;

    }

    



}
