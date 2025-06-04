using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System;

public class TitleSoundManager : MonoBehaviour
{
    [SerializeField] AudioMixer _audioMixer;
    [SerializeField] Slider _bgmSlider;
    [SerializeField] Slider _sfxSlider;
    [SerializeField] Slider _resourceSfxSlider;
    [SerializeField] Slider _uiSfxSlider;

    [SerializeField] AudioSource _uiSfx;

    [SerializeField] AudioClip _uiClick;


    static TitleSoundManager _instance;
    public static TitleSoundManager Instance => _instance;
    
    void Awake()
    {
        _instance = this;
        _bgmSlider.onValueChanged.AddListener(SetBGMVolume);
        _sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        _resourceSfxSlider.onValueChanged.AddListener(SetResourceSFXVolume);
        _uiSfxSlider.onValueChanged.AddListener(SetUISFXVolume);

        InitialSoundSetting();

        _bgmSlider.value = PlayerPrefs.GetFloat("BGMVolume");
        _sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume");
        _resourceSfxSlider.value = PlayerPrefs.GetFloat("ReourceSFXVolume");
        _uiSfxSlider.value = PlayerPrefs.GetFloat("UISFXVolume");
    }

    // ����, PlayerPrefs�� Ű�� ���� ������ ����� ��.
    void InitialSoundSetting()
    {
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

    // ��Ȳ�� �°� ����� �� �ִ� ȿ���� ��� �޼���
    public void ClickUI()
    {
        _uiSfx.PlayOneShot(_uiClick);
    }


}
