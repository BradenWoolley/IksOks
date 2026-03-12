using UnityEngine;


[CreateAssetMenu(fileName = "AudioConfig", menuName = "ScriptableObjects/AudioConfig")]
public class AudioConfig : ScriptableObject
{

    #region Properties

    [Header("Music")]
    public AudioClip BgmClip;

    [Header("SFX")]
    public AudioClip ButtonClickClip;

    public AudioClip PlacementClip;

    public AudioClip PopupClip;

    public AudioClip WinClip;

    #endregion

}