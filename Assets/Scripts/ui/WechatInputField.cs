using UnityEngine;
using WeChatWASM;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using static UnityEngine.UI.InputField;

[RequireComponent(typeof(InputField))]
public class WechatInputField : MonoBehaviour, IPointerClickHandler, IPointerExitHandler
{
    private InputField input;
    private bool isShowKeyboard = false;

    private void Awake()
    {
        input = GetComponent<InputField>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("OnPointerClick");
        ShowKeyboard();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("OnPointerExit");
        if (!input.isFocused)
        {
            HideKeyboard();
        }
    }

    public void OnInput(OnKeyboardInputListenerResult v)
    {
        Debug.Log("onInput " + v.value);
        if (input.isFocused)
        {
            input.text = v.value;
        }
    }

    public void OnConfirm(OnKeyboardInputListenerResult v)
    {
        // 输入法confirm回调
        Debug.Log("onConfirm" + v.value);
        input.textComponent.text = v.value;
        HideKeyboard();
        input.DeactivateInputField();
        input.onSubmit.Invoke(v.value);
    }

    public void OnComplete(OnKeyboardInputListenerResult v)
    {
        // 输入法complete回调
        Debug.Log("OnComplete" + v.value);
        input.textComponent.text = v.value;
        HideKeyboard();
        input.DeactivateInputField();
        input.onEndEdit.Invoke(v.value);
    }

    private void ShowKeyboard()
    {
        if (!isShowKeyboard)
        {
            WX.ShowKeyboard(new ShowKeyboardOption()
            {
                defaultValue = input.text,
                maxLength = 200,
                confirmType = "go",
                multiple =  LineType.SingleLine != input.lineType
            });

            // 绑定回调
            WX.OnKeyboardConfirm(OnConfirm);
            WX.OnKeyboardComplete(OnComplete);
            WX.OnKeyboardInput(OnInput);
            isShowKeyboard = true;
        }
    }

    private void HideKeyboard()
    {
        if (isShowKeyboard)
        {
            WX.HideKeyboard(new HideKeyboardOption());
            // 删除掉相关事件监听
            WX.OffKeyboardInput(OnInput);
            WX.OffKeyboardConfirm(OnConfirm);
            WX.OffKeyboardComplete(OnComplete);
            isShowKeyboard = false;
        }
    }
}

