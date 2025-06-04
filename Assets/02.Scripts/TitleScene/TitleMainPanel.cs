using UnityEngine;
using UnityEngine.UI;

public class TitleMain : MonoBehaviour
{
    [SerializeField] Button _startGameButton;
    [SerializeField] Button _settingButton;
    [SerializeField] Button _settingXButton;
    [SerializeField] Button _exitButton;
    [SerializeField] Image _settingPanel;
    [SerializeField] Image _difficultyPanel;

    private void Awake()
    {
        _difficultyPanel.gameObject.SetActive(false);
        _startGameButton.onClick.AddListener(OnClickStartGameButton);
        _settingButton.onClick.AddListener(OnClickSettingButton);
        _settingXButton.onClick.AddListener(OnClickSettingXButton);
        _exitButton.onClick.AddListener(OnClickExitButton);
    }


    void OnClickStartGameButton()
    {
        TitleSoundManager.Instance.ClickUI();
        _difficultyPanel.gameObject.SetActive(true);
    }

    void OnClickSettingButton()
    {
        TitleSoundManager.Instance.ClickUI();
        _settingPanel.gameObject.SetActive(true);
    }

    void OnClickSettingXButton()
    {
        TitleSoundManager.Instance.ClickUI();
        _settingPanel.gameObject.SetActive(false);
    }

    void OnClickExitButton()
    {
        TitleSoundManager.Instance.ClickUI();

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}
