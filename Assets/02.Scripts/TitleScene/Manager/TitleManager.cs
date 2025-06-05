using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// �̱���
// Ÿ��Ʋ ������ ����Ǵ� ���� �гΰ� �ʱ� ���� ����
public class TitleManager : MonoBehaviour
{
    public DifficultyPanel DifficultyPanel => _difficultyPanel;
    public HowToPlayPanel HowToPlayPanel => _howToPlayPanel;
    public TitleSettingPanel TitleSettingPanel => _titleSettingPanel;
    public ExitConfirmPanel ExitConfirmPanel => _exitConfirmPanel;
    public Image SceneChangePanel => _sceneChangePanel;

    [SerializeField] DifficultyPanel _difficultyPanel;
    [SerializeField] HowToPlayPanel _howToPlayPanel;
    [SerializeField] TitleSettingPanel _titleSettingPanel;
    [SerializeField] ExitConfirmPanel _exitConfirmPanel;
    [SerializeField] Image _sceneChangePanel;

    List<GameObject> _panels;

    float FADE_DURATION = 0.5f;

    #region �̱���
    public static TitleManager Instance => _instance;

    static TitleManager _instance;

    private void Awake()
    {
        _instance = this;

        Init();
        InactiveAllPanels();
    }
    #endregion


    void Init()
    {
        _panels = new List<GameObject>(7);

        _sceneChangePanel.color = new Color(0, 0, 0, 0);
        _sceneChangePanel.gameObject.SetActive(false);

        _panels.Add(_difficultyPanel.gameObject);
        _panels.Add(_howToPlayPanel.gameObject);
        _panels.Add(_titleSettingPanel.gameObject);
        _panels.Add(_sceneChangePanel.gameObject);
        _panels.Add(_exitConfirmPanel.gameObject);

        _titleSettingPanel.Init();
    }

    void InactiveAllPanels()
    {
        foreach (GameObject panel in _panels)
        {
            panel.SetActive(false);
        }
    }

    public void SceneChange()
    {
        StartCoroutine(C_SceneChange());
    }

    // �� ��ȯ �ڷ�ƾ
    IEnumerator C_SceneChange()
    {
        _sceneChangePanel.gameObject.SetActive(true);

        float cumulativeTime = 0f;
        while (cumulativeTime < FADE_DURATION)
        {
            cumulativeTime += Time.deltaTime;
            _sceneChangePanel.color = Color.Lerp(new Color(0, 0, 0, 0), Color.black, cumulativeTime / FADE_DURATION);  // ���� -> �������� ��ȯ
            yield return null;
        }

        SceneManager.LoadScene("InGameScene");
    }
}