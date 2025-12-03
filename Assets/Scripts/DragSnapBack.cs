using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class DragSnapBack : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;
    private CanvasGroup canvasGroup;

    private Vector2 originalPosition;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();

        // Get the parent canvas
        canvas = GetComponentInParent<Canvas>();

        // Save start position
        originalPosition = rectTransform.anchoredPosition;

        canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Optionally: bring to front
        transform.SetAsLastSibling();
        gameObject.GetComponent<Animator>().enabled = false;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Tween back to original position (if you want DOTween)
        rectTransform.DOAnchorPos(originalPosition, 0.3f).SetEase(Ease.OutQuad);
        gameObject.GetComponent<Animator>().enabled = true;
        canvasGroup.blocksRaycasts = true;
    }
}
