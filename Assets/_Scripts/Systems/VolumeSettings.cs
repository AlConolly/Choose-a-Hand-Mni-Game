using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class VolumeSettings : Menu<VolumeSettings>
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Image MuteImage;
    [SerializeField] private Sprite unmutedSprite;
    [SerializeField] private Sprite mutedSprite;

    // * These values correspond to the exposed values in the audioMixer
    const string SFX_VALUE = "sfxVolume";
    const string MUSIC_VALUE = "musicVolume";
    const string MASTER_VALUE = "masterVolume";
    const string MUTED_VALUE = "muted";

    private new void Awake()
    {
        base.Awake();
        if (!PlayerPrefs.HasKey(MUSIC_VALUE))
            InitializeDefaultVolumeSettings();
          
        LoadSettings();
    }

    public void SetMusicVolume()
    {
        PlayerPrefs.SetFloat(MUSIC_VALUE, musicSlider.value);
        float volume = Mathf.Log10(musicSlider.value) * 50;
        audioMixer.SetFloat(MUSIC_VALUE, volume);
    }

    public void SetSfxVolume()
    {
        PlayerPrefs.SetFloat(SFX_VALUE, sfxSlider.value);
        float volume = Mathf.Log10(sfxSlider.value) * 50;
        audioMixer.SetFloat(SFX_VALUE, volume);
    }
    public void SetMasterVolume()
    {

        PlayerPrefs.SetFloat(MASTER_VALUE, masterSlider.value);
        float volume = Mathf.Log10(masterSlider.value) * 50;
        audioMixer.SetFloat(MASTER_VALUE, volume);
        if(volume > -80)
        {
            PlayerPrefs.SetInt(MUTED_VALUE, 0);
            MuteImage.sprite = unmutedSprite;
            masterSlider.colors.Equals(masterSlider.colors.normalColor);
        }
    }
    public void MuteButton()
    {
        if (PlayerPrefs.GetInt(MUTED_VALUE)==0)
        {
            MuteOn();
        }
        else
        {
            MuteOff();
        }
    }
    public void MuteOn()
    {
        PlayerPrefs.SetInt(MUTED_VALUE, 1);
        audioMixer.SetFloat(MASTER_VALUE, -80f);
        MuteImage.sprite = mutedSprite;
        masterSlider.colors.Equals(masterSlider.colors.disabledColor);
    }
    private void MuteOff()
    {
        PlayerPrefs.SetInt(MUTED_VALUE, 0);
        SetMasterVolume();
        MuteImage.sprite = unmutedSprite;
        masterSlider.colors.Equals(masterSlider.colors.normalColor);
    }
    /// <summary>
    /// This function will load playerprefs and apply them
    /// </summary>
    private void LoadSettings()
    {
        musicSlider.value = PlayerPrefs.GetFloat(MUSIC_VALUE);
        sfxSlider.value = PlayerPrefs.GetFloat(SFX_VALUE);
        masterSlider.value = PlayerPrefs.GetFloat (MASTER_VALUE);

        SetMusicVolume();
        SetSfxVolume();
        SetMasterVolume();

        bool muted = PlayerPrefs.GetInt(MUTED_VALUE) == 1;
        if (muted) MuteOn(); else MuteOff();
    }
    private void InitializeDefaultVolumeSettings()
    {
        if (!PlayerPrefs.HasKey(MUSIC_VALUE))
        {
            PlayerPrefs.SetFloat(MUSIC_VALUE, musicSlider.value);
        }

        if (!PlayerPrefs.HasKey(SFX_VALUE))
        {
            PlayerPrefs.SetFloat(SFX_VALUE, sfxSlider.value);
        }
        if (!PlayerPrefs.HasKey(MASTER_VALUE))
        {
            PlayerPrefs.SetFloat(MASTER_VALUE, masterSlider.value);
        }
        if (!PlayerPrefs.HasKey(MUTED_VALUE))
        {
            PlayerPrefs.SetInt(MUTED_VALUE, 0);
        }
    }
}
