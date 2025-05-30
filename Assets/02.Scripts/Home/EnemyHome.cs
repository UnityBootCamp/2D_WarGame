using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHome : MonoBehaviour
{
    float _hp;
    float _maxHp = 50;
    public int _currentHomeLevel;

    public int _homeUpgradeCost;

    public float EnemyHomeHp => _hp;
    public int CurrentEnemyHomeLevel => _currentHomeLevel;
    public int EnemyHomeUpgradeCost => _homeUpgradeCost;


    [SerializeField] Sprite[] _homeSprites;


    SpriteRenderer _currentHomeSpriteRenderer;
    public Vector3 EnemyHomeSize;




    private void Awake()
    {
        _hp = _maxHp;
        _currentHomeSpriteRenderer = GetComponent<SpriteRenderer>();
        EnemyHomeSize = GetComponent<SpriteRenderer>().bounds.size;
        _currentHomeSpriteRenderer.sprite = _homeSprites[_currentHomeLevel];
    }


    private void Start()
    {
        UnitAttackManager.Instance.HomeHealthUI.SetEnemyHomeHealth(_hp / _maxHp);
    }


    public void GetDamage(float value)
    {
        _hp -= value;
        UnitAttackManager.Instance.HomeHealthUI.SetEnemyHomeHealth(_hp / _maxHp);
        if (_hp <= 0)
        {
            OnDestroyEnemyHome();
        }
    }

    public void SetEnemyUpgradeCost(int value)
    {
        _homeUpgradeCost = value;
    }

    public void SetHomeNextLevel()
    {
        if (_currentHomeLevel > 2)
            return;

        _currentHomeLevel++;
        EnemySpawnManager.Instance.EnemyMineral -= _homeUpgradeCost;
        _currentHomeSpriteRenderer.sprite = _homeSprites[_currentHomeLevel];

    }

    public void OnDestroyEnemyHome()
    {
        GameManager.Instance.OnClear();
    }


}
