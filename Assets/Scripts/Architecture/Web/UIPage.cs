using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

[RequireComponent(typeof(CanvasGroup))]
public class UIPage : MonoBehaviour
{
    [Header("Settings")]
    public UIPageName uIPageName;
    [HideInInspector] public CanvasGroup canvasGroup;
    [HideInInspector] public GameObject obj;
    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        obj = GetComponent<GameObject>();
    }
}
public enum UIPageName
{
    startScreen, menu, level, info
}

