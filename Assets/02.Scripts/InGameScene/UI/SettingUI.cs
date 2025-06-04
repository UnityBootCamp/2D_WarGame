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

    // ���� ��ư ���� ��
    public void OnSettingButton()
    {
        SoundManager.Instance.ClickUI();
        _soundPanel.gameObject.SetActive(true);
        _openedPanel.Push(_soundPanel);
    }

    // Ȩ ��ư ���� ��
    public void OnHomeButton()
    {
        SoundManager.Instance.ClickUI();
        _homePanel.gameObject.SetActive(true);
        _openedPanel.Push(_homePanel);
    }

    // â �ݱ� ��ư ���� ��
    public void OnExitButton()
    {
        SoundManager.Instance.ClickUI();
        ClosePanel();
    }

    // Ÿ��Ʋ ���ư��� ������ ��
    public void OnHomeConfirmButton()
    {
        SoundManager.Instance.ClickUI();
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
        SoundManager.Instance.ClickUI();
        GameManager.Instance.Restart();
    }
    
    // â �ݱ�
    public void ClosePanel()
    {
        SoundManager.Instance.ClickUI();
        if (_openedPanel.Count == 0)
            return;
        _openedPanel.Pop().gameObject.SetActive(false);
    }
}
