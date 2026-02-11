using ASUL.Canvas;
using DG.Tweening;
using UnityEngine;

public class DialogueWindowLauncher : CanvasGroupLauncher
{
    [SerializeField] private float duration = 0.5f;
    [SerializeField] private Ease easeIn = Ease.OutBack;
    [SerializeField] private Ease easeOut = Ease.InBack;

    private RectTransform rect;
    private CanvasGroup cg;

    protected override void Awake()
    {
        cg = GetComponent<CanvasGroup>();
        rect = transform as RectTransform;

        base.Awake();

        if (!IsOpened)
        {
            rect.localScale = Vector3.zero;
            cg.alpha = 0;
            cg.interactable = false;
            cg.blocksRaycasts = false;
        }
    }

    public void ToggleDialogueOpen(bool value)
    {
        ToggleOpen(value);
    }

    protected override void Open()
    {
        rect.localScale = Vector3.zero;
        cg.alpha = 0;

        DOTween.Kill(this);

        rect.DOScale(1f, duration)
            .SetEase(easeIn)
            .SetUpdate(true);

        cg.DOFade(1f, duration * 0.8f)
            .SetEase(Ease.OutCubic)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                cg.interactable = true;
                cg.blocksRaycasts = true;
                IsOpened = true;
            });
    }

    protected override void Close()
    {
        DOTween.Kill(this);

        rect.DOScale(0f, duration)
            .SetEase(easeOut)
            .SetUpdate(true);

        cg.DOFade(0f, duration * 0.8f)
            .SetEase(Ease.InCubic)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                cg.interactable = false;
                cg.blocksRaycasts = false;
                IsOpened = false;
            });
    }
}