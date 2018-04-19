using UnityEngine;
using UnityEngine.UI;

namespace Borodar.FarlandSkies.Core.Demo
{
    public abstract class BaseColorButton : MonoBehaviour
    {
        public ColorPicker ColorPicker;
        protected Image ColorImage;

        //---------------------------------------------------------------------
        // Messages
        //---------------------------------------------------------------------

        protected void Awake()
        {
            ColorImage = GetComponent<Image>();
        }

        //---------------------------------------------------------------------
        // Public
        //---------------------------------------------------------------------

        public void OnClick()
        {
            ColorPicker.ColorButton = this;
            ColorPicker.gameObject.SetActive(true);
        }

        public virtual void ChangeColor(Color color)
        {
            ColorImage.color = color;
        }
    }
}