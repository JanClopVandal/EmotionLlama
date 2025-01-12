using UnityEngine;
using System.Collections;
using UnityEngine.Events;


public class SceneViewer : MonoBehaviour
{

    [SerializeField] private AppearanceType appearanceType;
    [SerializeField] private Animator animator;

    #region Events
    [HideInInspector] public UnityEvent onAnimEnd;
    [HideInInspector] public UnityEvent onShowEnd;
    [HideInInspector] public UnityEvent onHideEnd;
    #endregion

    #region Interact
    public virtual void Show()
    {
        switch (appearanceType)
        {
            case AppearanceType.Animation:
                PlayAnimation("Show", onShowEnd);
                break;
            case AppearanceType.Custom:
                onShowEnd?.Invoke();
                break;




        }

    }
    public virtual void Hide()
    {
        switch (appearanceType)
        {
            case AppearanceType.Animation:
                PlayAnimation("Hide", onHideEnd);
                break;
            case AppearanceType.Custom:
                onHideEnd?.Invoke();
                break;

        }
        

    }
    public void Interact(int numState)
    {
        switch (appearanceType)
        {
            case AppearanceType.Animation:
                PlayAnimation("Show", onAnimEnd);
                break;
            case AppearanceType.Custom:
                
                break;

        }
    }
    #endregion

    #region Animation
    private void PlayAnimation(string triggerName)
    {
        PlayAnimation(triggerName, null);
    }
    private void PlayAnimation(string triggerName, UnityEvent onAnimationEnd)
    {
        if (animator == null || string.IsNullOrEmpty(triggerName))
        {
            Debug.LogWarning("Animator or trigger name is not set.");
            return;
        }
        animator.SetTrigger(triggerName);

        StartCoroutine(WaitForAnimationEnd(triggerName, onAnimationEnd));
    }
    
    private IEnumerator WaitForAnimationEnd(string triggerName, UnityEvent onAnimationEnd)
    {
        AnimatorStateInfo animationState;
        do
        {
            animationState = animator.GetCurrentAnimatorStateInfo(0);
            yield return null;
        }
        while (animationState.IsName(triggerName) && animationState.normalizedTime < 1f);

        onAnimationEnd?.Invoke();
        Debug.Log($"Animation with trigger '{triggerName}' finished!");
    }
    #endregion
}


enum AppearanceType
{
    Animation, DoTween, VFX,Custom
}
