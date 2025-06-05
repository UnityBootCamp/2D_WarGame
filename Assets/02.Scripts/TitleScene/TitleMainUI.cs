using UnityEngine;
using UnityEngine.UI;

public class TitleMainUI : MonoBehaviour
{
    [SerializeField] Button _startGameButton;
    [SerializeField] Button _settingButton;
    [SerializeField] Button _exitButton;

    private void Awake()
    {
        _startGameButton.onClick.AddListener(OnClickStartGameButton);
        _settingButton.onClick.AddListener(OnClickSettingButton);
        _exitButton.onClick.AddListener(OnClickExitButton);
    }


    void OnClickStartGameButton()
    {
        TitleSoundManager.Instance.ClickUI();
        TitleManager.Instance.DifficultyPanel.gameObject.SetActive(true);
    }

    void OnClickSettingButton()
    {
        TitleSoundManager.Instance.ClickUI();
        TitleManager.Instance.TitleSettingPanel.gameObject.SetActive(true);
    }


    void OnClickExitButton()
    {
        TitleSoundManager.Instance.ClickUI();
        TitleManager.Instance.ExitConfirmPanel.gameObject.SetActive(true);
    }
}
