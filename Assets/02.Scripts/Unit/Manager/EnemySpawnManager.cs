using TMPro;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    // For Test
    [SerializeField] TextMeshProUGUI EnemyMineralText;

    public EnemySpawnedUnitList UnitList = new EnemySpawnedUnitList();



    // 참조
    public EnemySpawnQueue EnemySpawnQueue;       // 생산 예약 큐를 관리
    public EnemyUnitSpawner EnemyUnitSpawner;     // 유닛 생산

    int _baseMineralGen;
    float _mineralGainCool;
    float _currentMineralGainCool;

    // 적 보유자원
    public int EnemyMineral
    {
        get
        {
            return _mineral;
        }
        set
        {
            _mineral = value;
        }
    }
    int _mineral;

    // 유닛이 생성가능한지 확인하는 bool


    // 유닛이 생성가능한지 확인하는 bool
    public bool IsCanSpawnUnit => UnitList.TotalUnitCount() < EnemyUnitSpawner.MaxUnitCount;

    #region 싱글톤
    public static EnemySpawnManager Instance => _instance;

    static EnemySpawnManager _instance;

    private void Awake()
    {
        _instance = this;

        EnemyMineral = 200;
        _baseMineralGen = 500;
        _mineralGainCool = 1f;

        EnemySpawnQueue = GetComponent<EnemySpawnQueue>();
        EnemyUnitSpawner = GetComponent<EnemyUnitSpawner>();


    }
    #endregion

    public void Update()
    {
        _currentMineralGainCool += Time.deltaTime;
        if (_currentMineralGainCool > _mineralGainCool)
        {
            EnemyMineral += _baseMineralGen;
            _currentMineralGainCool = 0;
        }

        EnemyMineralText.text = EnemyMineral.ToString();
    }
}