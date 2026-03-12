using UnityEngine;


[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{

    #region Fields

    [Header("BackgroundMusic")]
    [SerializeField] private AudioClip bgmClip;

    [Header("SFX")]
    [SerializeField] private AudioClip buttonClickClip;
    [SerializeField] private AudioClip placementClip;
    [SerializeField] private AudioClip popupClip;
    [SerializeField] private AudioClip winClip;

    // BGM uses the attached AudioSource for looping
    // SFX uses a secondary source to avoid interrupting BGM
    private AudioSource bgmSource;
    private AudioSource sfxSource;

    #endregion


    #region Properties

    public static AudioManager Instance { get; private set; }

    #endregion


    #region Methods

    public void PlayBGM()
    {
        if (!SettingsPopup.IsBGMEnabled() || bgmClip == null)
        {
            return;
        }

        if (bgmSource.isPlaying)
        {
            return; // already playing, don't restart
        }

        bgmSource.Play();
    }

    public void PlayButtonSFX()
    {
        PlaySFX(buttonClickClip);
    }

    public void PlayPlacementSFX()
    {
        PlaySFX(placementClip);
    }

    public void PlayPopupSFX()
    {
        PlaySFX(popupClip);
    }

    public void PlayWinSFX()
    {
        PlaySFX(winClip);
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }

        else
        {
            Instance = this;
        }

        DontDestroyOnLoad(gameObject);

        SetupAudioSources();
    }

    private void PlaySFX(AudioClip clip)
    {
        if (!SettingsPopup.IsSFXEnabled() || clip == null)
        {
            return;
        }

        sfxSource.PlayOneShot(clip);
    }

    private void SetupAudioSources()
    {
        // First AudioSource on the GameObject is for music.
        bgmSource = GetComponent<AudioSource>();
        bgmSource.loop = true;
        bgmSource.playOnAwake = false;
        bgmSource.clip = bgmClip;

        // Second AudioSource added dynamically for SFX.
        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.loop = false;
        sfxSource.playOnAwake = false;
    }

    private void Start()
    {
        PlayBGM();
    }

    #endregion

}