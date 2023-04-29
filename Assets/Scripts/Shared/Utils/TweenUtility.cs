using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public static class TweenUtility
{
    public static void Kill(Tweener p_tween, bool p_complete)
    {
        if (p_tween != null && p_tween.IsActive())
        {
            p_tween.Kill(p_complete);
        }
    }

    public static void Kill(Sequence p_tween, bool p_complete)
    {
        if (p_tween != null && p_tween.IsActive())
        {
            p_tween.Kill(p_complete);
        }
    }

    public static Sequence DoJumpSequence(this RectTransform p_rect, float p_power = 0.2f, float p_duration = 1.52f, float p_jumpPower = 100f, RectTransform p_shadow = null)
    {
        Vector2 l_rectSize = p_rect.sizeDelta;
        Vector2 l_rectPos = p_rect.anchoredPosition;

        float l_squashTimePercent = 0.1f;
        float l_jumpTimePercent = 0.2f;
        float l_fallTimePercent = 0.1f;
        float l_punchTimePercent = 0.6f;
        Vector2 l_squashSize = new Vector2(l_rectSize.x * (1f + p_power), l_rectSize.y * (1f - p_power));
        Vector2 l_stretchSize = new Vector2(l_rectSize.x * (1f - p_power), l_rectSize.y * (1f + p_power));

        Sequence l_jumpSequence = DOTween.Sequence();

        // squash 1/19th
        l_jumpSequence.Append(p_rect.DOSizeDelta(l_squashSize, p_duration * l_squashTimePercent));

        // get jump duration
        float l_jumpDuration = p_duration * l_jumpTimePercent;

        // jump
        l_jumpSequence.Append(p_rect.DOAnchorPosY(p_rect.anchoredPosition.y + p_jumpPower, l_jumpDuration));

        if (p_shadow)
            // tween the shadow for a cool effect
            l_jumpSequence.Join(p_shadow.DOScale(Vector3.one * 0.5f, l_jumpDuration));

        // stretch for half the jump
        l_jumpSequence.Join(p_rect.DOSizeDelta(l_stretchSize, l_jumpDuration * 0.5f));

        // go normal for the rest
        l_jumpSequence.Join(p_rect.DOSizeDelta(l_rectSize, l_jumpDuration * 0.5f).SetDelay(l_jumpDuration * 0.5f));


        // get fall duration (about half the jump time)
        float l_fallDuration = p_duration * l_fallTimePercent;

        // fall
        l_jumpSequence.Append(p_rect.DOAnchorPos(l_rectPos, l_fallDuration).SetEase(Ease.InQuad));

        if (p_shadow)
            // tween the shadow back to normal
            l_jumpSequence.Join(p_shadow.DOScale(Vector3.one, l_fallDuration).SetEase(Ease.InQuad));

        // stretch for half the fall
        //l_jumpSequence.Join(p_rect.DOSizeDelta(l_stretchSize, l_fallDuration * 0.5f).SetEase(Ease.Linear));

        // go normal for the rest
        l_jumpSequence.Join(p_rect.DOSizeDelta(l_rectSize, l_fallDuration * 0.5f)
            .SetDelay(l_fallDuration * 0.5f).SetEase(Ease.Linear));

        // slight movement
        // recover wiggle
        l_jumpSequence.Append(p_rect.DOPunchScale(new Vector2(0.15f, -0.15f), p_duration * l_punchTimePercent, 5, 10f));

        l_jumpSequence.AppendInterval(1f);
        l_jumpSequence.SetLoops(-1, LoopType.Restart);
        l_jumpSequence.OnComplete(() =>
            {
                p_rect.anchoredPosition = l_rectPos;
                p_rect.sizeDelta = l_rectSize;
            });

        return l_jumpSequence;

    }

    public static Sequence DoJumpSequence(this Transform p_rect, float p_power = 0.2f, float p_duration = 1.52f, float p_jumpPower = 0.5f, RectTransform p_shadow = null)
    {
        Vector3 l_rectSize = p_rect.localScale;
        Vector3 l_rectPos = p_rect.position;

        float l_squashTimePercent = 0.1f;
        float l_jumpTimePercent = 0.2f;
        float l_fallTimePercent = 0.1f;
        float l_punchTimePercent = 0.6f;
        Vector3 l_squashSize = new Vector3(l_rectSize.x * (1f + p_power), l_rectSize.y * (1f - p_power), 1);
        Vector3 l_stretchSize = new Vector3(l_rectSize.x * (1f - p_power), l_rectSize.y * (1f + p_power), 1);

        Sequence l_jumpSequence = DOTween.Sequence();

        // squash 1/19th
        l_jumpSequence.Append(p_rect.DOScale(l_squashSize, p_duration * l_squashTimePercent));

        // get jump duration
        float l_jumpDuration = p_duration * l_jumpTimePercent;

        // jump
        l_jumpSequence.Append(p_rect.DOMoveY(p_rect.position.y + p_jumpPower, l_jumpDuration));

        if (p_shadow)
            // tween the shadow for a cool effect
            l_jumpSequence.Join(p_shadow.DOScale(Vector3.one * 0.5f, l_jumpDuration));

        // stretch for half the jump
        l_jumpSequence.Join(p_rect.DOScale(l_stretchSize, l_jumpDuration * 0.5f));

        // go normal for the rest
        l_jumpSequence.Join(p_rect.DOScale(l_rectSize, l_jumpDuration * 0.5f).SetDelay(l_jumpDuration * 0.5f));


        // get fall duration (about half the jump time)
        float l_fallDuration = p_duration * l_fallTimePercent;

        // fall
        l_jumpSequence.Append(p_rect.DOMove(l_rectPos, l_fallDuration).SetEase(Ease.InQuad));

        if (p_shadow)
            // tween the shadow back to normal
            l_jumpSequence.Join(p_shadow.DOScale(Vector3.one, l_fallDuration).SetEase(Ease.InQuad));

        // stretch for half the fall
        //l_jumpSequence.Join(p_rect.DOSizeDelta(l_stretchSize, l_fallDuration * 0.5f).SetEase(Ease.Linear));

        // go normal for the rest
        l_jumpSequence.Join(p_rect.DOScale(l_rectSize, l_fallDuration * 0.5f)
            .SetDelay(l_fallDuration * 0.5f).SetEase(Ease.Linear));

        // slight movement
        // recover wiggle
        l_jumpSequence.Append(p_rect.DOPunchScale(new Vector2(0.15f, -0.15f), p_duration * l_punchTimePercent, 5, 10f));

        l_jumpSequence.AppendInterval(1f);
        l_jumpSequence.SetLoops(-1, LoopType.Restart);
        l_jumpSequence.OnComplete(() =>
            {
                p_rect.position = l_rectPos;
                p_rect.localScale = l_rectSize;
            });

        return l_jumpSequence;

    }
}
