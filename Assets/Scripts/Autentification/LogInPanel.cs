using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogInPanel : MonoBehaviour
{
    [SerializeField] private Text login;
    [SerializeField] private InputField password;

	public void OnLogInClick ()
    {
	    if (login.text == string.Empty)
        {
            SystemMessages.ShowError("enter login");
            return;
	    }
        if (password.text == string.Empty)
        {
            SystemMessages.ShowError("enter password");
            return;
	    }
        UIController.Instance.BlockInput();
		NetworkManager.LogIn(login.text.ToLower(), password.text, () =>
		{
            UIController.Instance.ShowChatListWindow();
        }, () =>
        {
            SystemMessages.ShowError("bad login or password");
            UIController.Instance.UnblockInput();
        });
	}
}
