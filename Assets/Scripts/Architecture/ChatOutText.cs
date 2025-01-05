using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class ChatOutText : MonoBehaviour
{
    private LLamaSharpController lLamaSharpController;
    private TextMeshProUGUI tmpText;

    void Start()
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

    void OnDestroy()
    {
        if (lLamaSharpController != null)
        {
            lLamaSharpController.onNewChatText.RemoveListener(UpdateText);
        }
    }
}
