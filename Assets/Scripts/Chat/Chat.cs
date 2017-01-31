using System;
using GameSparks.Api.Messages;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class Chat : MonoBehaviour
{
    [SerializeField] private Transform messagesList;
    [SerializeField] private Text messagePrefab;
    [SerializeField] private InputField sendingText;
    [SerializeField] private Text newMemberName;
     
    [UsedImplicitly]
    private void OnEnable()
    {
        for (int i = 0; i < messagesList.childCount; i++)
        {
            Destroy(messagesList.GetChild(i).gameObject);
        }
        ScriptMessage.Listener += GetMessages;
    }
    [UsedImplicitly]
    private void OnDisable()
    {
        ScriptMessage.Listener -= GetMessages;
    }

    public void GetMessages(ScriptMessage message)
    {
        Text text = Instantiate(messagePrefab);
        text.text = "<b><color=#3B5757FF>" + message.Data.GetString("displayName").ToLower() + ":</color></b>\n"+ message.Data.GetString("message");
        text.transform.SetParent(messagesList);
    }

    public void Send()
    {
        if (string.IsNullOrEmpty(sendingText.text))
        {
            return;
        }
        NetworkManager.SendMessage(sendingText.text.ToLower());
        sendingText.text = String.Empty;
    }
}