using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject _clearPanel;
    [SerializeField] GameObject _youLosePanel;



    public bool IsGameOver;
    public int TotalKill;
    public int TotalSpawn;


    static GameManager _instance;
    public static GameManager Instance => _instance;

    private void Awake()
    {
        _instance = this;
        IsGameOver = false;

        _clearPanel.SetActive(false);
        _youLosePanel.SetActive(false);
    }

    public int calcScore()
    {
        return TotalKill + TotalSpawn;
    }

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
            color = new Color(1.0f, 0.753f, 0.796f);                                // 분홍
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

    public void OnClickHomeButton()
    {
        SceneManager.LoadScene("TitleScene");
    }
}
