using UnityEngine;


public class AudioManager : MonoBehaviour
{

    #region Properties

    public static AudioManager Instance { get; private set; }

    #endregion


    #region Methods

    // Todo: Implement logic.
    public void PlayBGM()
    {
    }

    public void PlayButtonSFX()
    {
    }

    public void PlayPlacementSFX()
    {
    }

    public void PlayPopupSFX()
    {
    }

    public void PlayWinSFX()
    {
    }

    public void StopBGM()
    {
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
    }

    #endregion

}