using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class ChatCreating : MonoBehaviour
{
    [SerializeField]
    private Text title;

    [SerializeField]
    private Text chatName;
    [SerializeField]
    private InputField userName;
    [SerializeField]
    private UserToInvite userToInvite;
    [SerializeField]
    private Transform usersHolder;

    private List<string> userNames;
    private List<string> userIds;


    [UsedImplicitly]
    private void OnEnable()
    {
        userName.text = String.Empty;
        chatName.text = String.Empty;
        title.text = "hi, <color=#164C4C>" + NetworkManager.Username.ToLower() + "</color>! nice to see you!";
        for (int i = 0; i < usersHolder.childCount; i++)
        {
            Destroy(usersHolder.GetChild(i).gameObject);
        }
    }

    public void AddUser()
    {
        if (userName.text == String.Empty)
        {
            SystemMessages.ShowError("enter user name");
            return;
        }
        UserToInvite user = Instantiate(userToInvite);
        user.Init(userName.text.ToLower());
        user.transform.SetParent(usersHolder);
        userName.text = String.Empty;
    }
    

    public void Invite()
    {
        if (string.IsNullOrEmpty(chatName.text))
        {
            SystemMessages.ShowError("enter chat name");
            return;
        }

        if (usersHolder.childCount == 0)
        {
            SystemMessages.ShowError("you must invite at least one user");
            return;
        }

        userNames = new List<string>();
        for (int i = 0; i < usersHolder.childCount; i++)
        {
            userNames.Add(usersHolder.GetChild(i).GetComponent<UserToInvite>().Username);
        }
        UIController.Instance.BlockInput();
        userIds = new List<string>();
        GetId(0, () =>
        {
            NetworkManager.InviteToChat(chatName.text.ToLower(), userIds, () =>
            {
                SystemMessages.ShowMessage("done. wait for accept.");
                UIController.Instance.ShowChatListWindow();
                UIController.Instance.UnblockInput();
            },
                () =>
                {
                    SystemMessages.ShowError("error");
                    UIController.Instance.UnblockInput();
                }
               );
        });
    }

    private void GetId(int index, Action OnComplete)
    {
        NetworkManager.GetUserId(userNames[index], userId =>
        {
            userIds.Add(userId);
            index++;
            if (userNames.Count > index)
            {
                GetId(index, OnComplete);
            }
            else
            {
                OnComplete();
            }
        }, () =>
        {
            SystemMessages.ShowError("user "+ userNames[index] +" not found");
            UIController.Instance.UnblockInput();
        });
    }
}
