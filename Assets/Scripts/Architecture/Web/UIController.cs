using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using static Doozy.Editor.UIManager.UIMenu.UIMenuContextMenu.Scripts;

public class UIController : MonoBehaviour, IController
{

    private Dictionary<UIPage, UIPageName> uiPages;
    private UIPageName currentPageName;
    public void Init() {

        uiPages = new Dictionary<UIPage, UIPageName>();
        List<UIPage> pages = new List<UIPage> { FindAnyObjectByType<UIPage>() };
        foreach (UIPage page in pages) {
            uiPages.Add(page, page.uIPageName);
        }
        currentPageName = UIPageName.startScreen;
    }



}
