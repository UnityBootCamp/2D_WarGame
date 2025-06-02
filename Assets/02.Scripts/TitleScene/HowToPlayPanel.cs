using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class HowToPlayPanel : MonoBehaviour
{
    // 버튼
    [SerializeField] Button _leftArrowButton;  // 왼쪽 버튼
    [SerializeField] Button _rightArrowButton; // 오른쪽 버튼
    [SerializeField] Button _playButton;       // 플레이버튼
    [SerializeField] Button _exitButton;       // 플레이버튼


    // 설명 패널
    [SerializeField] Image[] _pagePanel;       // 플레이 설명 패널 리스트
    int _pageIndex;                            // 페이지 인덱스


    // 씬 전환
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


        _leftArrowButton.gameObject.SetActive(false);      // 1페이지부터 시작하므로, 최초 왼쪽 화살표는 보이지 않도록 설정

        _pagePanel[0].gameObject.SetActive(true);          // 1페이지 활성화
        
        for (int i =1; i< _pagePanel.Length; i++)
        {
            _pagePanel[i].gameObject.SetActive(false);     // 1페이지 패널 이외의 패널들은 비활성화
        }
    }


    // 오른쪽 버튼 클릭 시
    public void OnClickRightButton()
    {
        // 오른쪽 끝 페이지라면 return
        if (_pageIndex >= _pagePanel.Length-1)
            return;

        // 오른쪽 버튼을 눌렀을 때, 왼쪽 버튼이 비활성화 상태였다면(1페이지 였다면) 왼쪽 버튼 활성화
        if(_leftArrowButton.gameObject.activeSelf == false)
        {
            _leftArrowButton.gameObject.SetActive(true);
        }


        _pagePanel[_pageIndex].gameObject.SetActive(false); // 이전 페이지 비활성화
        _pageIndex += 1;
        _pagePanel[_pageIndex].gameObject.SetActive(true);  // 현재 페이지 활성화

        
        // 오른쪽으로 이동한 이번 페이지가 오른쪽 끝 페이지라면 오른쪽 버튼 비활성화
        if(_pageIndex == _pagePanel.Length-1)
        {
            _rightArrowButton.gameObject.SetActive(false);
        }

    }

    // 왼쪽 버튼 클릭시
    public void OnClickLeftButton()
    {
        // 왼쪽 끝 페이지라면 return
        if (_pageIndex <= 0)
            return;

        // 왼쪽 버튼을 눌렀을 때, 오른쪽 버튼이 비활성화 상태였다면(1페이지 였다면) 오른쪽 버튼 활성화
        if (_rightArrowButton.gameObject.activeSelf == false)
        {
            _rightArrowButton.gameObject.SetActive(true);
        }

        _pagePanel[_pageIndex].gameObject.SetActive(false); // 이전 페이지 비활성화
        _pageIndex -= 1;
        _pagePanel[_pageIndex].gameObject.SetActive(true);  // 현재 페이지 활성화


        // 왼쪽으로 이동한 이번 페이지가 오른쪽 끝 페이지라면 왼쪽 버튼 비활성화
        if (_pageIndex == 0)
        {
            _leftArrowButton.gameObject.SetActive(false);
        }

    }

    // play 버튼 눌렀을 때
    public void OnClickPlayButton()
    {
        StartCoroutine(C_SceneChange());
        
    }

    // x 버튼 눌렀을 때
    void OnClickExitButton()
    {
        gameObject.SetActive(false);
    }

    // 씬 변환 코루틴
    IEnumerator C_SceneChange()
    {
        _sceneChangePanel.gameObject.SetActive(true);
        float cumulativeTime = 0f;
        while (cumulativeTime < FADE_DURATION)
        {
            cumulativeTime += Time.deltaTime;
            _sceneChangePanel.color = Color.Lerp(new Color(0, 0, 0, 0), Color.black,  cumulativeTime / FADE_DURATION);  // 투명 -> 검정으로 변환
            yield return null;
        }

        gameObject.SetActive(false);
        SceneManager.LoadScene("InGameScene");
    }
}
