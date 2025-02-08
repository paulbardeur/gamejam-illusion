using UnityEngine;
using System; 
using UnityEngine.EventSystems;

namespace RW.MonumentValley
{
    [RequireComponent(typeof(Collider))]
    public class Clickable : MonoBehaviour,IPointerDownHandler
    { 
        private Node[] childNodes;
        public Node[] ChildNodes => childNodes;

        public Action<Clickable,Vector3> clickAction;

        private void Awake()
        {
            childNodes = GetComponentsInChildren<Node>();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (clickAction != null)
            {
                clickAction.Invoke(this, eventData.pointerPressRaycast.worldPosition);
            }
        }
    }
}