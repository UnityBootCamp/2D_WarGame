using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHome : MonoBehaviour
{
    public float PlayerHomeHp => _hp;
    public int CurrentPlayerHomeLevel => _currentHomeLevel;
    public int PlayerHomeUpgradeCost 
    {
        get
        {
            return _homeUpgradeCost;
        }
        set
        {

            _homeUpgradeCost = value;
            _enemyHome.SetEnemyUpgradeCost(_homeUpgradeCost);


        }
    }


    [SerializeField] Sprite[] _homeSprites;     // 본진 스프라이트
    [SerializeField] EnemyHome _enemyHome;      // 적 본진




    SpriteRenderer _currentHomeSpriteRenderer;
    public Vector3 PlayerHomeSize;

    public float _hp;
    public int _currentHomeLevel;
    int _homeUpgradeCost;

    float _maxHp = 100;


    private void Awake()
    {
        _hp = _maxHp;
        _currentHomeSpriteRenderer = GetComponent<SpriteRenderer>();
        PlayerHomeSize = GetComponent<SpriteRenderer>().bounds.size;
        _currentHomeSpriteRenderer.sprite = _homeSprites[_currentHomeLevel];
    }

    private void Start()
    {

        PlayerHomeUpgradeCost = 500;
        UnitAttackManager.Instance.HomeHealthUI.SetPlayerHomeHealth(_hp / _maxHp);
    }


    public void GetDamage(float value)
    {
        _hp -= value;
        UnitAttackManager.Instance.HomeHealthUI.SetPlayerHomeHealth(_hp/_maxHp);
        if (_hp <= 0)
        {
            OnDestroyPlayerHome();
        }
    }

    public void SetHomeLevel()
    {
        if (_currentHomeLevel > 2)
            return;

        
        PlayerSpawnManager.Instance.PlayerUnitControlUI.AbleLevelUnit(_currentHomeLevel); // 유닛 해금

        _currentHomeLevel++;
        PlayerSpawnManager.Instance.Mineral -= PlayerHomeUpgradeCost;
        PlayerHomeUpgradeCost *= 5;
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
