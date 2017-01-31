using System;
using System.Linq;
using GameSparks.Api.Responses;
using UnityEngine;
using UnityEngine.UI;

public class ActiveChat : MonoBehaviour
{

    [SerializeField]
    private Text chatName;
    [SerializeField]
    private Text count;
    private Action<ListChallengeResponse._Challenge> onStartClick;
    private Action<GameObject, ListChallengeResponse._Challenge> onRemoveClick;
    private ListChallengeResponse._Challenge challenge;

    public void Init(ListChallengeResponse._Challenge challenge, Action<ListChallengeResponse._Challenge> startClickCallback, Action<GameObject, ListChallengeResponse._Challenge> removeClickCallback)
    {
        this.challenge = challenge;
        chatName.text = challenge.ChallengeMessage;
        count.text = challenge.Accepted.Count().ToString();
        onStartClick = startClickCallback;
        onRemoveClick = removeClickCallback;
    }

    public void StartClick()
    {
        onStartClick(challenge);
    }

    public void RemoveClick()
    {
        NetworkManager.LeaveChat(challenge.ChallengeId);
        if (onRemoveClick != null)
        {
            onRemoveClick(gameObject, challenge);
        }
    }
}
