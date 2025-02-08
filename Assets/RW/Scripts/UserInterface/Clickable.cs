using UnityEngine;
using System;
using UnityEngine.EventSystems;

namespace RW.MonumentValley
{
    [RequireComponent(typeof(Collider), typeof(AudioSource))]
    public class Clickable : MonoBehaviour, IPointerDownHandler
    {
        private Node[] childNodes;
        public Node[] ChildNodes => childNodes;

        public Action<Clickable, Vector3> clickAction;

        private AudioSource audioSource;

        [SerializeField] private AudioClip clickSound;

        private void Awake()
        {
            childNodes = GetComponentsInChildren<Node>();
            audioSource = GetComponent<AudioSource>();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (clickAction != null) {
                clickAction.Invoke(this, eventData.pointerPressRaycast.worldPosition);
            }

            PlayClickSound();
        }

        private void PlayClickSound()
        {
            if (clickSound != null && audioSource != null) {
                audioSource.PlayOneShot(clickSound);
            }
        }
    }
}
