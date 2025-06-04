using UnityEngine;

// 싱글톤
// 플레이어의 자원과 유닛 생산을 관리한다.
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

    public bool IsCanSpawnUnit => UnitList.TotalUnitCount() < PlayerUnitSpawner.MaxUnitCount;                       // 플레이어 전투 유닛 생성가능한지 확인하는 bool

    public bool IsCanSpawnFarmingUnit => UnitList.TotalFarmingUnitCount() < PlayerUnitSpawner.MaxFarmingUnitCount;  // 플레이어 일꾼 유닛 생성가능한지 확인하는 bool


    // Fields
    // 참조
    public PlayerSpawnedUnitList UnitList = new PlayerSpawnedUnitList();    // 플레이어가 생산한 유닛의 목록을 관리

    [HideInInspector] public PlayerSpawnQueue PlayerSpawnQueue;       // 생산 예약 큐를 관리
    [HideInInspector] public PlayerUnitSpawner PlayerUnitSpawner;     // 유닛 생산

    // 생성된 유닛의 숫자를 보여주는 UI
    public PlayerResourceUI ResourceUI;
    public PlayerSpawnQueueUI SpawnQueueUI;
    public PlayerUnitControlUI PlayerUnitControlUI;
    public HomeUpgradeUI HomeUpgradeUI;

    float _mineral; // 플레이어 보유자원


    #region 싱글톤
    public static PlayerSpawnManager Instance => _instance;

    static PlayerSpawnManager _instance;

    private void Awake()
    {
        _instance = this;

        Mineral = 150;      // 초기 미네랄

        PlayerSpawnQueue = GetComponent<PlayerSpawnQueue>();
        PlayerUnitSpawner = GetComponent<PlayerUnitSpawner>();

    }

    #endregion


    // Methods
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