using System;
using System.Collections.Generic;
using GameSparks.Api.Requests;
using GameSparks.Api.Responses;
using GameSparks.Core;

public static class NetworkManager {
    
    public static string Username { get; private set; }
    public static string UserId { get; private set; }
    public static ListChallengeResponse._Challenge Challenge { get; set; }

    public static void Registration(string username, string password, Action OnComplete = null,
        Action<string> OnFail = null)
    {
        new RegistrationRequest()
            .SetDisplayName(username)
            .SetPassword(password)
            .SetUserName(username)
            .Send((response) =>
            {
                if (response.HasErrors)
                {
                    if (OnFail != null)
                    {
                        OnFail(response.Errors.JSON);
                    }
                }
                else
                {
                    Username = username;
                    UserId = response.UserId;

                    if (OnComplete != null)
                    {
                        OnComplete();
                    }
                }
            });
    }

    public static void LogIn(string username, string password, Action OnComplete = null,
        Action OnFail = null)
    {
        new AuthenticationRequest()
            .SetPassword(password)
            .SetUserName(username)
            .Send((response) =>
            {
                if (!response.HasErrors)
                {
                    Username = username;
                    UserId = response.UserId;
                    if (OnComplete != null)
                    {
                        OnComplete();
                    }
                }
                else
                {
                    if (OnFail != null)
                    {
                        OnFail();
                    }
                }
            });
    }

    public static void GetUserId(string username, Action<string> OnComplete = null,
        Action OnFail = null)
    {
        new LogEventRequest()
       .SetEventKey("GetUserId")
       .SetEventAttribute("username", username.ToLower())
       .Send((response) =>
       {
           if (!response.HasErrors)
                {
                    if (OnComplete != null)
                    {
                        OnComplete(response.ScriptData.GetString("id"));
                    }
                }
                else
                {
                    if (OnFail != null)
                    {
                        OnFail();
                    }
                }
            });
    }
    
    public static void InviteToChat(string chatname, List<string> userIds, Action OnComplete = null,
        Action OnFail = null)
    {
//        toInvite.Add(UserId);
        new CreateChallengeRequest()
            .SetChallengeShortCode("Chat")
            .SetUsersToChallenge(userIds)
            .SetEndTime(DateTime.Now.AddDays(1))
            .SetChallengeMessage(chatname)
            .Send(result =>
            {
                if (!result.HasErrors)
                {
                    if (OnComplete != null)
                    {
                        OnComplete();
                    }
                }
                else
                {
                    if (OnFail != null)
                    {
                        OnFail();
                    }
                }
            });
    }
    
    public static void GetInvites(Action<GSEnumerable<ListChallengeResponse._Challenge>> OnComplete = null)
    {
        new ListChallengeRequest()
            .SetShortCode("Chat")
            .SetState("RECEIVED")
            .Send(response =>
            {
                if (OnComplete != null)
                {
                    OnComplete(response.ChallengeInstances);
                }
            });
    }

    public static void GetActiveChats(Action<List<ListChallengeResponse._Challenge>> OnComplete = null)
    {
        new ListChallengeRequest()
            .SetShortCode("Chat")
            .SetState("RUNNING")
            .Send(response =>
            {
                new ListChallengeRequest()
                    .SetShortCode("Chat")
                    .SetState("ISSUED")
                    .Send(response2 =>
                    {
                        if (OnComplete != null)
                        {
                            List<ListChallengeResponse._Challenge> list = new List<ListChallengeResponse._Challenge>();
                            list.AddRange(response.ChallengeInstances);
                            list.AddRange(response2.ChallengeInstances);
                            OnComplete(list);
                        }
                    });
            });
    }

    public static void AcceptChatInvite(string challengeId, Action OnComplete = null, Action OnFail = null)
    {
        new AcceptChallengeRequest().SetChallengeInstanceId(challengeId)
            .Send((response) =>
            {
                if (response.HasErrors)
                {
                    if (OnFail != null)
                    {
                        OnFail();
                    }
                }
                else
                {
                    if (OnComplete != null)
                    {
                        OnComplete();
                    }
                }
            });
    }

    public static void DeclineChatInvite(string challengeId, Action OnComplete = null, Action OnFail = null)
    {
        new DeclineChallengeRequest().SetChallengeInstanceId(challengeId)
            .Send((response) =>
            {
                if (response.HasErrors)
                {
                    if (OnFail != null)
                    {
                        OnFail();
                    }
                }
                else
                {
                    if (OnComplete != null)
                    {
                        OnComplete();
                    }
                }
            });
    }

    public static void SendMessage(string message)
    {
        new LogEventRequest().SetEventKey("ChatMessage")
            .SetEventAttribute("challengeId", Challenge.ChallengeId)
            .SetEventAttribute("message", message)
            .Send(null);
    }

    public static void LeaveChat(string challengeId)
    {
        new LogEventRequest().SetEventKey("LeaveChat")
            .SetEventAttribute("ChallengeId", challengeId)
            .Send(null);
    }

    public static void GetChatUsers(Action<List<string>> callback)
    {
        new LogEventRequest()
            .SetEventKey("GetChatUsers")
            .SetEventAttribute("ChallengeId", Challenge.ChallengeId)
            .Send(result =>
            {
                List<string> res =result.ScriptData.GetGSData("users").GetStringList("username");
                callback(res);
            });
    }
}
