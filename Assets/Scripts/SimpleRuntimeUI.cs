using UnityEngine;
using UnityEngine.UIElements;

public class SimpleRuntimeUI : MonoBehaviour
{
    private Button _button;
    private Toggle _toggle;
    private int _clickCount;

    public static void InputMessage(ChangeEvent<string> e)
    {
        Debug.Log($"{e.newValue} -> {e.target}");
    }

    void PrintClickMessage(ClickEvent e)
    {
        ++_clickCount;
        Debug.Log($"{"button"} was clicked!" +
                (_toggle.value ? " Count: " + _clickCount : ""));
    }

    void OnEnable()
    {
        var uiDocument = GetComponent<UIDocument>();
        _button = uiDocument.rootVisualElement.Q("button") as Button;
        _toggle = uiDocument.rootVisualElement.Q("toggle") as Toggle;

        _button.RegisterCallback<ClickEvent>(PrintClickMessage);

        var _inputFields = uiDocument.rootVisualElement.Q("input-message");
        _inputFields.RegisterCallback<ChangeEvent<string>>(InputMessage);
    }
}
