using UnityEngine;
using UnityEngine.Events;

public class UIAnimator : MonoBehaviour, IController
{

    public UnityEvent onAnimationFinish;

    public void Init() { }
    
    public void ChangeScreen(UIPage hidePage, UIPage showPage, UIAnimation animationType = UIAnimation.fade, float duration = 1f)
    {

    }

    private void PageFadeAnimation(UIPage hidePage, UIPage showPage, float duration)
    {
        showPage.obj.SetActive(true);

        //canvasGroup.DOKill(); 
        //canvasGroup.alpha = 0f; 

        //canvasGroup.DOFade(1f, fadeDuration)
        //    .OnStart(() =>
        //    {
        //        canvasGroup.interactable = true;
        //        canvasGroup.blocksRaycasts = true;
        //    })
        //    .SetEase(Ease.OutQuad);
    }

}
public enum UIAnimation
{
    fade, blackScreen
}
