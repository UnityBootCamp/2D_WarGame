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


    // ��Ȳ�� �°� ����� �� �ִ� ȿ���� ��� �޼���
    public void ClickUI()
    {
        _uiSfx.PlayOneShot(_uiClick);
    }
}
