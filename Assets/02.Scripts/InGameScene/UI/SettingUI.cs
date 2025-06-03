using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SettingUI : MonoBehaviour
{
    public Stack<GameObject> OppendPanel => _openedPanel;

    Stack<GameObject> _openedPanel;

    [SerializeField] GameObject _soundPanel;
    [SerializeField] GameObject _homePanel;

 

    public void OnStart()
    {
        _soundPanel.gameObject.SetActive(false);
        _homePanel.gameObject.SetActive(false);
        _openedPanel = new Stack<GameObject>(4);
    }

    // 세팅 버튼 누를 시
    public void OnSettingButton()
    {
        _soundPanel.gameObject.SetActive(true);
        _openedPanel.Push(_soundPanel);
    }

    // 홈 버튼 누를 시
    public void OnHomeButton()
    {
        _homePanel.gameObject.SetActive(true);
        _openedPanel.Push(_homePanel);
    }

    // 창 닫기 버튼 누를 시
    public void OnExitButton()
    {
        ClosePanel();
    }

    // 타이틀 돌아가기 동의할 시
    
    public void OnHomeConfirmButton()
    {
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
        GameManager.Instance.Restart();
    }

    
    // 창 닫기
    public void ClosePanel()
    {
        if (_openedPanel.Count == 0)
            return;
        _openedPanel.Pop().gameObject.SetActive(false);
    }
}
