using UnityEngine;

public class UnitAttackManager : MonoBehaviour
{
    // 피격 가능한 오브젝트 모음
    [SerializeField] public PlayerUnit PlayerFirstUnit;         // 플레이어의 선봉 유닛
    [SerializeField] public EnemyUnit EnemyFirstUnit;           // 적의 선봉 유닛
    public PlayerHome PlayerHome;                               // 플레이어 본진
    public EnemyHome EnemyHome;                                 // 적 본진



    [SerializeField] public HomeHealthUI HomeHealthUI;



    #region 싱글톤
    public static UnitAttackManager Instance => _instance;

    static UnitAttackManager _instance;

    private void Awake()
    {
        _instance = this;

    }
    #endregion
    public void SetPlayerFirstUnit(PlayerUnit gameObject)
    {
        PlayerFirstUnit = gameObject;
    }

    public void SetEnemyFirstUnit(EnemyUnit gameObject)
    {
        EnemyFirstUnit = gameObject;
    }


}