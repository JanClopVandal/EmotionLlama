using UnityEngine;
using DG.Tweening;

public class AudioController : MonoBehaviour, IController
{
    [Header("Enviroment Sound")]
    [SerializeField] private AudioSource audioSource_Env;
    [SerializeField] private AudioLowPassFilter audioFilter_Env;
    [SerializeField] private Vector2 filterAnimValue_Env;
    [SerializeField] private float filterTime_Env;
    [Space(10)]
    [Header("Sound Effects")]
    [SerializeField] private AudioSource audioSource_Shot;


    private LevelController levelController;

    public void Init()
    {
        levelController = GameContext.instance.GetControllerByType<LevelController>();
        levelController.onChangeLevelSetting.AddListener(ChangeLevelSetting);
        levelController.onInteract.AddListener(PlayOneOfEffect); 
    }


    #region EventTriggers
    private void ChangeLevelSetting(LevelSettings levelSettings)
    {
        if (levelSettings.enviromentSound != null) 
        ChangeEnviromentSound(levelSettings.enviromentSound);
    }
    private void PlayOneOfEffect(LevelSettings levelSettings)
    {
        if (levelSettings.SoundEffects != null)
            PlayOneShot(levelSettings.SoundEffects[Random.Range(0, levelSettings.SoundEffects.Count)]);
    }

    #endregion

    #region AudioControl
    private void ChangeEnviromentSound(AudioClip clip)
    {
        DOTween.To(() => audioFilter_Env.cutoffFrequency, x => audioFilter_Env.cutoffFrequency = x, filterAnimValue_Env.x, filterTime_Env)
        .OnComplete(() =>
        {
            audioSource_Env.clip = clip;
            audioSource_Env.Play();
            DOTween.To(() => audioFilter_Env.cutoffFrequency, x => audioFilter_Env.cutoffFrequency = x, filterAnimValue_Env.y, filterTime_Env);

        });


    }
    private void PlayOneShot(AudioClip clip)
    {
        audioSource_Shot.clip = clip;
        audioSource_Shot.Play();
    }
    #endregion

}
