using System.Collections;
using UnityEngine;


// 적의 본진에 관련된 클래스
public class EnemyHome : MonoBehaviour
{
    // Properties
    public Vector3 EnemyHomeSize => _enemyHomeSize;         // 적 본진의 bound를 외부에서 읽기 접근하도록 제공하는 프로퍼티    
    public int CurrentEnemyHomeLevel => _currentHomeLevel;    // 적의 현재 본진 레벨을 반환하는 프로퍼티. 0~2레벨 까지 존재
    public float EnemyHomeUpgradeCost => _homeUpgradeCost;    // 적의 현재 업그레이드 비용을 반환하는 프로퍼티


    // Fields
    [SerializeField] Sprite[] _homeSprites;                 // 레벨 별 본진의 스프라이트를 저장.
    float _hp;                                              // 적 본진의 현재 체력
    float _maxHp;                                           // 적 본진의 최대 체력
    float _homeUpgradeCost;                                 // 적이 본진을 업그레이드하는데에 필요한 비용
    int _currentHomeLevel;                                  // 적 본진의 현재 레벨
    SpriteRenderer _currentHomeSpriteRenderer;              // 레벨 별 본진의 스프라이트를 보여주는 SpriteRenderer.
    Coroutine _getDamageCoroutine;                          // 적 본진이 데미지를 입는 코루틴을 저장하는 변수. 코루틴이 중첩되지 않도록 조정
    Vector3 _enemyHomeSize;                                 // 적 본진의 bound.
                 
    
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
        // 테스트코드. F1 누르면 적 본진 파괴
        if (Input.GetKeyDown(KeyCode.F1))
        {
            OnDestroyEnemyHome();
        }
    }

    
    // Methods
    public void GetDamage(float value)
    {
        _hp -= value;
        UnitAttackManager.Instance.HomeHealthUI.SetEnemyHomeHealth(_hp / _maxHp);   // 본진 체력을 표시해주는 UI 갱신


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

    // 적 본진 업그레이드
    public void SetHomeNextLevel()
    {
        if (_currentHomeLevel > 2)
            return;

        EnemySpawnManager.Instance.EnemyUnitSpawner.AddUnit();      // 유닛 해금
        EnemySpawnManager.Instance.BaseMineralGen *= GameManager.Instance.DifficultyData.DifficultyCoefficient; // 적 자원 생산량 증가


        _maxHp += GameManager.Instance.DifficultyData.EnemyHomeHp;
        _hp += GameManager.Instance.DifficultyData.EnemyHomeHp;                         // 레벨업하면 성 체력 증가
        EnemySpawnManager.Instance.EnemyUnitSpawner.UpgradeMaxUnitCount();              // 최대 유닛 수 5 증가

        UnitAttackManager.Instance.HomeHealthUI.SetEnemyHomeHealth(_hp / _maxHp);

        _homeUpgradeCost *= GameManager.Instance.DifficultyData.DifficultyCoefficient;  // 본진 업그레이드 코스트 증가

        _currentHomeLevel++;
        _currentHomeSpriteRenderer.sprite = _homeSprites[_currentHomeLevel];            // 스프라이트 변경

    }

    // 적 본진 파괴 시
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
