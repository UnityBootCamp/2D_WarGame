using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHome : MonoBehaviour
{
    public float _hp;               // �� ������ ���� ü��. �׽�Ʈ �Ϸ� �� public ���� ��
    float _maxHp;                   // �� ������ �ִ� ü��
    public int _currentHomeLevel;   // �� ������ ���� ����. �׽�Ʈ �Ϸ� �� public ���� ��

    public float _homeUpgradeCost;    // ���� ������ ���׷��̵��ϴµ��� �ʿ��� ���

    public int CurrentEnemyHomeLevel => _currentHomeLevel;  // ���� ���� ���� ������ ��ȯ�ϴ� ������Ƽ. 0~2���� ���� ����
    public float EnemyHomeUpgradeCost => _homeUpgradeCost;    // ���� ���� ���׷��̵� ����� ��ȯ�ϴ� ������Ƽ


    [SerializeField] Sprite[] _homeSprites;                 // ���� �� ������ ��������Ʈ�� ����.
    SpriteRenderer _currentHomeSpriteRenderer;              // ���� �� ������ ��������Ʈ�� �����ִ� SpriteRenderer.
    public Vector3 EnemyHomeSize;                           // �� ������ bound.

    Coroutine _getDamageCoroutine;


    private void Awake()
    {
        _currentHomeSpriteRenderer = GetComponent<SpriteRenderer>();
        EnemyHomeSize = GetComponent<SpriteRenderer>().bounds.size/3;
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
        // �׽�Ʈ�ڵ�
        if (Input.GetKeyDown(KeyCode.F1))
        {
            OnDestroyEnemyHome();
        }
    }


    public void GetDamage(float value)
    {
        _hp -= value;
        UnitAttackManager.Instance.HomeHealthUI.SetEnemyHomeHealth(_hp / _maxHp);


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

    IEnumerator C_GetDamageCoroutine()
    {
        _currentHomeSpriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        _currentHomeSpriteRenderer.color = Color.white;
        _getDamageCoroutine = null;
    }


    public void SetEnemyUpgradeCost(float value)
    {
        _homeUpgradeCost = value;
    }

    public void SetHomeNextLevel()
    {
        if (_currentHomeLevel > 2)
            return;

        EnemySpawnManager.Instance.EnemyUnitSpawner.AddUnit();
        EnemySpawnManager.Instance.EnemyUnitSpawner.MaxUnitCount += 5;                  // ���� ���� �������� �� ����
        EnemySpawnManager.Instance.BaseMineralGen *= GameManager.Instance.DifficultyData.DifficultyCoefficient;


        _maxHp += GameManager.Instance.DifficultyData.EnemyHomeHp;
        _hp += GameManager.Instance.DifficultyData.EnemyHomeHp;     // �������ϸ� �� ü�� ����
        EnemySpawnManager.Instance.EnemyUnitSpawner.MaxUnitCount += 5;  // ���갡�� ���� 5 ����

        UnitAttackManager.Instance.HomeHealthUI.SetEnemyHomeHealth(_hp / _maxHp);

        _homeUpgradeCost *= GameManager.Instance.DifficultyData.DifficultyCoefficient;

        _currentHomeLevel++;
        _currentHomeSpriteRenderer.sprite = _homeSprites[_currentHomeLevel];

    }

    public void OnDestroyEnemyHome()
    {
        GameManager.Instance.OnClear();
    }


}
