using UnityEngine;
using TMPro;

public class ChatOutText : MonoBehaviour
{
    private LLamaSharpController lLamaSharpController;
    private TextMeshProUGUI tmpText;

    #region MonoBehaviour
    private void Start()
    {
        lLamaSharpController = GameContext.instance.GetControllerByType<LLamaSharpController>();

        if (lLamaSharpController == null)
        {
            Debug.LogError("LLamaSharpController not found in GameContext.");
            return;
        }

  
        lLamaSharpController.onNewChatText.AddListener(UpdateText);

        tmpText = GetComponent<TextMeshProUGUI>();

        if (tmpText == null)
        {
            Debug.LogError("TextMeshProUGUI component not found on the GameObject.");
        }
    }
    private void OnDestroy()
    {
        if (lLamaSharpController != null)
        {
            lLamaSharpController.onNewChatText.RemoveListener(UpdateText);
        }
    }
    #endregion

    private void UpdateText(string newText)
    {
        if (tmpText != null)
        {
            tmpText.text = newText;
        }
        else
        {
            Debug.LogWarning("TextMeshProUGUI component is null, cannot update text.");
        }
    }
}
