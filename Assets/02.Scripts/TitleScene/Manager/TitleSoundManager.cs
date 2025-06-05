using UnityEngine;
public class TitleSoundManager : MonoBehaviour
{
    [SerializeField] AudioSource _uiSfx;
    [SerializeField] AudioClip _uiClick;


    static TitleSoundManager _instance;
    public static TitleSoundManager Instance => _instance;
    void Awake()
    {
        _instance = this;
    }


    // 상황에 맞게 사용할 수 있는 효과음 재생 메서드
    public void ClickUI()
    {
        _uiSfx.PlayOneShot(_uiClick);
    }
}
