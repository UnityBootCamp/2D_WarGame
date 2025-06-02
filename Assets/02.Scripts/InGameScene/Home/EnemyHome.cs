using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHome : MonoBehaviour
{
    public float _hp;               // 적 본진의 현재 체력. 테스트 완료 후 public 지울 것
    float _maxHp;                   // 적 본진의 최대 체력
    public int _currentHomeLevel;   // 적 본진의 현재 레벨. 테스트 완료 후 public 지울 것

    public float _homeUpgradeCost;    // 적이 본진을 업그레이드하는데에 필요한 비용

    public int CurrentEnemyHomeLevel => _currentHomeLevel;  // 적의 현재 본진 레벨을 반환하는 프로퍼티. 0~2레벨 까지 존재
    public float EnemyHomeUpgradeCost => _homeUpgradeCost;    // 적의 현재 업그레이드 비용을 반환하는 프로퍼티


    [SerializeField] Sprite[] _homeSprites;                 // 레벨 별 본진의 스프라이트를 저장.
    SpriteRenderer _currentHomeSpriteRenderer;              // 레벨 별 본진의 스프라이트를 보여주는 SpriteRenderer.
    public Vector3 EnemyHomeSize;                           // 적 본진의 bound.

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
        // 테스트코드
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
        EnemySpawnManager.Instance.EnemyUnitSpawner.MaxUnitCount += 5;                  // 전투 유닛 생성가능 수 증가
        EnemySpawnManager.Instance.BaseMineralGen *= GameManager.Instance.DifficultyData.DifficultyCoefficient;


        _maxHp += GameManager.Instance.DifficultyData.EnemyHomeHp;
        _hp += GameManager.Instance.DifficultyData.EnemyHomeHp;     // 레벨업하면 성 체력 증가
        EnemySpawnManager.Instance.EnemyUnitSpawner.MaxUnitCount += 5;  // 생산가능 유닛 5 증가

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
