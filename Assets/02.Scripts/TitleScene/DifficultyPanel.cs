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
            int index = i;          // 값을 할당해 주지 않고, 람다에 바로 사용하게되면 , i를 참조하는 방식으로 동작하여 모든 람다식의 파라미터가 i의 마지막 값
                                    // 즉 _difficultyButtons.Length를 갖게 된다.
                                    // int는 값 타입이지만, **람다(또는 익명 메서드)는 값 타입이라도
                                    // 내부적으로 "캡처"를 하면 그 값을 힙(Heap) 에 올려서 참조형처럼 관리합니다. ( Gpt)

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
