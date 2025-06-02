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


    [SerializeField] Sprite[] _homeSprites;     // 본진 스프라이트
    [SerializeField] EnemyHome _enemyHome;      // 적 본진

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

        
        PlayerSpawnManager.Instance.PlayerUnitControlUI.AbleLevelUnit(_currentHomeLevel); // 유닛 해금
        PlayerSpawnManager.Instance.PlayerUnitSpawner.MaxUnitCount += 5;                  // 전투 유닛 생성가능 수 증가
        PlayerSpawnManager.Instance.PlayerUnitSpawner.MaxFarmingUnitCount += 10;           // 일꾼 유닛 생성가능 수 증가


        PlayerSpawnManager.Instance.UpdateUnitResourceUI();                               // 전투 유닛 UI 갱신
        PlayerSpawnManager.Instance.UpdateFarmingUnitResourceUI();                        // 일꾼 유닛 UI 갱신


        _hp += _maxHp;
        _maxHp *= 2;        // 레벨업하면 성 체력 증가

        UnitAttackManager.Instance.HomeHealthUI.SetPlayerHomeHealth(_hp/ _maxHp);


        _currentHomeLevel++;
        PlayerSpawnManager.Instance.Mineral -= PlayerHomeUpgradeCost;
        PlayerHomeUpgradeCost *= GameManager.Instance.DifficultyData.DifficultyCoefficient; // 배율만큼 업그레이드 비용 증가
        PlayerSpawnManager.Instance.PlayerUnitControlUI.SetUpgradeCost(PlayerHomeUpgradeCost); // 텍스트 갱신

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
