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
    // �ʱ�ȭ
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
