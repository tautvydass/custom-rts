using UnityEngine;
using UnityEngine.UI;
using System;

[Serializable]
public class ErrorManager : IErrorManager
{
    public static IErrorManager Instance { get; private set; }

    [SerializeField]
    private Text errorTextField;

    public event Action OnErrorThrown;

    public void Initialize()
    {
        Instance = this;
    }

    public void ShowError(string message)
    {
        OnErrorThrown?.Invoke();
        errorTextField.text = message;
    }

    public void HideError()
    {
        errorTextField.text = "";
    }
}

