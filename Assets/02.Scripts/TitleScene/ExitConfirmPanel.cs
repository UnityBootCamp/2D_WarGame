using UnityEngine;
using UnityEngine.UI;

public class ExitConfirmPanel : MonoBehaviour
{
    [SerializeField] Button _yesButton;
    [SerializeField] Button _noButton;

    private void Awake()
    {
        _yesButton.onClick.AddListener(OnClickYesButton);
        _noButton.onClick.AddListener(OnClickNoButton);
    }


    void OnClickYesButton()
    {
        TitleSoundManager.Instance.ClickUI();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

    void OnClickNoButton()
    {
        TitleSoundManager.Instance.ClickUI();
        gameObject.SetActive(false);
    }
}
