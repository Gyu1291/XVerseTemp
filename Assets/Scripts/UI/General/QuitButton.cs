using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XVerse.UI;

public class QuitButton : MonoBehaviour
{
    void Start()
    {
        GetComponent<XButton>().Clicked(() =>
        {
            Application.Quit();
        });
    }

#if UNITY_EDITOR
    public void Build(Canvas main)
    {
        XButton b = XButton.New("Login", null);
        b.Get().Size(200f, 30f).Down().Center().SetScene(main);
        b.gameObject.AddComponent<QuitButton>();

        XInputField i = XInputField.New((str) =>
        {
            Debug.Log(str);
        });
    }
#endif
}
