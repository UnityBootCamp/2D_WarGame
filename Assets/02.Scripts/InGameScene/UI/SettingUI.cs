using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingUI : MonoBehaviour
{
    // Properties
    public Stack<GameObject> OppendPanel => _openedPanel;


    // Fields
    // Panels
    [SerializeField] GameObject _soundPanel;
    [SerializeField] GameObject _homePanel;
    Stack<GameObject> _openedPanel;

    // Buttons
    [SerializeField] Button _restartButton;
    [SerializeField] Button _settingButton;
    [SerializeField] Button _homeButton;
    [SerializeField] Button _homeConfirmButton;
    [SerializeField] Button _homeExitButton;
    [SerializeField] Button _soundExitButton;


    // Methods
    // 초기화
    public void Init()
    {
        _soundPanel.gameObject.SetActive(false);
        _homePanel.gameObject.SetActive(false);
        _openedPanel = new Stack<GameObject>(4);

        _restartButton.onClick.AddListener(OnRestartButton);
        _settingButton.onClick.AddListener(OnSettingButton);
        _homeButton.onClick.AddListener(OnHomeButton);
        _homeConfirmButton.onClick.AddListener(OnHomeConfirmButton);
        _homeExitButton.onClick.AddListener(OnExitButton);
        _soundExitButton.onClick.AddListener(OnExitButton);
    }

    // 세팅 버튼 누를 시
    public void OnSettingButton()
    {
        SoundManager.Instance.ClickUI();
        _soundPanel.gameObject.SetActive(true);
        _openedPanel.Push(_soundPanel);
    }

    // 홈 버튼 누를 시
    public void OnHomeButton()
    {
        SoundManager.Instance.ClickUI();
        _homePanel.gameObject.SetActive(true);
        _openedPanel.Push(_homePanel);
    }

    // 창 닫기 버튼 누를 시
    public void OnExitButton()
    {
        SoundManager.Instance.ClickUI();
        ClosePanel();
    }

    // 타이틀 돌아가기 동의할 시
    public void OnHomeConfirmButton()
    {
        SoundManager.Instance.ClickUI();
        // 모든 패널 닫고
        for (int i = 0; i < _openedPanel.Count; i++)
        {
            ClosePanel();
        }
        GameManager.Instance.Restart();

        SceneManager.LoadScene("TitleScene"); // 타이틀 씬 이동
    }

    // 다시 시작 누를 시
    public void OnRestartButton()
    {
        SoundManager.Instance.ClickUI();
        GameManager.Instance.Restart();
    }
    
    // 창 닫기
    public void ClosePanel()
    {
        SoundManager.Instance.ClickUI();
        if (_openedPanel.Count == 0)
            return;
        _openedPanel.Pop().gameObject.SetActive(false);
    }
}
