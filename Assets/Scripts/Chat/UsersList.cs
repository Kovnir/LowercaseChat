using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class UsersList : MonoBehaviour
{
    [SerializeField] private Text userListText;
    public static List<string> UserList = new List<string>();

    private bool stopCoroutine;

    [UsedImplicitly]
    private void OnEnable()
    {
        UserList = new List<string>();
        userListText.text = String.Empty;
        stopCoroutine = false;
        StartCoroutine(Updating());
    }


    private IEnumerator Updating()
    {
        while (!stopCoroutine)
        {
            NetworkManager.GetChatUsers(result =>
            {
                UserList = result;
                userListText.text = String.Empty;
                foreach (string name in result)
                {
                    userListText.text += name + "\n";
                }
            });
            yield return new WaitForSeconds(5);
        }
    }

    [UsedImplicitly]
    private void OnDisable()
    {
        stopCoroutine = true;
    }
}
