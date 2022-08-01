using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XVerse.UI;
using Firebase;
using Firebase.Auth;
using Xverse.Scene;

public class LoginUI : MonoBehaviour
{
    //Login
    public XButton loginBtn;
    public XInputField loginEmail;
    public XInputField loginPassword;
    public Label loginText;
    public Label createAccountText;

    //Signup
    public XInputField signUpEmail;
    public XButton emailAuthBtn;

    public XInputField signUpPassword;
    public XInputField signUpCheckPassword;
    public XButton signUpBtn;

    //FinishSignup
    public XButton backToLoginBtn;

    //Scene
    private Scene personalScene = Scene.PersonalWorld;

    private void onEmailInput(string str)
    {
        Debug.Log(str);
    }

    private void Start()
    {
        loginBtn.onClick.AddListener(loginBtnClick);
    }

    public void loginBtnClick()
    {
        Debug.Log(loginEmail.text+" "+loginPassword.text);
        StartCoroutine(XVerseFirebase.Instance.LoginLogic(loginEmail.text, loginPassword.text,
            () => {
                Debug.Log("success");
                loginText.color = new Color32(59, 165, 93, 255);
                loginText.text = $"Successfully Logged in as {loginEmail.text}";
                SceneManagerEx.Instance.LoadScene(personalScene);
            },
            (error) => {
                Debug.Log("fail: " + error);
                loginText.color = new Color32(237, 66, 69, 255);
                switch (error)
                {
                    case AuthError.MissingEmail:
                        loginText.text = "Please Enter Your Email";
                        break;
                    case AuthError.MissingPassword:
                        loginText.text = "Please Enter Your Password";
                        break;
                    case AuthError.InvalidEmail:
                        loginText.text = "Invalid Email";
                        break;
                    case AuthError.WrongPassword:
                        loginText.text = "Wrond Password";
                        break;
                    case AuthError.UserNotFound:
                        loginText.text = "Account Does Not Exist";
                        break;
                    default:
                        loginText.text = "Unknown Error, Please Try Again";
                        break;
                }


            }));
    }

    public void signUpBtnClick()
    {
        Debug.Log("Button Clicked");
        StartCoroutine(XVerseFirebase.Instance.RegisterLogic("Soongyuu", signUpEmail.text, signUpPassword.text,
            () =>
            {
                createAccountText.color = new Color32(59, 165, 93, 255);
                createAccountText.text = $"Successfully Created Account!";
            },
            (error) =>
            {
                createAccountText.color = new Color32(237, 66, 69, 255);
                switch (error)
                {
                    case AuthError.MissingEmail:
                        createAccountText.text = "Please Enter Your Email";
                        break;
                    case AuthError.MissingPassword:
                        createAccountText.text = "Please Enter Your Password";
                        break;
                    case AuthError.InvalidEmail:
                        createAccountText.text = "Invalid Email";
                        break;
                    case AuthError.WeakPassword:
                        createAccountText.text = "Weak Password";
                        break;
                    case AuthError.EmailAlreadyInUse:
                        createAccountText.text = "Email Already In Use";
                        break;
                    case AuthError.Cancelled:
                        createAccountText.text = "Update User Cancelled";
                        break;
                    case AuthError.SessionExpired:
                        createAccountText.text = "Session Expired";
                        break;

                    default:
                        createAccountText.text = "Unknown Error, Please Try Again";
                        break;
                }
            }
            ));
    }
}
