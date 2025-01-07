using UnityEngine;

public class WebOpen : MonoBehaviour
{
    public void OpenWebLink(string url)
    {
        Application.OpenURL(url);
    }
}
