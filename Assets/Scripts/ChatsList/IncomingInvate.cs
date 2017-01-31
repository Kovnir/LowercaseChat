using System;
using System.Linq;
using GameSparks.Api.Responses;
using UnityEngine;
using UnityEngine.UI;

public class IncomingInvate : MonoBehaviour
{
    [SerializeField] private Text chatname;
    [SerializeField] private Text users;
    private Action<GameObject, ListChallengeResponse._Challenge, bool> onResponse;
    private ListChallengeResponse._Challenge challenge;


    public void Init(ListChallengeResponse._Challenge challenge, Action<GameObject, ListChallengeResponse._Challenge, bool> responseCallback)
    {
        this.challenge = challenge;
        chatname.text = challenge.ChallengeMessage;
        users.text = challenge.Accepted.Count().ToString();
        onResponse = responseCallback;
    }

    public void Accept()
    {
        NetworkManager.AcceptChatInvite(challenge.ChallengeId, () =>
        {
            if (onResponse != null)
            {
                onResponse(gameObject, challenge, true);
            }
        }, () =>
        {
            Debug.LogError("Accept chat error");
        });
    }

    public void Discard()
    {
        NetworkManager.DeclineChatInvite(challenge.ChallengeId, () =>
        {
            if (onResponse != null)
            {
                onResponse(gameObject, challenge, false);
            }
        }, () =>
        {
            Debug.LogError("Decline chat error");
        });
    }
}
