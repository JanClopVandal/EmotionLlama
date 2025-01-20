using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using UnityEngine;

public class GameContext : MonoBehaviour
{
    public static GameContext instance;

    public List<IController> controllers;



    public T GetControllerByType<T>() where T : class
    {
        foreach (var controller in controllers)
        {
            if (controller is T desiredController)
            {
                return desiredController;
            }
        }

        Debug.LogWarning($"Controller of type {typeof(T)} not found.");
        return null;
    }


    // Start is called before the first frame update
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        CreateControllers();
        InitControlers();

    }

    private void CreateControllers()
    {
        controllers = new List<IController>
        {
            FindAnyObjectByType<LLamaSharpController>(),
            FindAnyObjectByType<LevelController>(),
            FindAnyObjectByType<AudioController>(),
            FindAnyObjectByType<UIAnimator>(),
            FindAnyObjectByType<UIController>()


        };
    }
    private void InitControlers()
    {
        foreach (var controller in controllers)
        {
            controller.Init();
        }
    }

}