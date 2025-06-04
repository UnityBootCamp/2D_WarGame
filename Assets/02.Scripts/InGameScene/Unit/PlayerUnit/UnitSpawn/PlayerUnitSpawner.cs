using UnityEngine;

public class PlayerUnitSpawner : UnitSpawner<PlayerUnitData>
{
    // Properties
    public float UnitUpgradeCost => _unitUpgradeCost;
    public float MaxFarmingUnitCount => _maxFarmingUnitCount;
    public float HealthUpgradeValue => _healthUpgradeValue;
    public float AttackUpgradeValue => _attackUpgradeValue;

    // Field
    int _maxFarmingUnitCount;         // 일꾼 유닛 최대 인구수
    float _unitUpgradeCost;           // 업그레이드 비용
    float _healthUpgradeValue;        // 체력 업그레이드 수치
    float _attackUpgradeValue;        // 공격 업그레이드 수치

    Vector3 _spawnPos = new Vector3(-15f, -3.1f, 0f);
    public PlayerUnit PrevUnit;


    // UnityLifeCycle
    private void Awake()
    {
        _unitUpgradeCost = 4000f;   
    }

    protected override void Start()
    {
        base.Start();

        _maxFarmingUnitCount = 10;

        PlayerSpawnManager.Instance.UpdateFarmingUnitResourceUI();
        PlayerSpawnManager.Instance.UpdateUnitResourceUI();
        

        for (int i=0; i<_units.Count; i++)
        {
            PoolManager.Instance.CreatePool(_units[i].UnitType.ToString(),  () => Instantiate(_units[i].UnitPrefab, _spawnPos, Quaternion.identity));
        }
    }

    
    // Methods
    public void UpgradeMaxUnit()
    {
        _maxUnitCount += 5;
        _maxFarmingUnitCount += 10;
    }

    public void HealthUpgrade()
    {
        if (_healthUpgradeValue >= 10 || PlayerSpawnManager.Instance.Mineral < UnitUpgradeCost)
            return;

        PlayerSpawnManager.Instance.Mineral -= _unitUpgradeCost;
        _healthUpgradeValue++;
    }

    public void AttackUpgrade()
    {
        if (_attackUpgradeValue >= 10 || PlayerSpawnManager.Instance.Mineral < UnitUpgradeCost)
            return;

        PlayerSpawnManager.Instance.Mineral -= _unitUpgradeCost;
        _attackUpgradeValue++;
    }

    public void Spawn(PlayerUnitType unitType) 
    {
        // 농부가 아닌 유닛은 생성후에 자신의 앞에 있는 유닛의 참조를 저장
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
                // 현재 적의 가장 선봉 유닛을 UnitAttackManager에서 참조하고 있게 함
                UnitAttackManager.Instance.SetPlayerFirstUnit(spawnedUnit);
            }

            // 생성 유닛리스트에 삽입
            PlayerSpawnManager.Instance.UnitList.EnqueueUnitList(spawnedUnit);


            PrevUnit = spawnedUnit;
        }
        // 농부
        else
        {
            PlayerFarmingUnit spawnedUnit = PoolManager.Instance.Get(unitType.ToString()).transform.GetChild(0).GetComponent<PlayerFarmingUnit>();

        }

        GameManager.Instance.TotalSpawn++;
    }
}
