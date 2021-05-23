using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public static class UIExtension {

    public static void SetAlpha(this Image image, float alpha) {
        Color color = image.color;
        color.a = alpha;
        image.color = color;
    }

    public static void ForceUpdateLayout(this HorizontalLayoutGroup layoutGroup) {
        layoutGroup.SetLayoutHorizontal();
        LayoutRebuilder.ForceRebuildLayoutImmediate(layoutGroup.GetComponent<RectTransform>());
    }

    public static void ForceUpdateLayout(this VerticalLayoutGroup layoutGroup) {
        layoutGroup.SetLayoutVertical();
        LayoutRebuilder.ForceRebuildLayoutImmediate(layoutGroup.GetComponent<RectTransform>());
    }
    
    public static void ScrollTo(this ScrollRect scrollRect, Transform target, bool scrollY = true, float duration = .2f) {
        Canvas.ForceUpdateCanvases();
        Transform viewport = scrollRect.viewport == null ? scrollRect.transform : scrollRect.viewport;
        Vector2 screenSize = ((RectTransform)viewport).rect.size;
        Vector2 targetPosition = scrollRect.transform.InverseTransformPoint(scrollRect.content.position) - scrollRect.transform.InverseTransformPoint(target.position);
        if (scrollY) {
            targetPosition -= .5f * screenSize;
            targetPosition.y = Mathf.Clamp(targetPosition.y, 0.0f, scrollRect.content.rect.height - screenSize.y);
        } else {
            targetPosition += .5f * screenSize;
            targetPosition.x = Mathf.Clamp(targetPosition.x, -scrollRect.content.rect.width + screenSize.x, 0.0f);
        }

        if (duration == 0.0f) {
            scrollRect.content.DOKill();
            Vector2 anchorPos = scrollRect.content.anchoredPosition;
            if (scrollY) {
                anchorPos.y = targetPosition.y;
            } else {
                anchorPos.x = targetPosition.x;
            }
            scrollRect.content.anchoredPosition = anchorPos;
        } else {
            if (scrollY) {
                scrollRect.content.DOAnchorPosY(targetPosition.y, duration)
                    .SetEase(Ease.InOutSine);
            } else {
                scrollRect.content.DOAnchorPosX(targetPosition.x, duration)
                    .SetEase(Ease.InOutSine);
            }
        }
    }

}
