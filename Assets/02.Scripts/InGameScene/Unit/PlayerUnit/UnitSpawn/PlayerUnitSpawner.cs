using UnityEngine;

public class PlayerUnitSpawner : UnitSpawner<PlayerUnitData>
{
    // Properties
    public float UnitUpgradeCost => _unitUpgradeCost;
    public float MaxFarmingUnitCount => _maxFarmingUnitCount;
    public float HealthUpgradeValue => _healthUpgradeValue;
    public float AttackUpgradeValue => _attackUpgradeValue;

    // Field
    int _maxFarmingUnitCount;         // �ϲ� ���� �ִ� �α���
    float _unitUpgradeCost;           // ���׷��̵� ���
    float _healthUpgradeValue;        // ü�� ���׷��̵� ��ġ
    float _attackUpgradeValue;        // ���� ���׷��̵� ��ġ

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
