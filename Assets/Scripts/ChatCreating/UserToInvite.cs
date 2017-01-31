using UnityEngine;
using UnityEngine.UI;

public class UserToInvite : MonoBehaviour
{
    [SerializeField] private Text userName;
    public string Username;


    public void Init(string username)
    {
        userName.text = username;
        Username = username;
    }

    public void OnCancelClick()
    {
        Destroy(gameObject);
    }
}
