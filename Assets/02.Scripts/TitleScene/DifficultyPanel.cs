using UnityEngine;
using UnityEngine.UI;

public class DifficultyPanel : MonoBehaviour
{
    [SerializeField] Button _difficultyExitButton;
    [SerializeField] Button[] _difficultyButtons;

    private void Awake()
    {
        _difficultyExitButton.onClick.AddListener(OnClickExitButton);

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

        TitleManager.Instance.HowToPlayPanel.gameObject.SetActive(true);

        gameObject.SetActive(false);

    }

    void OnClickExitButton()
    {
        TitleSoundManager.Instance.ClickUI();
        gameObject.SetActive(false);
    }

}
