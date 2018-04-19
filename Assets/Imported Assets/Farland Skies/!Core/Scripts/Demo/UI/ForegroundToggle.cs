using UnityEngine;

namespace Borodar.FarlandSkies.Core.Demo
{
    public class ForegroundToggle : MonoBehaviour
    {
        public GameObject[] GameObjects;
        public Renderer[] Renderers;

        public void OnValueChanged(bool value)
        {
            foreach (var currentObject in GameObjects) currentObject.SetActive(value);
            foreach (var currentRenderer in Renderers) currentRenderer.enabled = value;
        }
    }
}