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


    // ����, PlayerPrefs�� Ű�� ���� ������ ����� ��.
    public void Init()
    {
        _settingExitButton.onClick.AddListener(OnClickExitButton);

        // Slider �� �� ���濡 ������ ���
        _bgmSlider.onValueChanged.AddListener(SetBGMVolume);
        _sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        _resourceSfxSlider.onValueChanged.AddListener(SetResourceSFXVolume);
        _uiSfxSlider.onValueChanged.AddListener(SetUISFXVolume);

        
        // �ʱⰪ ����
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

        // �����̴��� �ʱ� �� �ݿ�
        _bgmSlider.value = PlayerPrefs.GetFloat("BGMVolume");
        _sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume");
        _resourceSfxSlider.value = PlayerPrefs.GetFloat("ReourceSFXVolume");
        _uiSfxSlider.value = PlayerPrefs.GetFloat("UISFXVolume");

        // �����ͼ��� �ʱ� �� �ݿ�
        SetBGMVolume(_bgmSlider.value);
        SetSFXVolume(_sfxSlider.value);
        SetResourceSFXVolume(_resourceSfxSlider.value);
        SetUISFXVolume(_uiSfxSlider.value);
    }

    // �ͼ��� ���� ���� �����ϰ�, ����� ���� PlayerPrefs�� ���
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

    // â �ݱ� ��ư
    void OnClickExitButton()
    {
        TitleSoundManager.Instance.ClickUI();
        TitleManager.Instance.TitleSettingPanel.gameObject.SetActive(false);
    }
}