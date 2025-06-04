using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


// �̱���
// ���� ��ü�� ������ ��ġ�� ��ҵ��� �����ϴ� �Ŵ��� Ŭ����
// ������ �¸��� �й�, �÷��̾ ȹ���� ����, ������ TimeScale, ���̵��� ������.
public class GameManager : MonoBehaviour
{
    public SettingUI OptionaPanel => _settingPanel;
    public UIExplainUI ExplainPanel => _explainPanel;
    public static GameManager Instance => _instance;


    
    // �г� ����
    [SerializeField] GameObject _clearPanel;        // Ŭ���� �� ����ϴ� �г�
    [SerializeField] GameObject _youLosePanel;      // �й� �� ����ϴ� �г�
    [SerializeField] Image _sceneChangePanel;       // ȭ����ȯ �г�
    [SerializeField] SettingUI _settingPanel;       // �ɼ� �г�
    [SerializeField] UIExplainUI _explainPanel;     // �ΰ��� ���Խ� ��Ÿ���� ���� �г�


    [SerializeField] DifficultyData[] DifficultyDatas;      // ���̵� ���� ������. 0 = easy, 1 = normal, 2 = hard
    [HideInInspector] public DifficultyData DifficultyData; // �̹��� ���õ� ���̵� ����

    public bool IsGameOver;                                 // ���ӿ����� �Ǿ��°�
    public int TotalKill;                                   // �÷��̾ óġ�� ��� �� ������ ��
    public int TotalSpawn;                                  // �÷��̾ ������ ��� �Ʊ� ���� ������ ��

    const float FADE_DURATION = 0.5f;


    static GameManager _instance;


    private void Awake()
    {

        _sceneChangePanel.gameObject.SetActive(true);   // �ε��� ���� ȭ�� ����ó��
        _settingPanel.OnStart();
        _explainPanel.OnStart();
        
        StartCoroutine(C_SceneChange());


        _instance = this;
        IsGameOver = false;
        DifficultyData = DifficultyDatas[PlayerPrefs.GetInt("Difficulty")];

        // �г� ��Ȱ��ȭ
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

    // ���� ��� �޼���
    public int calcScore()
    {
        return TotalKill + TotalSpawn;
    }

    // ������ ������� �÷��� ��ũ�� ����ϴ� �޼���
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
            color = new Color(0.647f, 0.165f, 0.165f);        // ����
            return "D";
        }
        else if (3000 <= score && score < 4000)
        {
            color = new Color(1.0f, 0.647f, 0.0f);            // ��Ȳ
            return "C";
        }
        else if (4000 <= score && score < 5000)
        {
            color = new Color(1.0f, 0.753f, 0.796f);          // ��ȫ
            return "B";
        }
        else if (5000 <= score && score < 6000)
        {
            color = Color.red;                 
            return "A";
        }
        else
        {
            color = new Color(1.0f, 0.843f, 0.0f);            // �ݻ�
            return "S";
        }

    }

    // Ŭ���� �� ����Ǵ� �޼���
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

    // �й� �� ����Ǵ� �޼���
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

    // Ŭ����, �й� �� ��µǴ� �г��� Ȩ ��ư�� ���� �� ����Ǵ� �޼���
    public void OnClickHomeButton()
    {
        SceneManager.LoadScene("TitleScene");
    }

    // �� ��ȯ �ڷ�ƾ
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
