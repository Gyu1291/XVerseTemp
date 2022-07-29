using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XVerse.UI;

public class LoginUI : MonoBehaviour
{
    public XButton LoginBtn;
    public XInputField EmailInput;

    private void onEmailInput(string str)
    {
        Debug.Log(str);
    }

}
