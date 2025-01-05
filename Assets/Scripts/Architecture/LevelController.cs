using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using UnityEngine.Events;
using Doozy.Runtime.Common.Extensions;
using UnityEngine.UI;
using Doozy.Runtime;
using Doozy.Runtime.Signals;

public class LevelController : MonoBehaviour, IController
{
    [SerializeField] private List<LevelSettings> levelSettings;
    [SerializeField] private Transform parentSceneObject;
    //[SerializeField] private Slider slider;

    private LevelSettings currentLevel;
    private LevelSettings prevLevel;

    private LLamaSharpController lLamaSharpController;
    private int score;

    public UnityEvent onLevelWin;

    public void Init() {

        lLamaSharpController = GameContext.instance.GetControllerByType<LLamaSharpController>();
        lLamaSharpController.onNewEmotion.AddListener(SetNewEmotion);



    }


    public void SetNewEmotion(Emotion emotion)
    {
        if (emotion == currentLevel.levelEmotion)
        {
            score += 1;
            
            currentLevel.sceneViewer.Interact(score);
            if (currentLevel.uiElement != null)
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

        }


    }
 


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
        if (!currentLevel.uiElementName.IsNull())
        {
            currentLevel.uiElement = GameObject.Find(currentLevel.uiElementName);
        }

        score = 0;
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


}

public enum ScreenState
{
    menu, info, scoreboard, level01, level02
}
