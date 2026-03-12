using UnityEngine;
using UnityEngine.UI;


public class ExitConfirmPopup : PopupBase
{

    #region Fields

    [Header("Buttons")]
    [SerializeField]
    private Button cancelButton;

    [SerializeField]
    private Button confirmButton;

    #endregion


    #region Methods

    protected override void Awake()
    {
        base.Awake();

        confirmButton?.onClick.AddListener(() =>
        {
            AudioManager.Instance?.PlayButtonSFX();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        });

        cancelButton?.onClick.AddListener(() =>
        {
            AudioManager.Instance?.PlayButtonSFX();
            Hide();
        });
    }

    #endregion

}