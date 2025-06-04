using UnityEngine;

// 싱글톤
// 게임에 생성된 유닛들의 공격, 피격, 타겟 설정을 관리한다.
public class UnitAttackManager : MonoBehaviour
{
    //Fields
    // 피격 가능한 오브젝트 모음
    [SerializeField] public PlayerUnit PlayerFirstUnit;         // 플레이어의 선봉 유닛
    [SerializeField] public EnemyUnit EnemyFirstUnit;           // 적의 선봉 유닛
    public PlayerHome PlayerHome;                               // 플레이어 본진
    public EnemyHome EnemyHome;                                 // 적 본진

    [HideInInspector] public LongRangeAttack LongRangeAttack;   // 원거리 공격

    [SerializeField] public HomeHealthUI HomeHealthUI;          // 본진 체력 UI


    #region 싱글톤
    public static UnitAttackManager Instance => _instance;

    static UnitAttackManager _instance;

    private void Awake()
    {
        _instance = this;
        LongRangeAttack = GetComponent<LongRangeAttack>();

    }
    #endregion


    // Methods
    public void SetPlayerFirstUnit(PlayerUnit gameObject)
    {
        PlayerFirstUnit = gameObject;   // 플레이어 선봉 유닛 설정
    }

    public void SetEnemyFirstUnit(EnemyUnit gameObject)
    {
        EnemyFirstUnit = gameObject;    // 적 선봉 유닛 설정
    }


}