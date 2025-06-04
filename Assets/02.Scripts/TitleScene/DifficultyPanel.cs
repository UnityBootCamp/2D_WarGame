using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DifficultyPanel : MonoBehaviour
{
    [SerializeField] Button _exitButton;
    [SerializeField] Button[] _difficultyButtons;
    [SerializeField] Image _howToPlayPanel;

    private void Awake()
    {
        _howToPlayPanel.gameObject.SetActive(false);
        _exitButton.onClick.AddListener(OnClickExitButton);

        for(int i = 0; i<_difficultyButtons.Length; i++)
        {
            int index = i;          // ���� �Ҵ��� ���� �ʰ�, ���ٿ� �ٷ� ����ϰԵǸ� , i�� �����ϴ� ������� �����Ͽ� ��� ���ٽ��� �Ķ���Ͱ� i�� ������ ��
                                    // �� _difficultyButtons.Length�� ���� �ȴ�.
                                    // int�� �� Ÿ��������, **����(�Ǵ� �͸� �޼���)�� �� Ÿ���̶�
                                    // ���������� "ĸó"�� �ϸ� �� ���� ��(Heap) �� �÷��� ������ó�� �����մϴ�. ( Gpt)

            _difficultyButtons[i].onClick.AddListener(() => OnClickDifficultyButton(index));
        }

    }

    void OnClickDifficultyButton(int index)
    {
        TitleSoundManager.Instance.ClickUI();
        PlayerPrefs.SetInt("Difficulty", index);
        Debug.Log(PlayerPrefs.GetInt("Difficulty"));

        gameObject.SetActive(false);
        _howToPlayPanel.gameObject.SetActive(true);

    }

    void OnClickExitButton()
    {
        TitleSoundManager.Instance.ClickUI();
        gameObject.SetActive(false);
    }

}
