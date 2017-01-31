using System.Collections;
using GameSparks.Api.Responses;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class ChatListPanel : MonoBehaviour
{
    [SerializeField] private Text title;
    [SerializeField] private Transform invitesList;
    [SerializeField] private Transform activeList;

    [SerializeField] private IncomingInvate incomingInvate;
    [SerializeField] private ActiveChat activeChat;
    
    private bool stopCoroutine;

    [UsedImplicitly]
    private void OnEnable()
    {
        title.text = "hi, <color=#164C4C>" + NetworkManager.Username.ToLower() + "</color>! nice to see you!";
        for (int i = 0; i < invitesList.childCount; i++)
        {
            Destroy(invitesList.GetChild(i).gameObject);
        }
        for (int i = 0; i < activeList.childCount; i++)
        {
            Destroy(activeList.GetChild(i).gameObject);
        }
        stopCoroutine = false;
        StartCoroutine(Updating());
    }

    private IEnumerator Updating()
    {
        while (!stopCoroutine)
        {
            UpdateInvites();
            UpdateActive();
            yield return new WaitForSeconds(3);
        }
    }

    [UsedImplicitly]
    private void OnDisable()
    {
        stopCoroutine = true;
    }

    public void UpdateInvites()
    {
        NetworkManager.GetInvites(invites =>
        {
            for (int i = 0; i < invitesList.childCount; i++)
            {
                Destroy(invitesList.GetChild(i).gameObject);
            }
            foreach (var challenge in invites)
            {
                IncomingInvate go = Instantiate(incomingInvate);
                go.Init(challenge, OnResponse);
                go.transform.SetParent(invitesList);
            }
        });
    }

    public void UpdateActive()
    {
        NetworkManager.GetActiveChats(chats=>
        {
            for (int i = 0; i < activeList.childCount; i++)
            {
                Destroy(activeList.GetChild(i).gameObject);
            }

            foreach (var challenge in chats)
            {
                ActiveChat go = Instantiate(activeChat);
                go.Init(challenge, OnChatStart, OnChatRemoved);
                go.transform.SetParent(activeList);
            }
        });
    }

    private void OnChatStart(ListChallengeResponse._Challenge challenge)
    {
        NetworkManager.Challenge = challenge;
        UIController.Instance.ShowChat();
    }
    private void OnChatRemoved(GameObject gameObject, ListChallengeResponse._Challenge challenge)
    {
        Destroy(gameObject);
    }

    private void OnResponse(GameObject gameObject, ListChallengeResponse._Challenge challenge, bool result)
    {
        Destroy(gameObject);
        if (result)
        {
            OnChatStart(challenge);
        }
    }

}
