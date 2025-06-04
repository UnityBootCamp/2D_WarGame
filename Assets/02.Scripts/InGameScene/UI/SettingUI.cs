using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SettingUI : MonoBehaviour
{
    // Properties
    public Stack<GameObject> OppendPanel => _openedPanel;


    // Fields
    [SerializeField] GameObject _soundPanel;
    [SerializeField] GameObject _homePanel;
    Stack<GameObject> _openedPanel;

 
    // Methods
    public void OnStart()
    {
        SoundManager.Instance.ClickUI();
        _soundPanel.gameObject.SetActive(false);
        _homePanel.gameObject.SetActive(false);
        _openedPanel = new Stack<GameObject>(4);
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
