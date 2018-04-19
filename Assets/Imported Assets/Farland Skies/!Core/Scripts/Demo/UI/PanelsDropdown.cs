using UnityEngine;
using UnityEngine.UI;

namespace Borodar.FarlandSkies.Core.Demo
{
    public class PanelsDropdown : MonoBehaviour
    {
        [SerializeField]
        protected GameObject[] Panels;
        private Dropdown _dropdown;

        public void Awake()
        {
            _dropdown = GetComponent<Dropdown>();
        }

        public void OnValueChanged()
        {
            var value = _dropdown.value;
            for (var i = 0; i < Panels.Length; i++)
            {
                Panels[i].SetActive(i == value);
            }
        }
    }
}