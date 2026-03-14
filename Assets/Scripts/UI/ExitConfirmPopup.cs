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
        confirmButton?.onClick.AddListener(OnConfirmClicked);
        cancelButton?.onClick.AddListener(OnCancelClicked);
    }

    private void OnConfirmClicked()
    {
        AudioManager.Instance?.PlayButtonSFX();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }

    private void OnCancelClicked()
    {
        AudioManager.Instance?.PlayButtonSFX();
        Hide();
    }

    private void OnDestroy()
    {
        confirmButton?.onClick.RemoveListener(OnConfirmClicked);
        cancelButton?.onClick.RemoveListener(OnCancelClicked);
    }

    #endregion

}