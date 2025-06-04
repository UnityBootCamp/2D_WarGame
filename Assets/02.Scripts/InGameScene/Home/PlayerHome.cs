using UnityEngine;
using System.Collections;

public class PlayerHome : MonoBehaviour
{
    // Properties
    public float PlayerHomeUpgradeCost 
    {
        get
        {
            return _homeUpgradeCost;
        }
        set
        {

            _homeUpgradeCost = value;
            
        }
    }
    public Vector3 PlayerHomeSize => _playerHomeSize;


    // Fields
    [SerializeField] Sprite[] _homeSprites;     // ���� ��������Ʈ
    [SerializeField] EnemyHome _enemyHome;      // �� ����
    float _hp;                                  // ���� ���� ü��
    float _maxHp = 500;                         // ���� �ִ� ü��
    float _homeUpgradeCost;                     // ���� ���׷��̵� �ڽ�Ʈ
    int _currentHomeLevel;                      // ���� ���� ����
    SpriteRenderer _currentHomeSpriteRenderer;  // ���� �� ������ ��������Ʈ�� �����ִ� SpriteRenderer.
    Coroutine _getDamageCoroutine;              // �� ������ �������� �Դ� �ڷ�ƾ�� �����ϴ� ����. �ڷ�ƾ�� ��ø���� �ʵ��� ����
    Vector3 _playerHomeSize;                    // �� ������ bound.


    // UnityLifeCycle
    private void Awake()
    {
        _hp = _maxHp;
        _currentHomeSpriteRenderer = GetComponent<SpriteRenderer>();
        _playerHomeSize = GetComponent<SpriteRenderer>().bounds.size/3;
        _currentHomeSpriteRenderer.sprite = _homeSprites[_currentHomeLevel];
    }

    private void Start()
    {

        PlayerHomeUpgradeCost = 8000;
        UnitAttackManager.Instance.HomeHealthUI.SetPlayerHomeHealth(_hp / _maxHp);
        PlayerSpawnManager.Instance.HomeUpgradeUI.SetUpgradeCost(PlayerHomeUpgradeCost);
    }


    // Methods
    public void GetDamage(float value)
    {
        _hp -= value;
        UnitAttackManager.Instance.HomeHealthUI.SetPlayerHomeHealth(_hp/_maxHp);
        if (_hp <= 0)
        {
            OnDestroyPlayerHome();
        }
        else
        {
            if (_getDamageCoroutine != null)
            {
                _getDamageCoroutine = null;
            }
            _getDamageCoroutine = StartCoroutine(C_GetDamageCoroutine());
        }
    }

    public void SetHomeNextLevel()
    {
        if (_currentHomeLevel > 2 || PlayerSpawnManager.Instance.Mineral - PlayerHomeUpgradeCost < 0)
            return;


        PlayerSpawnManager.Instance.PlayerUnitControlUI.AbleLevelUnit(_currentHomeLevel); // ���� �ر�
        PlayerSpawnManager.Instance.PlayerUnitSpawner.UpgradeMaxUnit();


        PlayerSpawnManager.Instance.UpdateUnitResourceUI();                               // ���� ���� UI ����
        PlayerSpawnManager.Instance.UpdateFarmingUnitResourceUI();                        // �ϲ� ���� UI ����


        _hp += _maxHp;
        _maxHp *= 2;        // �������ϸ� �� ü�� ����

        UnitAttackManager.Instance.HomeHealthUI.SetPlayerHomeHealth(_hp / _maxHp);


        _currentHomeLevel++;
        PlayerSpawnManager.Instance.Mineral -= PlayerHomeUpgradeCost;
        PlayerHomeUpgradeCost *= GameManager.Instance.DifficultyData.DifficultyCoefficient; // ������ŭ ���׷��̵� ��� ����
        PlayerSpawnManager.Instance.HomeUpgradeUI.SetUpgradeCost(PlayerHomeUpgradeCost);    // �ؽ�Ʈ ����

        _currentHomeSpriteRenderer.sprite = _homeSprites[_currentHomeLevel];

        if (_currentHomeLevel == 2)
        {
            PlayerSpawnManager.Instance.HomeUpgradeUI.DisableUpgradeButton();
        }
    }

    public void OnDestroyPlayerHome()
    {
        GameManager.Instance.OnLose();
    }

    IEnumerator C_GetDamageCoroutine()
    {
        _currentHomeSpriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        _currentHomeSpriteRenderer.color = Color.white;
        _getDamageCoroutine = null;
    }
}
