using UnityEngine;

public class PlayerSpawnManager : MonoBehaviour
{
    // 참조
    [HideInInspector] public PlayerSpawnQueue PlayerSpawnQueue;       // 생산 예약 큐를 관리
    [HideInInspector] public PlayerUnitSpawner PlayerUnitSpawner;     // 유닛 생산

    // 생성된 유닛의 숫자를 보여주는 UI
    public PlayerResourceUI ResourceUI;
    public PlayerSpawnQueueUI SpawnQueueUI;
    public PlayerUnitControlUI PlayerUnitControlUI;




    public PlayerSpawnedUnitList UnitList = new PlayerSpawnedUnitList();

    // 플레이어 보유자원
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

    // 유닛이 생성가능한지 확인하는 bool
    public bool IsCanSpawnUnit => UnitList.TotalUnitCount() < PlayerUnitSpawner.MaxUnitCount;
    public bool IsCanSpawnFarmingUnit => UnitList.TotalFarmingUnitCount() < PlayerUnitSpawner.MaxFarmingUnitCount;

    #region 싱글톤
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

    #region 리소스 UI
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

    #region 스폰 큐 UI
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