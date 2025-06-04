using System.Collections;
using UnityEngine;


// ���� ������ ���õ� Ŭ����
public class EnemyHome : MonoBehaviour
{
    // Properties
    public Vector3 EnemyHomeSize => _enemyHomeSize;         // �� ������ bound�� �ܺο��� �б� �����ϵ��� �����ϴ� ������Ƽ    
    public int CurrentEnemyHomeLevel => _currentHomeLevel;    // ���� ���� ���� ������ ��ȯ�ϴ� ������Ƽ. 0~2���� ���� ����
    public float EnemyHomeUpgradeCost => _homeUpgradeCost;    // ���� ���� ���׷��̵� ����� ��ȯ�ϴ� ������Ƽ


    // Fields
    [SerializeField] Sprite[] _homeSprites;                 // ���� �� ������ ��������Ʈ�� ����.
    float _hp;                                              // �� ������ ���� ü��
    float _maxHp;                                           // �� ������ �ִ� ü��
    float _homeUpgradeCost;                                 // ���� ������ ���׷��̵��ϴµ��� �ʿ��� ���
    int _currentHomeLevel;                                  // �� ������ ���� ����
    SpriteRenderer _currentHomeSpriteRenderer;              // ���� �� ������ ��������Ʈ�� �����ִ� SpriteRenderer.
    Coroutine _getDamageCoroutine;                          // �� ������ �������� �Դ� �ڷ�ƾ�� �����ϴ� ����. �ڷ�ƾ�� ��ø���� �ʵ��� ����
    Vector3 _enemyHomeSize;                                 // �� ������ bound.
                 
    
    // UnityLifeCycle
    private void Awake()
    {
        _currentHomeSpriteRenderer = GetComponent<SpriteRenderer>();
        _enemyHomeSize = GetComponent<SpriteRenderer>().bounds.size/3;
        _currentHomeSpriteRenderer.sprite = _homeSprites[_currentHomeLevel];
        
    }

    private void Start()
    {
        _homeUpgradeCost = 8000;                                    
        _maxHp = GameManager.Instance.DifficultyData.EnemyHomeHp;   
        _hp = _maxHp;
        UnitAttackManager.Instance.HomeHealthUI.SetEnemyHomeHealth(_hp / _maxHp);
    }

    private void Update()
    {
        // �׽�Ʈ�ڵ�. F1 ������ �� ���� �ı�
        if (Input.GetKeyDown(KeyCode.F1))
        {
            OnDestroyEnemyHome();
        }
    }

    
    // Methods
    public void GetDamage(float value)
    {
        _hp -= value;
        UnitAttackManager.Instance.HomeHealthUI.SetEnemyHomeHealth(_hp / _maxHp);   // ���� ü���� ǥ�����ִ� UI ����


        if (_hp <= 0)
        {
            OnDestroyEnemyHome();
        }
        else
        {
            if(_getDamageCoroutine != null)
            {
                _getDamageCoroutine = null;
            }
            _getDamageCoroutine = StartCoroutine(C_GetDamageCoroutine());
        }
    }

    // �� ���� ���׷��̵�
    public void SetHomeNextLevel()
    {
        if (_currentHomeLevel > 2)
            return;

        EnemySpawnManager.Instance.EnemyUnitSpawner.AddUnit();      // ���� �ر�
        EnemySpawnManager.Instance.BaseMineralGen *= GameManager.Instance.DifficultyData.DifficultyCoefficient; // �� �ڿ� ���귮 ����


        _maxHp += GameManager.Instance.DifficultyData.EnemyHomeHp;
        _hp += GameManager.Instance.DifficultyData.EnemyHomeHp;                         // �������ϸ� �� ü�� ����
        EnemySpawnManager.Instance.EnemyUnitSpawner.UpgradeMaxUnitCount();              // �ִ� ���� �� 5 ����

        UnitAttackManager.Instance.HomeHealthUI.SetEnemyHomeHealth(_hp / _maxHp);

        _homeUpgradeCost *= GameManager.Instance.DifficultyData.DifficultyCoefficient;  // ���� ���׷��̵� �ڽ�Ʈ ����

        _currentHomeLevel++;
        _currentHomeSpriteRenderer.sprite = _homeSprites[_currentHomeLevel];            // ��������Ʈ ����

    }

    // �� ���� �ı� ��
    public void OnDestroyEnemyHome()
    {
        GameManager.Instance.OnClear();
    }

    IEnumerator C_GetDamageCoroutine()
    {
        _currentHomeSpriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        _currentHomeSpriteRenderer.color = Color.white;
        _getDamageCoroutine = null;
    }
}
