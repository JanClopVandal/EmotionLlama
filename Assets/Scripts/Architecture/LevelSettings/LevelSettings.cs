using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;


[CreateAssetMenu(fileName = "Level", menuName = "Game/Level")]
public class LevelSettings : ScriptableObject
{
    public ScreenState screenState;
    public GameObject scenePrefab;
    [HideInInspector] public GameObject sceneObject;
    [HideInInspector] public SceneViewer sceneViewer;
    public bool interative;
    public Emotion levelEmotion;
    public int winScore;
    public string uiElementName;
    [HideInInspector] public GameObject uiElement;
    public AudioClip enviromentSound;
    public List<AudioClip> SoundEffects;


}

