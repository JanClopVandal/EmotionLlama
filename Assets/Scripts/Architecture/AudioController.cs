using UnityEngine;
using DG.Tweening;
using UnityEngine.Audio;

public class AudioController : MonoBehaviour, IController
{

    [SerializeField] private AudioSource audioSource_Env;
    [SerializeField] private AudioLowPassFilter audioFilter_Env;
    [SerializeField] private Vector2 filterAnimValue_Env;
    [SerializeField] private float filterTime_Env;
    private LevelController levelController;

    public void Init()
    {
        levelController = GameContext.instance.GetControllerByType<LevelController>();
        levelController.onChangeLevelSetting.AddListener(ChangeLevelSetting);
    }

    public void ChangeLevelSetting(LevelSettings levelSettings)
    {
        if (levelSettings.enviromentSound != null) 
        ChangeEnviromentSound(levelSettings.enviromentSound);
    }
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

}
