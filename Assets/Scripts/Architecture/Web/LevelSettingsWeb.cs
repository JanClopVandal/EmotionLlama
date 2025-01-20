using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Game/Level_Web")]
public class LevelSettingsWeb : ScriptableObject
{
    [Header("Scene")]
    public ScreenState screenState;
    public GameObject scenePrefab;
    [HideInInspector] public GameObject sceneObject;
    [HideInInspector] public SceneViewer sceneViewer;

    public bool interative;

    
    [Space(10)]

    [Header("Sound setting")]
    public AudioClip waweSampleBase;
    public AudioClip waweSampleModule;
    public Vector2 pithFrame;
}
