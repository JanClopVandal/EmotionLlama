using UnityEngine;
using System.Collections.Generic;
using LLama;
using LLama.Common;
using Cysharp.Threading.Tasks;
using TMPro;
using System.Threading;
using static LLama.StatefulExecutorBase;
using System;
using UnityEngine.Events;

public class LLamaSharpController : MonoBehaviour, IController
{
    public string ModelPath = "models/mistral-7b-instruct-v0.1.Q4_K_M.gguf"; // change it to your own model path
    [TextArea(3, 10)]
    public string SystemPrompt;
    public TMP_Text Output;
    public TMP_Text OutputToken;
    //public TMP_InputField Input;
    public TMP_Dropdown SessionSelector;
    public InputController inputController;
    //public Button Submit;

    public VFXController vFXController;

    private ExecutorBaseState _emptyState;
    private List<ExecutorBaseState> _executorStates = new List<ExecutorBaseState>();
    private List<ChatSession> _chatSessions = new List<ChatSession>();
    private int _activeSession = 0;

    private string _submittedText = "";
    private CancellationTokenSource _cts;
    public void Init() { }

    public UnityEvent<Emotion> onNewEmotion;

    async UniTaskVoid Start()
    {
        _cts = new CancellationTokenSource();
        SetInteractable(false);
        inputController.onEnterPressed.AddListener(() =>
        {
            _submittedText = inputController.userText;
            inputController.RefreshField();
        });
        Output.text = "User: ";
        // Load a model
        var parameters = new ModelParams(Application.streamingAssetsPath + "/" + ModelPath)
        {
            ContextSize = 4096,
           
            GpuLayerCount = 35
        };
        // Switch to the thread pool for long-running operations
        await UniTask.SwitchToThreadPool();
        using var model = LLamaWeights.LoadFromFile(parameters);
        await UniTask.SwitchToMainThread();
        // Initialize a chat session
        using var context = model.CreateContext(parameters);
        var ex = new InteractiveExecutor(context);
        // Save the empty state for cases when we need to switch to empty session
        _emptyState = ex.GetStateData();
        foreach (var option in SessionSelector.options)
        {
            var session = new ChatSession(ex);
            // This won't process the system prompt until the first user message is received
            // to pre-process it you'd need to look into context.Decode() method.
            // Create an issue on github if you need help with that.
            session.AddSystemMessage(SystemPrompt);
            _chatSessions.Add(session);
            _executorStates.Add(null);
        }
        SessionSelector.onValueChanged.AddListener(SwitchSession);
        _activeSession = 0;
        // run the inference in a loop to chat with LLM
        await ChatRoutine(_cts.Token);
        
    }

    /// <summary>
    /// Chat routine that sends user messages to the chat session and receives responses.
    /// </summary>
    /// <param name="session">Active chat session</param>
    /// <param name="cancel">Cancellation token to stop the routine</param>
    /// <returns></returns>
    public async UniTask ChatRoutine(CancellationToken cancel = default)
    {
        var userMessage = "";
        string tokenText = "";
        while (!cancel.IsCancellationRequested)
        {
            // Allow input and wait for the user to submit a message or switch the session
            SetInteractable(true);
            await UniTask.WaitUntil(() => _submittedText != "");
            Output.text = "";
            tokenText = "";
            userMessage = _submittedText;
            _submittedText = "";
            Output.text += " " + userMessage + "\n";
            // Disable input while processing the message
            SetInteractable(false);
            await foreach (var token in ChatConcurrent(
                _chatSessions[_activeSession].ChatAsync(
                    new ChatHistory.Message(AuthorRole.User, userMessage),
                    new InferenceParams()
                    {
                        
                        AntiPrompts = new List<string> { "User:" , "\nUser:", "\n\nUser:" }
                    }
                )
            ))
            {

                Output.text += token;
                tokenText += token;
                
                await UniTask.NextFrame();
            }
            //vFXController.AnswerToColor(tokenText);
            SetEmotionFromText(tokenText);
        }
        
    }


    private void SwitchSession(int index)
    {
        SaveActiveSession();
        SetActiveSession(index);
    }

    /// <summary>
    /// Saves the state of the active chat session executor.
    /// </summary>
    private void SaveActiveSession()
    {
        _executorStates[_activeSession] = (_chatSessions[_activeSession].Executor as InteractiveExecutor).GetStateData();
    }

    /// <summary>
    /// Sets the active chat session and loads its state.
    /// If the session has a saved state, it loads it. Otherwise, it loads an empty state.
    /// </summary>
    /// <param name="index"></param>
    private void SetActiveSession(int index)
    {
        _activeSession = index;
        if (_executorStates[_activeSession] != null)
        {
            (_chatSessions[_activeSession].Executor as InteractiveExecutor).LoadState(_executorStates[_activeSession]);
        }
        else
        {
            (_chatSessions[_activeSession].Executor as InteractiveExecutor).LoadState(_emptyState);
        }
        Output.text = "User: ";
        foreach (var message in _chatSessions[_activeSession].History.Messages)
        {
            // Skip system prompt
            if (message.AuthorRole != AuthorRole.System)
            {
                // Do not add a new line to the last message
                if (!message.Content.Trim().EndsWith("User:"))
                {
                    Output.text += message.Content + "\n";
                }
                else
                {
                    Output.text += message.Content;
                }
            }
        }
    }

    /// <summary>
    /// Cancels the chat routine when the object is destroyed.
    /// </summary>
    private void OnDestroy()
    {
        _cts.Cancel();
    }

    /// <summary>
    /// Wraps AsyncEnumerable with transition to the thread pool. 
    /// </summary>
    /// <param name="tokens"></param>
    /// <returns>IAsyncEnumerable computed on a thread pool</returns>
    private async IAsyncEnumerable<string> ChatConcurrent(IAsyncEnumerable<string> tokens)
    {
        await UniTask.SwitchToThreadPool();
        await foreach (var token in tokens)
        {
            yield return token;
        }
    }

    /// <summary>
    /// Sets the interactable property of the UI elements.
    /// </summary>
    /// <param name="interactable"></param>
    private void SetInteractable(bool interactable)
    {
        //Submit.interactable = interactable;
        inputController.ChangeInteractive(interactable);
        //SessionSelector.interactable = interactable;
    }

    private void SetEmotionFromText(string text)
    {
        string lowerCaseText = text.ToLower();


        foreach (Emotion emotion in Enum.GetValues(typeof(Emotion)))
        {
            
            if (lowerCaseText.Contains(emotion.ToString().ToLower()))
            {
                onNewEmotion.Invoke(emotion); ;
            }
        }

    }
}


public enum Emotion
{
    Compassion,
    Joy,
    Love,
    Inspiration,
    Respect,
    Anger,
    Hatred,
    Sadness,
    Envy,
    Fear,
    Surprise,
    Calmness,
    Doubt,
    Interest,
    Anticipation
}
