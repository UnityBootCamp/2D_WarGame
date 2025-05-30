using TMPro;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    // For Test
    [SerializeField] TextMeshProUGUI EnemyMineralText;

    public EnemySpawnedUnitList UnitList = new EnemySpawnedUnitList();



    // ����
    public EnemySpawnQueue EnemySpawnQueue;       // ���� ���� ť�� ����
    public EnemyUnitSpawner EnemyUnitSpawner;     // ���� ����

    int _baseMineralGen;
    public int _cumlativeMineral;

    float _mineralGainCool;
    float _cumulativeMineralGainCool;



    // �� �����ڿ�
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

    // ������ ������������ Ȯ���ϴ� bool


    // ������ ������������ Ȯ���ϴ� bool
    public bool IsCanSpawnUnit => UnitList.TotalUnitCount() < EnemyUnitSpawner.MaxUnitCount;

    #region �̱���
    public static EnemySpawnManager Instance => _instance;

    static EnemySpawnManager _instance;

    private void Awake()
    {
        _instance = this;

        EnemyMineral = 200;
        _baseMineralGen = 50;
        _mineralGainCool = 1f;

        EnemySpawnQueue = GetComponent<EnemySpawnQueue>();
        EnemyUnitSpawner = GetComponent<EnemyUnitSpawner>();


    }
    #endregion

    public void Update()
    {

        if (GameManager.Instance.IsGameOver)
            return;

        _cumulativeMineralGainCool += Time.deltaTime;
        if (_cumulativeMineralGainCool > _mineralGainCool)
        {
            EnemyMineral += _baseMineralGen;

            if(UnitAttackManager.Instance.EnemyHome.CurrentEnemyHomeLevel <2)
            {
                _cumlativeMineral += _baseMineralGen;

                if (_cumlativeMineral > UnitAttackManager.Instance.EnemyHome.EnemyHomeUpgradeCost * 4)
                {
                    EnemyMineral -= UnitAttackManager.Instance.EnemyHome.EnemyHomeUpgradeCost;
                    UnitAttackManager.Instance.EnemyHome.SetHomeNextLevel();
                    _cumlativeMineral = 0;
                }
            }

            _cumulativeMineralGainCool = 0;
        }

        EnemyMineralText.text = EnemyMineral.ToString();
    }


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