using UnityEngine;
using UnityEngine.UI;

public class RegistrationPanel : MonoBehaviour
{
    [SerializeField] private Text userName;
    [SerializeField] private InputField password;
    [SerializeField] private InputField password2;
    
    public void RegistrateClick ()
	{
        if (userName.text == string.Empty)
        {
            SystemMessages.ShowError("enter login");
            return;
        }
        if (password.text == string.Empty)
        {
            SystemMessages.ShowError("enter password");
            return;
        }
        if (password.text != password2.text)
        {
            SystemMessages.ShowError("passwords are not the same");
            return;
        }

        UIController.Instance.BlockInput();
		NetworkManager.Registration(userName.text, password.text, () =>
		{
            UIController.Instance.ShowChatListWindow();
            UIController.Instance.UnblockInput();
        }, (str) =>
	    {
	        SystemMessages.ShowError("try another name");
            UIController.Instance.UnblockInput();
        });
    }
}
