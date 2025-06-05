using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


public class TitleSettingPanel : MonoBehaviour
{
    [SerializeField] AudioMixer _audioMixer;
    [SerializeField] Slider _bgmSlider;
    [SerializeField] Slider _sfxSlider;
    [SerializeField] Slider _resourceSfxSlider;
    [SerializeField] Slider _uiSfxSlider;
    [SerializeField] Button _settingExitButton;


    // 최초, PlayerPrefs에 키가 없는 값들을 등록해 줌.
    public void Init()
    {
        _settingExitButton.onClick.AddListener(OnClickExitButton);

        // Slider 의 값 변경에 리스너 등록
        _bgmSlider.onValueChanged.AddListener(SetBGMVolume);
        _sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        _resourceSfxSlider.onValueChanged.AddListener(SetResourceSFXVolume);
        _uiSfxSlider.onValueChanged.AddListener(SetUISFXVolume);

        
        // 초기값 설정
        if (!PlayerPrefs.HasKey("BGMVolume"))
        {
            PlayerPrefs.SetFloat("BGMVolume", 1);
        }
        if (!PlayerPrefs.HasKey("SFXVolume"))
        {
            PlayerPrefs.SetFloat("SFXVolume", 1);
        }
        if (!PlayerPrefs.HasKey("ResourceSFXVolume"))
        {
            PlayerPrefs.SetFloat("ResourceSFXVolume", 1);
        }
        if (!PlayerPrefs.HasKey("UISFXVolume"))
        {
            PlayerPrefs.SetFloat("UISFXVolume", 1);
        }

        // 슬라이더에 초기 값 반영
        _bgmSlider.value = PlayerPrefs.GetFloat("BGMVolume");
        _sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume");
        _resourceSfxSlider.value = PlayerPrefs.GetFloat("ReourceSFXVolume");
        _uiSfxSlider.value = PlayerPrefs.GetFloat("UISFXVolume");

        // 볼륨믹서에 초기 값 반영
        SetBGMVolume(_bgmSlider.value);
        SetSFXVolume(_sfxSlider.value);
        SetResourceSFXVolume(_resourceSfxSlider.value);
        SetUISFXVolume(_uiSfxSlider.value);
    }

    // 믹서의 볼륨 값을 변경하고, 변경된 값은 PlayerPrefs에 기록
    public void SetBGMVolume(float value)
    {
        float volume = Mathf.Log10(Mathf.Max(value, 0.0001f)) * 20;
        _audioMixer.SetFloat("BGMVolume", volume);
        PlayerPrefs.SetFloat("BGMVolume", value);
    }

    public void SetSFXVolume(float value)
    {
        float volume = Mathf.Log10(Mathf.Max(value, 0.0001f)) * 20;
        _audioMixer.SetFloat("SFXVolume", volume);
        PlayerPrefs.SetFloat("SFXVolume", value);
    }

    public void SetResourceSFXVolume(float value)
    {
        float volume = Mathf.Log10(Mathf.Max(value, 0.0001f)) * 20;
        _audioMixer.SetFloat("ResourceSFXVolume", volume);
        PlayerPrefs.SetFloat("ResourceSFXVolume", value);
    }

    public void SetUISFXVolume(float value)
    {
        float volume = Mathf.Log10(Mathf.Max(value, 0.0001f)) * 20;
        _audioMixer.SetFloat("UISFXVolume", volume);
        PlayerPrefs.SetFloat("UISFXVolume", value);
    }

    // 창 닫기 버튼
    void OnClickExitButton()
    {
        TitleSoundManager.Instance.ClickUI();
        TitleManager.Instance.TitleSettingPanel.gameObject.SetActive(false);
    }
}