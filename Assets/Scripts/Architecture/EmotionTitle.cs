using TMPro;
using UnityEngine;

public class EmotionTitle : MonoBehaviour
{
    private LevelController levelController;
    private TextMeshProUGUI tmpText;

    #region MonoBehaviour

    private void Start()
    {
        levelController = GameContext.instance.GetControllerByType<LevelController>();

        if (levelController == null)
        {
            Debug.LogError("levelController not found in GameContext.");
            return;
        }


        levelController.onChangeLevelEmotion.AddListener(UpdateText);

        tmpText = GetComponent<TextMeshProUGUI>();

        if (tmpText == null)
        {
            Debug.LogError("TextMeshProUGUI component not found on the GameObject.");
        }
    }
    private void OnDestroy()
    {
        if (levelController != null)
        {
            levelController.onChangeLevelEmotion.RemoveListener(UpdateText);
        }
    }

    #endregion

    #region TextChange

    private void UpdateText(Emotion emotion)
    {
        UpdateText(emotion.ToString());
    }
    private void UpdateText(string newText)
    {
        if (tmpText != null)
        {
            string txt = "Write an " + newText + " message";
            tmpText.text = txt;
        }
        else
        {
            Debug.LogWarning("TextMeshProUGUI component is null, cannot update text.");
        }
    }

    #endregion

}
