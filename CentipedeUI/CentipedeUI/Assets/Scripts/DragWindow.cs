using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class DragWindow : MonoBehaviour, IDragHandler, IPointerDownHandler
{
    [SerializeField] private RectTransform dragRectTransform;
    [SerializeField] private Canvas canvas;

    public void OnDrag(PointerEventData eventData)
    {
        int y = SceneManager.GetActiveScene().buildIndex;

        if (y == 1)
        {
            if (MainMenu.Instance.isPaused)
            {
                dragRectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
            }            
        }
        else
        {
            dragRectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }        
    }

    public void OnPointerDown(PointerEventData eventData) => dragRectTransform.SetAsLastSibling();
}
