using TMPro;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    // For Test
    [SerializeField] TextMeshProUGUI EnemyMineralText;


    // 적이 현재 생성한 유닛의 목록
    public EnemySpawnedUnitList UnitList = new EnemySpawnedUnitList();


    // 참조
    public EnemySpawnQueue EnemySpawnQueue;       // 생산 예약 큐를 관리
    public EnemyUnitSpawner EnemyUnitSpawner;     // 유닛 생산

    // 자원관련
    public float BaseMineralGen;                 // 초당 미네랄 획득량              
    public float _cumlativeMineral;        // 누적된 미네랄 획득량. 적 본진이 3레벨 되기전까지 각 레벨마다 누적하며, 이 변수가
                                         // 업그레이드 비용의 4배를 누적하면 적은 본진을 업그레이드하고 초기화. 테스트 이후 public 지울 것.

    const float MINERAL_GAIN_COOL= 1f;   // 적이 다음 미네랄을 얻기 위해 필요한 시간
    float _cumulativeMineralGainCool;    // 적이 다음 미네랄을 얻기 위해 필요한 시간을 계산하기위해 누적하는 시간.

    bool _isStartDelayFinish;
    const float START_DELAY = 10f; 
    float _cumulativeStartDelay;



    // 적 보유자원
    public float EnemyMineral
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
    float _mineral;

  
    // 유닛이 생성가능한지 확인하는 bool
    public bool IsCanSpawnUnit => UnitList.TotalUnitCount() < EnemyUnitSpawner.MaxUnitCount;

    #region 싱글톤
    public static EnemySpawnManager Instance => _instance;

    static EnemySpawnManager _instance;

    private void Awake()
    {
        _instance = this;

        EnemySpawnQueue = GetComponent<EnemySpawnQueue>();
        EnemyUnitSpawner = GetComponent<EnemyUnitSpawner>();
    }

    #endregion


    private void Start()
    {
        EnemyMineral = 0;     // 최초에 적이 보유하는 미네랄 양. 0으로 조정.
        BaseMineralGen = GameManager.Instance.DifficultyData.EnemyMineralEarn;
    }

    public void Update()
    {
        if(_isStartDelayFinish == false && _cumulativeStartDelay < START_DELAY )
        {
            _cumulativeStartDelay += Time.deltaTime;
        }
        else
        {
            _isStartDelayFinish = true;
        }

        // 게임 오버 상태가 아니면 진행
        if (GameManager.Instance.IsGameOver || _isStartDelayFinish == false)
            return;

        
        // 일정 시간마다 정해진 양의 미네랄을 획득
        _cumulativeMineralGainCool += Time.deltaTime;
        if (_cumulativeMineralGainCool > MINERAL_GAIN_COOL)
        {
            EnemyMineral += BaseMineralGen;

            // 적의 본진이 최종 업그레이드 상태가 아니라면
            if(UnitAttackManager.Instance.EnemyHome.CurrentEnemyHomeLevel <2)
            {
                _cumlativeMineral += BaseMineralGen; // 업그레이드를 위한 미네랄 누적

                // 업그레이드 비용을 누적했다면 업그레이드 진행. 
                if (_cumlativeMineral > UnitAttackManager.Instance.EnemyHome.EnemyHomeUpgradeCost*2)
                {
                    EnemyMineral -= UnitAttackManager.Instance.EnemyHome.EnemyHomeUpgradeCost;  // 업그레이드 비용만큼 미네랄 차감
                    BaseMineralGen *= 2;
                    UnitAttackManager.Instance.EnemyHome.SetHomeNextLevel();                    // 레벨업으로 변하는 적군 본진의 내용 세팅
                    _cumlativeMineral = 0;                                                      // 누적 미네랄 초기화
                }
            }
            _cumulativeMineralGainCool = 0;                 // 미네랄을 얻기위한 쿨타임 초기화
        }

        EnemyMineralText.text = EnemyMineral.ToString();    // 텍스트로 출력. 테스트 완료시 삭제
    }


    // 테스트 메서드
    public void OnTestGetMineral()
    {
        EnemyMineral += 500;
        _cumlativeMineral += 500;

        if (UnitAttackManager.Instance.EnemyHome.CurrentEnemyHomeLevel < 2)
        {
            if (_cumlativeMineral > UnitAttackManager.Instance.EnemyHome.EnemyHomeUpgradeCost * 4)
            {
                EnemyMineral -= UnitAttackManager.Instance.EnemyHome.EnemyHomeUpgradeCost;
                _cumlativeMineral = 0;
                UnitAttackManager.Instance.EnemyHome.SetHomeNextLevel();
            }
        }
        EnemyMineralText.text = EnemyMineral.ToString();
    }
}