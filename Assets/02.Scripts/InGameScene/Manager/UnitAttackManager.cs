using UnityEngine;

// �̱���
// ���ӿ� ������ ���ֵ��� ����, �ǰ�, Ÿ�� ������ �����Ѵ�.
public class UnitAttackManager : MonoBehaviour
{
    //Fields
    // �ǰ� ������ ������Ʈ ����
    [SerializeField] public PlayerUnit PlayerFirstUnit;         // �÷��̾��� ���� ����
    [SerializeField] public EnemyUnit EnemyFirstUnit;           // ���� ���� ����
    public PlayerHome PlayerHome;                               // �÷��̾� ����
    public EnemyHome EnemyHome;                                 // �� ����

    [HideInInspector] public LongRangeAttack LongRangeAttack;   // ���Ÿ� ����

    [SerializeField] public HomeHealthUI HomeHealthUI;          // ���� ü�� UI


    #region �̱���
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
        PlayerFirstUnit = gameObject;   // �÷��̾� ���� ���� ����
    }

    public void SetEnemyFirstUnit(EnemyUnit gameObject)
    {
        EnemyFirstUnit = gameObject;    // �� ���� ���� ����
    }


}