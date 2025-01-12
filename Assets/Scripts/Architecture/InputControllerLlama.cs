using UnityEngine;
using DG.Tweening;

public class InputControllerLlama : InputController
{
    [SerializeField] private CanvasGroup fieldAnim;
    [SerializeField] private CanvasGroup buttonAnim;

    private LLamaSharpController lLamaSharpController;
    private LevelController levelController;

    #region MonoBehaviour
    protected override void Start()
    {
        base.Start();

        lLamaSharpController = GameContext.instance.GetControllerByType<LLamaSharpController>();

        onSendMessage.AddListener(userText =>
        {
            lLamaSharpController._submittedText = userText;
            RefreshField();
        });

        lLamaSharpController.onChangeInputInteract.AddListener(ChangeInteractive);

        levelController = GameContext.instance.GetControllerByType<LevelController>();
        levelController.onLevelWin.AddListener(ChangeFieldToButton);

        onSendMessage.AddListener(levelController.onNewMessage);

    }
    private void OnEnable()
    {
        ChangeButtonToField();
    }
    #endregion

    #region view
    private void ChangeFieldToButton()
    {

        fieldAnim.DOFade(0f, 1).OnComplete(() =>
        {
            fieldAnim.interactable = false;
            fieldAnim.blocksRaycasts = false;
            buttonAnim.DOFade(1f, 1).OnComplete(() =>
            {
                buttonAnim.interactable = true;
                buttonAnim.blocksRaycasts = true;
            });

        });
    }
    private void ChangeButtonToField()
    {
        buttonAnim.interactable = false;
        buttonAnim.blocksRaycasts = false;
        buttonAnim.alpha = 0f;

        fieldAnim.DOFade(1f, 1).OnComplete(() =>
        {
            fieldAnim.interactable = true;
            fieldAnim.blocksRaycasts = true;
        });
    }
    #endregion
}
