using TMPro;
using UnityEngine;
using System.Collections;

public class PlayerHome : MonoBehaviour
{
    public float PlayerHomeHp => _hp;
    public int CurrentPlayerHomeLevel => _currentHomeLevel;
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


    [SerializeField] Sprite[] _homeSprites;     // ���� ��������Ʈ
    [SerializeField] EnemyHome _enemyHome;      // �� ����

    Coroutine _getDamageCoroutine;


    SpriteRenderer _currentHomeSpriteRenderer;
    public Vector3 PlayerHomeSize;

    public float _hp;
    public int _currentHomeLevel;
    float _homeUpgradeCost;

    float _maxHp = 500;


    private void Awake()
    {
        _hp = _maxHp;
        _currentHomeSpriteRenderer = GetComponent<SpriteRenderer>();
        PlayerHomeSize = GetComponent<SpriteRenderer>().bounds.size/3;
        _currentHomeSpriteRenderer.sprite = _homeSprites[_currentHomeLevel];
    }

    private void Start()
    {

        PlayerHomeUpgradeCost = 8000;
        UnitAttackManager.Instance.HomeHealthUI.SetPlayerHomeHealth(_hp / _maxHp);
        PlayerSpawnManager.Instance.PlayerUnitControlUI.SetUpgradeCost(PlayerHomeUpgradeCost);
    }


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

    IEnumerator C_GetDamageCoroutine()
    {
        _currentHomeSpriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        _currentHomeSpriteRenderer.color = Color.white;
        _getDamageCoroutine = null;
    }


    public void SetHomeNextLevel()
    {
        if (_currentHomeLevel > 2 || PlayerSpawnManager.Instance.Mineral - PlayerHomeUpgradeCost < 0)
            return;

        
        PlayerSpawnManager.Instance.PlayerUnitControlUI.AbleLevelUnit(_currentHomeLevel); // ���� �ر�
        PlayerSpawnManager.Instance.PlayerUnitSpawner.MaxUnitCount += 5;                  // ���� ���� �������� �� ����
        PlayerSpawnManager.Instance.PlayerUnitSpawner.MaxFarmingUnitCount += 10;           // �ϲ� ���� �������� �� ����


        PlayerSpawnManager.Instance.UpdateUnitResourceUI();                               // ���� ���� UI ����
        PlayerSpawnManager.Instance.UpdateFarmingUnitResourceUI();                        // �ϲ� ���� UI ����


        _hp += _maxHp;
        _maxHp *= 2;        // �������ϸ� �� ü�� ����

        UnitAttackManager.Instance.HomeHealthUI.SetPlayerHomeHealth(_hp/ _maxHp);


        _currentHomeLevel++;
        PlayerSpawnManager.Instance.Mineral -= PlayerHomeUpgradeCost;
        PlayerHomeUpgradeCost *= GameManager.Instance.DifficultyData.DifficultyCoefficient; // ������ŭ ���׷��̵� ��� ����
        PlayerSpawnManager.Instance.PlayerUnitControlUI.SetUpgradeCost(PlayerHomeUpgradeCost); // �ؽ�Ʈ ����

        _currentHomeSpriteRenderer.sprite = _homeSprites[_currentHomeLevel];

        if (_currentHomeLevel == 2)
        {
            PlayerSpawnManager.Instance.PlayerUnitControlUI.DisableUpgradeButton();
        }
    }

    public void OnDestroyPlayerHome()
    {
        GameManager.Instance.OnLose();
    }



}
