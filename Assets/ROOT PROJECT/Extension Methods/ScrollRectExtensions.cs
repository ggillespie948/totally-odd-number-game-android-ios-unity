using UnityEngine;
using UnityEngine.UI;
public static class ScrollRectExtensions
{
    public static void ScrollToTop(this ScrollRect scrollRect)
    {
        scrollRect.normalizedPosition = new Vector2(0, 1);
    }
    public static void ScrollToBottom(this ScrollRect scrollRect)
    {
        scrollRect.normalizedPosition = new Vector2(0, 0);
    }

	public static void ScrollToLeft(this ScrollRect scrollRect)
    {
        scrollRect.normalizedPosition = new Vector2(1, 0);
    }

	public static void ScrollToRight(this ScrollRect scrollRect)
    {
        scrollRect.normalizedPosition = new Vector2(0, 1);
    }

	public static Vector2 GetSnapToPositionToBringChildIntoView(this ScrollRect instance, RectTransform child)
	{
		Canvas.ForceUpdateCanvases();
		Vector2 viewportLocalPosition = instance.viewport.localPosition;
		Vector2 childLocalPosition   = child.localPosition;
		Vector2 result = new Vector2(
			0 - (viewportLocalPosition.x + childLocalPosition.x),
			0 - (viewportLocalPosition.y + childLocalPosition.y)
		);
		return result;
	}

	public static Vector2 GetSnapToPositionToBringChildIntoViewVertical(this ScrollRect instance, RectTransform child)
	{
		Canvas.ForceUpdateCanvases();
		Vector2 viewportLocalPosition = instance.viewport.localPosition;
		Vector2 childLocalPosition   = child.localPosition;
		Vector2 result = new Vector2(
			0-(viewportLocalPosition.x),
			0-(viewportLocalPosition.y)
		);
		return result;
	}

	public static void SnapToPositionVertical(this ScrollRect stageScrollRect, RectTransform target, RectTransform contentRect, Vector3 padding)
    {
        Canvas.ForceUpdateCanvases();
        Vector2 pos =
            (Vector2)stageScrollRect.transform.InverseTransformPoint(contentRect.position)
            - (Vector2)stageScrollRect.transform.InverseTransformPoint(target.position + padding);
        pos.x = contentRect.anchoredPosition.x;
        contentRect.anchoredPosition = pos;
    }

	
}
