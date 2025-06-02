using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using TMPro;

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


    // �� ��ȯ
    [SerializeField] Image _sceneChangePanel;
    float FADE_DURATION = 0.5f;

    private void Awake()
    {
        _sceneChangePanel.color = new Color(0, 0, 0, 0);
        gameObject.SetActive(false);
        _sceneChangePanel.gameObject.SetActive(false);
    }

    void Start()
    {

        _leftArrowButton.onClick.AddListener(OnClickLeftButton);
        _rightArrowButton.onClick.AddListener(OnClickRightButton);
        _playButton.onClick.AddListener(OnClickPlayButton);
        _exitButton.onClick.AddListener(OnClickExitButton);


        _leftArrowButton.gameObject.SetActive(false);      // 1���������� �����ϹǷ�, ���� ���� ȭ��ǥ�� ������ �ʵ��� ����

        _pagePanel[0].gameObject.SetActive(true);          // 1������ Ȱ��ȭ
        
        for (int i =1; i< _pagePanel.Length; i++)
        {
            _pagePanel[i].gameObject.SetActive(false);     // 1������ �г� �̿��� �гε��� ��Ȱ��ȭ
        }
    }


    // ������ ��ư Ŭ�� ��
    public void OnClickRightButton()
    {
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
        StartCoroutine(C_SceneChange());
        
    }

    // x ��ư ������ ��
    void OnClickExitButton()
    {
        gameObject.SetActive(false);
    }

    // �� ��ȯ �ڷ�ƾ
    IEnumerator C_SceneChange()
    {
        _sceneChangePanel.gameObject.SetActive(true);
        float cumulativeTime = 0f;
        while (cumulativeTime < FADE_DURATION)
        {
            cumulativeTime += Time.deltaTime;
            _sceneChangePanel.color = Color.Lerp(new Color(0, 0, 0, 0), Color.black,  cumulativeTime / FADE_DURATION);  // ���� -> �������� ��ȯ
            yield return null;
        }

        gameObject.SetActive(false);
        SceneManager.LoadScene("InGameScene");
    }
}
