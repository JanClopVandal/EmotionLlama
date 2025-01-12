using UnityEngine;
using TMPro;
using UnityEngine.Events;


public class InputController : MonoBehaviour
{
    [SerializeField] protected TMP_InputField inputField;
    [SerializeField] protected bool canBeEmpty = false;
    [SerializeField] protected int messageLenght = 100;
    protected bool isEdit = false;

    [HideInInspector] public string userText;

    [HideInInspector] public UnityEvent<string> onSendMessage;

    #region MonoBehaviour
    protected virtual void Start()
    {
            if (inputField != null)
            {
                inputField.onValueChanged.AddListener(OnInputFieldChanged);
                inputField.onEndEdit.AddListener(OnEndEdit);
                inputField.onSelect.AddListener(onSelect);
            }

        
    }
    protected virtual void OnDestroy()
    {

            if (inputField != null)
            {
                inputField.onValueChanged.RemoveListener(OnInputFieldChanged);
                inputField.onEndEdit.RemoveListener(OnEndEdit);
                inputField.onSelect.RemoveListener(onSelect);
            }
        
    }
    protected void Update()
    {
        EnterClickCheck();
    }
    #endregion

    #region FieldEvents
    protected virtual void onSelect(string text)
    {
        isEdit = true;

    }
    protected virtual void OnInputFieldChanged(string text)
    {
        
        userText = LimitMessageLength(text, messageLenght);

        inputField.text = userText;
        
    }
    protected virtual void OnEndEdit(string text)
    {
        isEdit = true;
    }

    protected void EnterClickCheck()
    {
        if (Input.GetKeyDown(KeyCode.Return) )
        {
            if(isEdit && !isFieldEmpty())
            {
                onSendMessage?.Invoke(userText);
            }
        }
    }
    
    #endregion

    #region Utility
    public virtual void RefreshField()
    {
        userText = null;

            inputField.text = "";
        
    }
    public virtual void ChangeInteractive(bool state)
    {
   
            inputField.interactable = state;
        
    }
    public virtual bool isFieldEmpty()
    {
        bool isEmpty = false;

        if (canBeEmpty) return isEmpty;

        if (userText == null && userText != "")
        {
            isEmpty = true;
        }
        return isEmpty;
    }

    public string LimitMessageLength(string message, int maxLength)
    {
        if (message.Length > maxLength)
        {
            return message.Substring(0, maxLength);
        }
        return message;
    }

    #endregion

}
