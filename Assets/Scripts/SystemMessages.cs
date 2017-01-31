using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class SystemMessages : Singleton<SystemMessages>
{
    [SerializeField]
    private Text text;
    private CanvasGroup canvasGroup;
    private Tween tween;

    [UsedImplicitly]
    protected override void Awake()
    {
        base.Awake();
        canvasGroup = GetComponent<CanvasGroup>();
    }  

    public static void ShowError(string error)
    {
        Instance.text.text = "<color=#EC4F58FF>" + error + "</color>";
        Instance.Show();
    }

    private void Show()
    {
        canvasGroup.blocksRaycasts = true;
        tween.Kill();
        canvasGroup.alpha = 0;
        tween = canvasGroup.DOFade(0.8f, 1f).OnComplete(() =>
        {
            tween = canvasGroup.DOFade(0, 1f).OnComplete(() =>
            {
                canvasGroup.blocksRaycasts = false;
            });
        });
    }

    public static void ShowMessage(string message)
    {
        Instance.text.text = "<color=#3B5757FF>" + message + "</color>";
        Instance.Show();
    }
}
