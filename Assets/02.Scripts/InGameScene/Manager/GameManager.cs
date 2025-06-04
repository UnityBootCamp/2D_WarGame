using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


// 싱글톤
// 게임 전체에 영향을 미치는 요소들을 관리하는 매니저 클래스
// 게임의 승리와 패배, 플레이어가 획득한 점수, 게임의 TimeScale, 난이도를 관리함.
public class GameManager : MonoBehaviour
{
    public SettingUI OptionaPanel => _settingPanel;
    public UIExplainUI ExplainPanel => _explainPanel;
    public static GameManager Instance => _instance;


    
    // 패널 참조
    [SerializeField] GameObject _clearPanel;        // 클리어 시 출력하는 패널
    [SerializeField] GameObject _youLosePanel;      // 패배 시 출력하는 패널
    [SerializeField] Image _sceneChangePanel;       // 화면전환 패널
    [SerializeField] SettingUI _settingPanel;       // 옵션 패널
    [SerializeField] UIExplainUI _explainPanel;     // 인게임 진입시 나타나는 설명 패널


    [SerializeField] DifficultyData[] DifficultyDatas;      // 난이도 관련 데이터. 0 = easy, 1 = normal, 2 = hard
    [HideInInspector] public DifficultyData DifficultyData; // 이번에 선택된 난이도 저장

    public bool IsGameOver;                                 // 게임오버가 되었는가
    public int TotalKill;                                   // 플레이어가 처치한 모든 적 유닛의 수
    public int TotalSpawn;                                  // 플레이어가 생성한 모든 아군 전투 유닛의 수

    const float FADE_DURATION = 0.5f;


    static GameManager _instance;


    private void Awake()
    {

        _sceneChangePanel.gameObject.SetActive(true);   // 로딩을 위한 화면 암흑처리
        _settingPanel.OnStart();
        _explainPanel.OnStart();
        
        StartCoroutine(C_SceneChange());


        _instance = this;
        IsGameOver = false;
        DifficultyData = DifficultyDatas[PlayerPrefs.GetInt("Difficulty")];

        // 패널 비활성화
        _settingPanel.gameObject.SetActive(false);
        _clearPanel.SetActive(false);
        _youLosePanel.SetActive(false);
    }

    public void GameStart()
    {
        Time.timeScale = 1;
    }

    public void Restart()
    {
        Time.timeScale = 1;
        _settingPanel.gameObject.SetActive(false);

    }

    public void Pause()
    {
        Time.timeScale = 0;
        _settingPanel.gameObject.SetActive(true);

    }

    // 점수 계산 메서드
    public int calcScore()
    {
        return TotalKill + TotalSpawn;
    }

    // 점수를 기반으로 플레이 랭크를 계산하는 메서드
    public string CalcRank(int score, out Color color)
    {
        if(0<score && score < 1000)
        {
            color = Color.black;
            return "F";
        }
        else if (1000 <= score && score < 2000)
        {
            color = Color.gray;
            return "E";
        }
        else if (2000 <= score && score < 3000)
        {
            color = new Color(0.647f, 0.165f, 0.165f);        // 갈색
            return "D";
        }
        else if (3000 <= score && score < 4000)
        {
            color = new Color(1.0f, 0.647f, 0.0f);            // 주황
            return "C";
        }
        else if (4000 <= score && score < 5000)
        {
            color = new Color(1.0f, 0.753f, 0.796f);          // 분홍
            return "B";
        }
        else if (5000 <= score && score < 6000)
        {
            color = Color.red;                 
            return "A";
        }
        else
        {
            color = new Color(1.0f, 0.843f, 0.0f);            // 금색
            return "S";
        }

    }

    // 클리어 시 실행되는 메서드
    public void OnClear()
    {
        IsGameOver = true;

        TextMeshProUGUI earnedRank = _clearPanel.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI totalSpawnUnit = _clearPanel.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI totalKillUnit = _clearPanel.transform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>();

        Color rankColor;

        earnedRank.text = CalcRank(calcScore() + 3000, out rankColor);
        earnedRank.color = rankColor;
        totalSpawnUnit.text = "Total Spawn Unit :  " + TotalSpawn;
        totalKillUnit.text = "Total Kill Unit :  " + TotalKill;

        _clearPanel.SetActive(true);
    }

    // 패배 시 실행되는 메서드
    public void OnLose()
    {
        IsGameOver = true;
        
        TextMeshProUGUI earnedRank = _clearPanel.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI totalSpawnUnit = _clearPanel.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI totalKillUnit = _clearPanel.transform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>();

        Color rankColor;

        earnedRank.text = CalcRank(calcScore() + 3000, out rankColor);
        earnedRank.color = rankColor;
        totalSpawnUnit.text = "Total Spawn Unit :  " + TotalSpawn;
        totalKillUnit.text = "Total Kill Unit :  " + TotalKill;

        _youLosePanel.SetActive(true);
    }

    // 클리어, 패배 시 출력되는 패널의 홈 버튼을 누를 시 실행되는 메서드
    public void OnClickHomeButton()
    {
        SceneManager.LoadScene("TitleScene");
    }

    // 씬 전환 코루틴
    IEnumerator C_SceneChange()
    {
        float cumulativeTime = 0f;
        while (cumulativeTime < FADE_DURATION)
        {
            cumulativeTime += Time.deltaTime;
            _sceneChangePanel.color = Color.Lerp(Color.black, new Color(0,0,0,0), cumulativeTime / FADE_DURATION);
            yield return null;
        }

        _sceneChangePanel.gameObject.SetActive(false);
        _sceneChangePanel.color = Color.black;
        Time.timeScale = 0;
    }
}
