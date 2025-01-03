using NUnit.Framework;
using System.Net.Http;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using UnityEngine.Events;
using System;
using UnityEngine.SocialPlatforms.Impl;

public class LevelController : MonoBehaviour, IController
{
    [SerializeField] private List<LevelSettings> levelSettings;
    [SerializeField] private Transform parentSceneObject;
    private LevelSettings currentLevel;
    private LevelSettings prevLevel;

    private LLamaSharpController lLamaSharpController;
    private int score;
    public void Init() {

        lLamaSharpController = GameContext.instance.GetControllerByType<LLamaSharpController>();
        lLamaSharpController.onNewEmotion.AddListener(SetNewEmotion);

    }


    public void SetNewEmotion(Emotion emotion)
    {
        if (emotion == currentLevel.levelEmotion)
        {
            score += 1;
            if(score >= currentLevel.winScore)
            {
                score = 0;
                return;
            }
            currentLevel.sceneViewer.Interact(score);
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
