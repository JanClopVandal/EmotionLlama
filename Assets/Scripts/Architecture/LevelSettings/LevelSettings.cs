using UnityEngine;
using System.Collections.Generic;


[CreateAssetMenu(fileName = "Level", menuName = "Game/Level")]
public class LevelSettings : ScriptableObject
{
    [Header("Scene")]
    public ScreenState screenState;
    public GameObject scenePrefab;
    [HideInInspector] public GameObject sceneObject;
    [HideInInspector] public SceneViewer sceneViewer;
    [Space(10)]

    [Header("Level setting")]
    public bool interative;
    [HideInInspector] public Emotion levelEmotion;
    public List<Emotion> levelEmotions;
    public int winScore;
    public string uiElementName;
    [HideInInspector] public GameObject uiElement;
    [Space(10)]

    [Header("Sound setting")]
    public AudioClip enviromentSound;
    public List<AudioClip> SoundEffects;


}

