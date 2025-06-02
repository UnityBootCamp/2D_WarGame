using UnityEngine;
using UnityEngine.UI;

public class TitleMain : MonoBehaviour
{
    [SerializeField] Button _startGameButton;
    [SerializeField] Button _exitButton;
    [SerializeField] Image _difficultyPanel;

    private void Awake()
    {
        _startGameButton.onClick.AddListener(OnClickStartGameButton);
        _exitButton.onClick.AddListener(OnClickExitButton);
    }


    void OnClickStartGameButton()
    {
        _difficultyPanel.gameObject.SetActive(true);
    }

    void OnClickExitButton()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
