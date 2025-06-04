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
    [SerializeField] Sprite[] _homeSprites;     // 본진 스프라이트
    [SerializeField] EnemyHome _enemyHome;      // 적 본진
    float _hp;                                  // 본진 현재 체력
    float _maxHp = 500;                         // 본진 최대 체력
    float _homeUpgradeCost;                     // 본진 업그레이드 코스트
    int _currentHomeLevel;                      // 현재 본진 레벨
    SpriteRenderer _currentHomeSpriteRenderer;  // 레벨 별 본진의 스프라이트를 보여주는 SpriteRenderer.
    Coroutine _getDamageCoroutine;              // 적 본진이 데미지를 입는 코루틴을 저장하는 변수. 코루틴이 중첩되지 않도록 조정
    Vector3 _playerHomeSize;                    // 적 본진의 bound.


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


        PlayerSpawnManager.Instance.PlayerUnitControlUI.AbleLevelUnit(_currentHomeLevel); // 유닛 해금
        PlayerSpawnManager.Instance.PlayerUnitSpawner.UpgradeMaxUnit();


        PlayerSpawnManager.Instance.UpdateUnitResourceUI();                               // 전투 유닛 UI 갱신
        PlayerSpawnManager.Instance.UpdateFarmingUnitResourceUI();                        // 일꾼 유닛 UI 갱신


        _hp += _maxHp;
        _maxHp *= 2;        // 레벨업하면 성 체력 증가

        UnitAttackManager.Instance.HomeHealthUI.SetPlayerHomeHealth(_hp / _maxHp);


        _currentHomeLevel++;
        PlayerSpawnManager.Instance.Mineral -= PlayerHomeUpgradeCost;
        PlayerHomeUpgradeCost *= GameManager.Instance.DifficultyData.DifficultyCoefficient; // 배율만큼 업그레이드 비용 증가
        PlayerSpawnManager.Instance.HomeUpgradeUI.SetUpgradeCost(PlayerHomeUpgradeCost);    // 텍스트 갱신

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
