using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.UI;

public class LevelController : MonoBehaviour, IController
{
    [SerializeField] private List<LevelSettings> levelSettings;
    [SerializeField] private Transform parentSceneObject;


    private LevelSettings currentLevel;
    private LevelSettings prevLevel;

    private LLamaSharpController lLamaSharpController;
    private int score;
    private bool newMessage = true;

    #region Events
    public UnityEvent onLevelWin;
    public UnityEvent<LevelSettings> onChangeLevelSetting;
    public UnityEvent<Emotion> onChangeLevelEmotion;
    public UnityEvent<LevelSettings> onInteract;
    #endregion

    #region Utility
    public void Init()
    {

        lLamaSharpController = GameContext.instance.GetControllerByType<LLamaSharpController>();
        lLamaSharpController.onNewEmotion.AddListener(SetNewEmotion);
    }
    private void ChangeLevelEmotion()
    {
        currentLevel.levelEmotion = currentLevel.levelEmotions[Random.Range(0, currentLevel.levelEmotions.Count)];
        onChangeLevelEmotion?.Invoke(currentLevel.levelEmotion);
    }
    public void onNewMessage(string message)
    {
        newMessage = true;
    }
    #endregion
    

    #region Interact
    public void ChangeScreenState(int index)
    {
        ScreenState state = (ScreenState)index;
        ChangeScreenState(state);
    }
    public void ChangeScreenState(ScreenState newState)
    {
        if(currentLevel !=null) 
        prevLevel = currentLevel;

        currentLevel = levelSettings.FirstOrDefault(setting => setting.screenState == newState);
        if (currentLevel == null) return;

        currentLevel.sceneObject = Instantiate(currentLevel.scenePrefab, Vector3.zero, Quaternion.identity, parentSceneObject);

        currentLevel.sceneObject.transform.localScale = Vector3.one;
        
        currentLevel.sceneViewer = currentLevel.sceneObject.GetComponent<SceneViewer>();
        if (currentLevel.interative)
        {
            currentLevel.uiElement = GameObject.Find(currentLevel.uiElementName);
            var slider = currentLevel.uiElement.GetComponent<Slider>();

            slider.value = 0;
            ChangeLevelEmotion();
        }

        score = 0;
        onChangeLevelSetting?.Invoke(currentLevel);

        
        StartCoroutine(animateScreenState());
        
    }
    private IEnumerator animateScreenState()
    {
        bool eventTriggered = false;
        UnityAction action;

        if (prevLevel != null)
        {
            eventTriggered = false;
            action = () => eventTriggered = true;
            prevLevel.sceneViewer.onHideEnd.AddListener(action);

            prevLevel.sceneViewer.Hide();
            yield return new WaitUntil(() => eventTriggered);
            prevLevel.sceneViewer.onHideEnd.RemoveListener(action);

            Destroy(prevLevel.sceneObject);
            prevLevel.sceneObject = null;

        }
        

        eventTriggered = false;
        action = () => eventTriggered = true;
        currentLevel.sceneViewer.onShowEnd.AddListener(action);

        currentLevel.sceneViewer.Show();
        yield return new WaitUntil(() => eventTriggered);
        currentLevel.sceneViewer.onShowEnd.RemoveListener(action);

    }

    private void SetNewEmotion(Emotion emotion)
    {
        if (emotion == currentLevel.levelEmotion && newMessage)
        {
            score += 1;
            onInteract?.Invoke(currentLevel);
            currentLevel.sceneViewer.Interact(score);
            if (currentLevel.interative)
            {
                var slider = currentLevel.uiElement.GetComponent<Slider>();

                slider.value = (float)score / ((float)currentLevel.winScore);

            }

            if (score == currentLevel.winScore)
            {
                score = 0;
                //SignalStream.Get("UiController", "NextLevel").SendSignal();
                onLevelWin?.Invoke();
                return;
            }
            newMessage = false;
            ChangeLevelEmotion();
        }


    }

    #endregion


}

public enum ScreenState
{
    menu, info, level01, level02, level
}
