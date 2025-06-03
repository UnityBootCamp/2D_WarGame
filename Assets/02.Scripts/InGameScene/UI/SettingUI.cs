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

    // ���� ��ư ���� ��
    public void OnSettingButton()
    {
        _soundPanel.gameObject.SetActive(true);
        _openedPanel.Push(_soundPanel);
    }

    // Ȩ ��ư ���� ��
    public void OnHomeButton()
    {
        _homePanel.gameObject.SetActive(true);
        _openedPanel.Push(_homePanel);
    }

    // â �ݱ� ��ư ���� ��
    public void OnExitButton()
    {
        ClosePanel();
    }

    // Ÿ��Ʋ ���ư��� ������ ��
    
    public void OnHomeConfirmButton()
    {
        // ��� �г� �ݰ�
        for (int i = 0; i < _openedPanel.Count; i++)
        {
            ClosePanel();
        }
        GameManager.Instance.Restart();


        SceneManager.LoadScene("TitleScene"); // Ÿ��Ʋ �� �̵�
    }

    // �ٽ� ���� ���� ��

    public void OnRestartButton()
    {
        GameManager.Instance.Restart();
    }

    
    // â �ݱ�
    public void ClosePanel()
    {
        if (_openedPanel.Count == 0)
            return;
        _openedPanel.Pop().gameObject.SetActive(false);
    }
}
