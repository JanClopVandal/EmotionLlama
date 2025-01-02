using UnityEngine;


[CreateAssetMenu(fileName = "Level", menuName = "Game/Level")]
public class LevelSettings : ScriptableObject
{
    public ScreenState screenState;
    public GameObject scenePrefab;
    public GameObject sceneObject;
    public SceneViewer sceneViewer;
    public bool interative;
    public Emotion levelEmotion;
    public int winScore;


}

