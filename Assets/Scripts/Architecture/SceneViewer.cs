using Doozy.Runtime.Reactor.Animators;
using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System;

public class SceneViewer : MonoBehaviour
{

    [SerializeField] private AppearanceType appearanceType;
    [SerializeField] private Animator animator;
    
    public UnityEvent onShowEnd;
    public UnityEvent onHideEnd;

    public virtual void Show()
    {
        switch (appearanceType)
        {
            case AppearanceType.Animation:
                PlayAnimation("Show", onShowEnd);
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

        }
        

    }


    public void PlayAnimation(string triggerName, UnityEvent onAnimationEnd)
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
}


enum AppearanceType
{
    Animation, DoTween, VFX
}
