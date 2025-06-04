using TMPro;
using UnityEngine;

// �̱���
// ���� �ڿ��� ���� ������ �����Ѵ�.
public class EnemySpawnManager : MonoBehaviour
{
    // Properties
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

    public bool IsCanSpawnUnit => UnitList.TotalUnitCount() < EnemyUnitSpawner.MaxUnitCount; // ������ ������������ Ȯ���ϴ� bool


    // Fields   
    [SerializeField] TextMeshProUGUI EnemyMineralText;                  // For Test

    public EnemySpawnedUnitList UnitList = new EnemySpawnedUnitList();  // ���� ���� ������ ������ ����� ����

    //����
    public EnemySpawnQueue EnemySpawnQueue;                             // ���� ���� ť�� ����
    public EnemyUnitSpawner EnemyUnitSpawner;                           // ���� ����

    // �ڿ�����
    public float BaseMineralGen;         // �ʴ� �̳׶� ȹ�淮              
    public float _cumlativeMineral;      // ������ �̳׶� ȹ�淮. �� ������ 3���� �Ǳ������� �� �������� �����ϸ�, �� ������
                                         // ���׷��̵� ����� 4�踦 �����ϸ� ���� ������ ���׷��̵��ϰ� �ʱ�ȭ. �׽�Ʈ ���� public ���� ��.

    const float MINERAL_GAIN_COOL= 1f;   // ���� ���� �̳׶��� ��� ���� �ʿ��� �ð�
    const float START_DELAY = 10f;       // ���� �ΰ��� ������ 10�� ������ �� ���� ���� x

    float _cumulativeMineralGainCool;    // ���� ���� �̳׶��� ��� ���� �ʿ��� �ð��� ����ϱ����� �����ϴ� �ð�.
    float _cumulativeStartDelay;         // START_DELAY �� ���������� üũ�ϱ� ���� float ����
    float _mineral;                      // �� �����ڿ�

    bool _isStartDelayFinish;            // _cumulativeStartDelay < START_DELAY �̶�� true


    #region �̱���
    public static EnemySpawnManager Instance => _instance;

    static EnemySpawnManager _instance;

    private void Awake()
    {
        _instance = this;

        EnemySpawnQueue = GetComponent<EnemySpawnQueue>();
        EnemyUnitSpawner = GetComponent<EnemyUnitSpawner>();
    }

    #endregion


    // UnityLifeCycle
    private void Start()
    {
        EnemyMineral = 0;     // ���ʿ� ���� �����ϴ� �̳׶� ��. 0���� ����.
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

        // ���� ���� ���°� �ƴϸ� ����
        if (GameManager.Instance.IsGameOver || _isStartDelayFinish == false)
            return;

        
        // ���� �ð����� ������ ���� �̳׶��� ȹ��
        _cumulativeMineralGainCool += Time.deltaTime;
        if (_cumulativeMineralGainCool > MINERAL_GAIN_COOL)
        {
            EnemyMineral += BaseMineralGen;

            // ���� ������ ���� ���׷��̵� ���°� �ƴ϶��
            if(UnitAttackManager.Instance.EnemyHome.CurrentEnemyHomeLevel <2)
            {
                _cumlativeMineral += BaseMineralGen; // ���׷��̵带 ���� �̳׶� ����

                // ���׷��̵� ����� �����ߴٸ� ���׷��̵� ����. 
                if (_cumlativeMineral > UnitAttackManager.Instance.EnemyHome.EnemyHomeUpgradeCost*2)
                {
                    EnemyMineral -= UnitAttackManager.Instance.EnemyHome.EnemyHomeUpgradeCost;  // ���׷��̵� ��븸ŭ �̳׶� ����
                    BaseMineralGen *= 2;
                    UnitAttackManager.Instance.EnemyHome.SetHomeNextLevel();                    // ���������� ���ϴ� ���� ������ ���� ����
                    _cumlativeMineral = 0;                                                      // ���� �̳׶� �ʱ�ȭ
                }
            }
            _cumulativeMineralGainCool = 0;                 // �̳׶��� ������� ��Ÿ�� �ʱ�ȭ
        }

        EnemyMineralText.text = EnemyMineral.ToString();    // �ؽ�Ʈ�� ���. �׽�Ʈ �Ϸ�� ����
    }


    // Methods
    // �׽�Ʈ �޼���. ��ư Ŭ���� ���� �������� �ڿ� ȹ��
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