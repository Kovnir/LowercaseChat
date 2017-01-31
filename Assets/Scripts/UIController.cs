using System;
using DG.Tweening;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class UIController : Singleton<UIController>
{
    [SerializeField] private CanvasGroup LoginPanel;
    [SerializeField] private CanvasGroup RegistrationPanel;
    [SerializeField]
    private CanvasGroup ChatListPanel;
    [SerializeField]
    private CanvasGroup ChatCreatePanel;
    [SerializeField] private CanvasGroup ChatPanel;

    private const float ANIMATION_TIME = 0.5f;

    private CanvasGroup currentPanel;

    private GraphicRaycaster graphicRaycaster;

    [UsedImplicitly]
	protected override void Awake ()
    {
        base.Awake();
        graphicRaycaster = GetComponent<GraphicRaycaster>();
    }

	[UsedImplicitly]
	private void Start ()
	{
        ShowLogin();
	}

    public void ShowLogin()
    {
        UpdatedView(LoginPanel);
    }
    public void ShowRegistration()
    {
        UpdatedView(RegistrationPanel);
    }
    public void ShowChatListWindow()
    {
        UpdatedView(ChatListPanel);
    }
    public void ShowChatCreateWindow()
    {
        UpdatedView(ChatCreatePanel);
    }
    public void ShowChat()
    {
        UpdatedView(ChatPanel);
    }

    private void UpdatedView(CanvasGroup panel)
    {
        if (currentPanel != null)
        {
            UpdatePanel(currentPanel, false, () => UpdatePanel(panel));
        }
        else
        {
            UpdatePanel(panel);
        }
        currentPanel = panel;
    }
    private void UpdatePanel(CanvasGroup panel, bool show = true, Action OnComplete = null)
    {
        if (!show)
        {
            graphicRaycaster.enabled = false;
        }
        panel.alpha = show ? 0 : 1;
        if (show)
        {
            panel.gameObject.SetActive(true);
        }
        panel.DOFade(show? 1 : 0, ANIMATION_TIME).OnComplete(
            () =>
            {
                if (!show)
                {
                    panel.gameObject.SetActive(false);
                }
                else
                {
                    graphicRaycaster.enabled = true;
                }
                if (OnComplete != null)
                {
                    OnComplete();
                }
            });

    }

    public void BlockInput()
    {
        graphicRaycaster.enabled = false;
    }
    public void UnblockInput()
    {
        graphicRaycaster.enabled = true;
    }
}
