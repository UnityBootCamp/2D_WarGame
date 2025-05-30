using UnityEngine;

public class PlayerSpawnManager : MonoBehaviour
{
    // ����
    [HideInInspector] public PlayerSpawnQueue PlayerSpawnQueue;       // ���� ���� ť�� ����
    [HideInInspector] public PlayerUnitSpawner PlayerUnitSpawner;     // ���� ����

    // ������ ������ ���ڸ� �����ִ� UI
    public PlayerResourceUI ResourceUI;
    public PlayerSpawnQueueUI SpawnQueueUI;
    public PlayerUnitControlUI PlayerUnitControlUI;




    public PlayerSpawnedUnitList UnitList = new PlayerSpawnedUnitList();

    // �÷��̾� �����ڿ�
    public int Mineral
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
    int _mineral;

    // ������ ������������ Ȯ���ϴ� bool
    public bool IsCanSpawnUnit => UnitList.TotalUnitCount() < PlayerUnitSpawner.MaxUnitCount;
    public bool IsCanSpawnFarmingUnit => UnitList.TotalFarmingUnitCount() < PlayerUnitSpawner.MaxFarmingUnitCount;

    #region �̱���
    public static PlayerSpawnManager Instance => _instance;

    static PlayerSpawnManager _instance;

    private void Awake()
    {
        _instance = this;

        Mineral = 5000;

        PlayerSpawnQueue = GetComponent<PlayerSpawnQueue>();
        PlayerUnitSpawner = GetComponent<PlayerUnitSpawner>();


        UpdateUnitResourceUI();
        UpdateFarmingUnitResourceUI();
    }

    #endregion

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