using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Borodar.FarlandSkies.Core.Demo
{
    public class ColorPicker : MonoBehaviour, IDragHandler, IEndDragHandler, IPointerClickHandler
    {
        private RectTransform _rectTransform;
        private Image _image;

        //---------------------------------------------------------------------
        // Properties
        //---------------------------------------------------------------------

        public BaseColorButton ColorButton { get; set; }

        //---------------------------------------------------------------------
        // Messages
        //---------------------------------------------------------------------

        public void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _image = GetComponent<Image>();
        }

        //---------------------------------------------------------------------
        // Interfaces
        //---------------------------------------------------------------------

        public void OnDrag(PointerEventData eventData)
        {
            OnPickColor(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            gameObject.SetActive(false);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnPickColor(eventData);
            gameObject.SetActive(false);
        }

        //---------------------------------------------------------------------
        // Helpers
        //---------------------------------------------------------------------

        private void OnPickColor(PointerEventData eventData)
        {
            Vector2 localCursor;
            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(_rectTransform, eventData.position, eventData.pressEventCamera, out localCursor)) return;

            var xpos = (int)(_rectTransform.rect.width + localCursor.x);
            var ypos = (int)(_rectTransform.rect.height + localCursor.y);
            var color = _image.sprite.texture.GetPixel(xpos, ypos);

            ColorButton.ChangeColor(color);
        }
    }
}