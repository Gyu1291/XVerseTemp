using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XVerse.UI;

public class SignUpUI : MonoBehaviour
{
    public XButton ContinueBtn;
    private XInputField _emailInput;
    private XInputField _pwInput;
    private XInputField _pwVerifyInput;

    private RectTransform _emailHolder;
    private RectTransform _passwordHolder;

    private void onEmailInput(string str)
    {
        Debug.Log(str);
    }

}
