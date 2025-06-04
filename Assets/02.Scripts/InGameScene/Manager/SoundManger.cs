using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

// 싱글톤
// 게임 전체에서 사용되는 음향효과를 관리한다.
public class SoundManager : MonoBehaviour
{
    // Fields
    // 오디오 소스
    [SerializeField] AudioSource _bgm;
    [SerializeField] AudioSource _sfx;
    [SerializeField] AudioSource _enemySfx;
    [SerializeField] AudioSource _resourceSfx;
    [SerializeField] AudioSource _uiSfx;

    // 오디오 클립
    [SerializeField] AudioClip _attack;
    [SerializeField] AudioClip _shotArrow;
    [SerializeField] AudioClip _summonMeteor;
    [SerializeField] AudioClip _getResource;
    [SerializeField] AudioClip _uiClick;
    [SerializeField] AudioClip _pickaxeBlow;

    // 사운드 조절 슬라이더
    [SerializeField] Slider _bgmSlider;
    [SerializeField] Slider _sfxSlider;
    [SerializeField] Slider _resourceSfxSlider;
    [SerializeField] Slider _uiSfxSlider;

    // 오디오 믹서
    [SerializeField] AudioMixer _audioMixer;


    #region 싱글톤
    static SoundManager _instance;
    public static SoundManager Instance => _instance;

    private void Awake()
    {
        _instance = this;

        // 슬라이더 값 변경 시, 믹서의 값이 변경될 수 있도록 이벤트 등록
        _bgmSlider.onValueChanged.AddListener(SetBGMVolume);
        _sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        _resourceSfxSlider.onValueChanged.AddListener(SetResourceSFXVolume);
        _uiSfxSlider.onValueChanged.AddListener(SetUISFXVolume);

        // 이전에 조절해두었던 오디오 세팅을 가져옴
        _bgmSlider.value = PlayerPrefs.GetFloat("BGMVolume");
        _sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume");
        _resourceSfxSlider.value = PlayerPrefs.GetFloat("ResourceSFXVolume");
        _uiSfxSlider.value = PlayerPrefs.GetFloat("UISFXVolume");
    }
    #endregion


    // Methods
    // 믹서의 볼륨 값을 변경하고, 변경된 값은 PlayerPrefs에 기록
    void SetBGMVolume(float value)
    {
        // 0~1 스케일을 데시벨로 변환 (-80 ~ 0)
        float volume = Mathf.Log10(Mathf.Max(value, 0.0001f)) * 20;
        _audioMixer.SetFloat("BGMVolume", volume);
        PlayerPrefs.SetFloat("BGMVolume", value);
    }

    void SetSFXVolume(float value)
    {
        // 0~1 스케일을 데시벨로 변환 (-80 ~ 0)
        float volume = Mathf.Log10(Mathf.Max(value, 0.0001f)) * 20;
        _audioMixer.SetFloat("SFXVolume", volume);
        PlayerPrefs.SetFloat("SFXVolume", value);
    }

    void SetResourceSFXVolume(float value)
    {
        // 0~1 스케일을 데시벨로 변환 (-80 ~ 0)
        float volume = Mathf.Log10(Mathf.Max(value, 0.0001f)) * 20;
        _audioMixer.SetFloat("ResourceSFXVolume", volume);
        PlayerPrefs.SetFloat("ResourceSFXVolume", value);
    }

    void SetUISFXVolume(float value)
    {
        // 0~1 스케일을 데시벨로 변환 (-80 ~ 0)
        float volume = Mathf.Log10(Mathf.Max(value, 0.0001f)) * 20;
        _audioMixer.SetFloat("UISFXVolume", volume);
        PlayerPrefs.SetFloat("UISFXVolume", value);
    }

    // 상황에 맞게 사용할 수 있는 효과음 재생 메서드 모음
    public void ClickUI()
    {
        _uiSfx.PlayOneShot(_uiClick);
    }

    public void PlayerUnitShotArrow()
    {
        _sfx.PlayOneShot(_shotArrow);
    }

    public void EnemyUnitShotArrow()
    {
        _enemySfx.PlayOneShot(_shotArrow);
    }

    public void PlayerUnitAttack()
    {
        _sfx.PlayOneShot(_attack);
    }

    public void EnemyUnitAttack()
    {
        _enemySfx.PlayOneShot(_attack);
    }


    public void SummonMeteor()
    {
        _sfx.PlayOneShot(_summonMeteor);
    }

    public void GetResource()
    {
        _resourceSfx.PlayOneShot(_getResource);
    }

    public void FarmingResource()
    {
        _resourceSfx.PlayOneShot(_pickaxeBlow);
    }



}
