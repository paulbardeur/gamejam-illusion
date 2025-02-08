using UnityEngine;

namespace RW.MonumentValley
{
    [RequireComponent(typeof(Animator))]
    public class Cursor : MonoBehaviour
    {
        [SerializeField] private float offsetDistance = 1f;

        private Camera cam;

        private Animator animController;

        private void Awake()
        {
            if (cam == null)
            {
                cam = Camera.main;
            }
            animController = GetComponent<Animator>();
        }

        void LateUpdate()
        {
            if (cam != null)
            {
                Vector3 cameraForward = cam.transform.rotation * Vector3.forward;
                Vector3 cameraUp = cam.transform.rotation * Vector3.up;

                transform.LookAt(transform.position + cameraForward, cameraUp);
            }
        }

        public void ShowCursor(Vector3 position)
        {
            if (cam != null && animController != null)
            {
                Vector3 cameraForwardOffset = cam.transform.rotation * new Vector3(0f, 0f, offsetDistance);
                transform.position = position - cameraForwardOffset;

                animController.SetTrigger("ClickTrigger");
            }
        }
    }

}