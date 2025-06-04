using UnityEngine;

// �̱���
// �÷��̾��� �ڿ��� ���� ������ �����Ѵ�.
public class PlayerSpawnManager : MonoBehaviour
{ 
    // Properties
    public float Mineral
    {
        get
        {
            return _mineral;
        }
        set
        {
            _mineral = value;
            UpdateResourceUI();
        }
    }

    public bool IsCanSpawnUnit => UnitList.TotalUnitCount() < PlayerUnitSpawner.MaxUnitCount;                       // �÷��̾� ���� ���� ������������ Ȯ���ϴ� bool

    public bool IsCanSpawnFarmingUnit => UnitList.TotalFarmingUnitCount() < PlayerUnitSpawner.MaxFarmingUnitCount;  // �÷��̾� �ϲ� ���� ������������ Ȯ���ϴ� bool


    // Fields
    // ����
    public PlayerSpawnedUnitList UnitList = new PlayerSpawnedUnitList();    // �÷��̾ ������ ������ ����� ����

    [HideInInspector] public PlayerSpawnQueue PlayerSpawnQueue;       // ���� ���� ť�� ����
    [HideInInspector] public PlayerUnitSpawner PlayerUnitSpawner;     // ���� ����

    // ������ ������ ���ڸ� �����ִ� UI
    public PlayerResourceUI ResourceUI;
    public PlayerSpawnQueueUI SpawnQueueUI;
    public PlayerUnitControlUI PlayerUnitControlUI;
    public HomeUpgradeUI HomeUpgradeUI;

    float _mineral; // �÷��̾� �����ڿ�


    #region �̱���
    public static PlayerSpawnManager Instance => _instance;

    static PlayerSpawnManager _instance;

    private void Awake()
    {
        _instance = this;

        Mineral = 150;      // �ʱ� �̳׶�

        PlayerSpawnQueue = GetComponent<PlayerSpawnQueue>();
        PlayerUnitSpawner = GetComponent<PlayerUnitSpawner>();

    }

    #endregion


    // Methods
    #region ���ҽ� UI
    public void UpdateUnitResourceUI()
    {
        ResourceUI.UpdateUnitResource
            ($"{UnitList.TotalUnitCount()}/{PlayerUnitSpawner.MaxUnitCount}");
    }
    public void UpdateFarmingUnitResourceUI()
    {
        ResourceUI.UpdateFarmingUnitResource
            ($"{UnitList.TotalFarmingUnitCount()}/{PlayerUnitSpawner.MaxFarmingUnitCount}");
    }
    public void UpdateResourceUI()
    {
        ResourceUI.UpdateResource(Mineral);
    }
    #endregion

    #region ���� ť UI
    public void SetSlider(float value)
    {
        SpawnQueueUI.SetSlider(value);
    }

    public void SetAfterSpawn(PlayerUnitType unitType)
    {
        SpawnQueueUI.SetSlider(1);
        SpawnQueueUI.UpdateQueueUI(unitType);
        SpawnQueueUI.WaitingUnits--;
    }
    #endregion
}