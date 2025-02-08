using UnityEngine;
using UnityEngine.EventSystems;

namespace RW.MonumentValley
{
    [RequireComponent(typeof(Collider))]
    public class Highlighter : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private MeshRenderer[] meshRenderers;

        [SerializeField] private string highlightProperty = "_IsHighlighted";

        private bool isEnabled;
        public bool IsEnabled { get { return isEnabled; } set { isEnabled = value; } }


        private void Start()
        {
            isEnabled = true;
            ToggleHighlight(false);
        }

        public void ToggleHighlight(bool onOff)
        {
            foreach (MeshRenderer meshRenderer in meshRenderers)
            {
                if (meshRenderer != null)
                {
                    meshRenderer.material.SetFloat(highlightProperty, onOff ? 1f : 0f);
                }   
            }
        }

        public void EnableHighlight(bool state)
        {
            isEnabled = state;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            ToggleHighlight(isEnabled);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            ToggleHighlight(false);
        }
    }
}