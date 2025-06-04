using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

// �̱���
// ���� ��ü���� ���Ǵ� ����ȿ���� �����Ѵ�.
public class SoundManager : MonoBehaviour
{
    // Fields
    // ����� �ҽ�
    [SerializeField] AudioSource _bgm;
    [SerializeField] AudioSource _sfx;
    [SerializeField] AudioSource _enemySfx;
    [SerializeField] AudioSource _resourceSfx;
    [SerializeField] AudioSource _uiSfx;

    // ����� Ŭ��
    [SerializeField] AudioClip _attack;
    [SerializeField] AudioClip _shotArrow;
    [SerializeField] AudioClip _summonMeteor;
    [SerializeField] AudioClip _getResource;
    [SerializeField] AudioClip _uiClick;
    [SerializeField] AudioClip _pickaxeBlow;

    // ���� ���� �����̴�
    [SerializeField] Slider _bgmSlider;
    [SerializeField] Slider _sfxSlider;
    [SerializeField] Slider _resourceSfxSlider;
    [SerializeField] Slider _uiSfxSlider;

    // ����� �ͼ�
    [SerializeField] AudioMixer _audioMixer;


    #region �̱���
    static SoundManager _instance;
    public static SoundManager Instance => _instance;

    private void Awake()
    {
        _instance = this;

        // �����̴� �� ���� ��, �ͼ��� ���� ����� �� �ֵ��� �̺�Ʈ ���
        _bgmSlider.onValueChanged.AddListener(SetBGMVolume);
        _sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        _resourceSfxSlider.onValueChanged.AddListener(SetResourceSFXVolume);
        _uiSfxSlider.onValueChanged.AddListener(SetUISFXVolume);

        // ������ �����صξ��� ����� ������ ������
        _bgmSlider.value = PlayerPrefs.GetFloat("BGMVolume");
        _sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume");
        _resourceSfxSlider.value = PlayerPrefs.GetFloat("ResourceSFXVolume");
        _uiSfxSlider.value = PlayerPrefs.GetFloat("UISFXVolume");
    }
    #endregion


    // Methods
    // �ͼ��� ���� ���� �����ϰ�, ����� ���� PlayerPrefs�� ���
    void SetBGMVolume(float value)
    {
        // 0~1 �������� ���ú��� ��ȯ (-80 ~ 0)
        float volume = Mathf.Log10(Mathf.Max(value, 0.0001f)) * 20;
        _audioMixer.SetFloat("BGMVolume", volume);
        PlayerPrefs.SetFloat("BGMVolume", value);
    }

    void SetSFXVolume(float value)
    {
        // 0~1 �������� ���ú��� ��ȯ (-80 ~ 0)
        float volume = Mathf.Log10(Mathf.Max(value, 0.0001f)) * 20;
        _audioMixer.SetFloat("SFXVolume", volume);
        PlayerPrefs.SetFloat("SFXVolume", value);
    }

    void SetResourceSFXVolume(float value)
    {
        // 0~1 �������� ���ú��� ��ȯ (-80 ~ 0)
        float volume = Mathf.Log10(Mathf.Max(value, 0.0001f)) * 20;
        _audioMixer.SetFloat("ResourceSFXVolume", volume);
        PlayerPrefs.SetFloat("ResourceSFXVolume", value);
    }

    void SetUISFXVolume(float value)
    {
        // 0~1 �������� ���ú��� ��ȯ (-80 ~ 0)
        float volume = Mathf.Log10(Mathf.Max(value, 0.0001f)) * 20;
        _audioMixer.SetFloat("UISFXVolume", volume);
        PlayerPrefs.SetFloat("UISFXVolume", value);
    }

    // ��Ȳ�� �°� ����� �� �ִ� ȿ���� ��� �޼��� ����
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
