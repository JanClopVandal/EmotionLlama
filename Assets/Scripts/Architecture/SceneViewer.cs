using UnityEngine;
using UnityEngine.Events;

public class SceneViewer : MonoBehaviour
{
    public UnityEvent onShowEnd;
    public UnityEvent onHideEnd;

    public virtual void Show()
    {
        Debug.Log("Show menu");
        onShowEnd?.Invoke();
    }
    
    public virtual void Hide()
    {
        onHideEnd?.Invoke();

    }
}
