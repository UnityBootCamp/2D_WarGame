using UnityEngine;
using UnityEngine.UI;

public class HowToPlayPanel : MonoBehaviour
{
    // ��ư
    [SerializeField] Button _leftArrowButton;  // ���� ��ư
    [SerializeField] Button _rightArrowButton; // ������ ��ư
    [SerializeField] Button _playButton;       // �÷��̹�ư
    [SerializeField] Button _exitButton;       // �÷��̹�ư


    // ���� �г�
    [SerializeField] Image[] _pagePanel;       // �÷��� ���� �г� ����Ʈ
    int _pageIndex;                            // ������ �ε���


    private void Awake()
    {
    }

    void Start()
    {
        Init();
    }


    void Init()
    {
        _leftArrowButton.onClick.AddListener(OnClickLeftButton);
        _rightArrowButton.onClick.AddListener(OnClickRightButton);
        _playButton.onClick.AddListener(OnClickPlayButton);
        _exitButton.onClick.AddListener(OnClickExitButton);

        _leftArrowButton.gameObject.SetActive(false);      // 1���������� �����ϹǷ�, ���� ���� ȭ��ǥ�� ������ �ʵ��� ����
        _pagePanel[0].gameObject.SetActive(true);          // 1������ Ȱ��ȭ

        for (int i = 1; i < _pagePanel.Length; i++)
        {
            _pagePanel[i].gameObject.SetActive(false);     // 1������ �г� �̿��� �гε��� ��Ȱ��ȭ
        }
    }

    // ������ ��ư Ŭ�� ��
    public void OnClickRightButton()
    {
        TitleSoundManager.Instance.ClickUI();

        // ������ �� ��������� return
        if (_pageIndex >= _pagePanel.Length-1)
            return;

        // ������ ��ư�� ������ ��, ���� ��ư�� ��Ȱ��ȭ ���¿��ٸ�(1������ ���ٸ�) ���� ��ư Ȱ��ȭ
        if(_leftArrowButton.gameObject.activeSelf == false)
        {
            _leftArrowButton.gameObject.SetActive(true);
        }


        _pagePanel[_pageIndex].gameObject.SetActive(false); // ���� ������ ��Ȱ��ȭ
        _pageIndex += 1;
        _pagePanel[_pageIndex].gameObject.SetActive(true);  // ���� ������ Ȱ��ȭ

        
        // ���������� �̵��� �̹� �������� ������ �� ��������� ������ ��ư ��Ȱ��ȭ
        if(_pageIndex == _pagePanel.Length-1)
        {
            _rightArrowButton.gameObject.SetActive(false);
        }

    }

    // ���� ��ư Ŭ����
    public void OnClickLeftButton()
    {
        TitleSoundManager.Instance.ClickUI();
        // ���� �� ��������� return
        if (_pageIndex <= 0)
            return;

        // ���� ��ư�� ������ ��, ������ ��ư�� ��Ȱ��ȭ ���¿��ٸ�(1������ ���ٸ�) ������ ��ư Ȱ��ȭ
        if (_rightArrowButton.gameObject.activeSelf == false)
        {
            _rightArrowButton.gameObject.SetActive(true);
        }

        _pagePanel[_pageIndex].gameObject.SetActive(false); // ���� ������ ��Ȱ��ȭ
        _pageIndex -= 1;
        _pagePanel[_pageIndex].gameObject.SetActive(true);  // ���� ������ Ȱ��ȭ


        // �������� �̵��� �̹� �������� ������ �� ��������� ���� ��ư ��Ȱ��ȭ
        if (_pageIndex == 0)
        {
            _leftArrowButton.gameObject.SetActive(false);
        }

    }

    // play ��ư ������ ��
    public void OnClickPlayButton()
    {
        TitleSoundManager.Instance.ClickUI();
        TitleManager.Instance.SceneChange();
    }

    // x ��ư ������ ��
    void OnClickExitButton()
    {
        TitleSoundManager.Instance.ClickUI();
        gameObject.SetActive(false);
    }

    
}
