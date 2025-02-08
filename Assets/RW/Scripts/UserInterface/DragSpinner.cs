using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace RW.MonumentValley
{

    [RequireComponent(typeof(Collider))]
    public class DragSpinner : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public enum SpinAxis
        {
            X,
            Y,
            Z
        }

        [SerializeField] private Transform targetToSpin;

        [SerializeField] private SpinAxis spinAxis = SpinAxis.X;

        [SerializeField] private Transform pivot;

        [SerializeField] private int minDragDist = 10;

        private Vector2 directionToMouse;

        private bool isSpinning;

        private bool isActive;

        private float angleToMouse;

        private float previousAngleToMouse;

        private Vector3 axisDirection;

        public UnityEvent snapEvent;

        private float timeCount;

        void Start()
        {
            switch (spinAxis)
            {
                case (SpinAxis.X):
                    axisDirection = Vector3.right;
                    break;
                case (SpinAxis.Y):
                    axisDirection = Vector3.up;
                    break;
                case (SpinAxis.Z):
                    axisDirection = Vector3.forward;
                    break;
            }
            EnableSpinner(true);
        }

        public void OnBeginDrag(PointerEventData data)
        {
            if (!isActive)
            {
                return;
            }

            isSpinning = true;

            Vector3 inputPosition = new Vector3(data.position.x, data.position.y, 0f);
            directionToMouse = inputPosition - Camera.main.WorldToScreenPoint(pivot.position);

            previousAngleToMouse = Mathf.Atan2(directionToMouse.y, directionToMouse.x) * Mathf.Rad2Deg;

        }

        public void OnEndDrag(PointerEventData data)
        {
            if (isActive)
            {
                SnapSpinner();
            }
        }

        public void OnDrag(PointerEventData data)
        {
            if (isSpinning && Camera.main != null && pivot != null && isActive)
            {
                Vector3 inputPosition = new Vector3(data.position.x, data.position.y, 0f);
                directionToMouse = inputPosition - Camera.main.WorldToScreenPoint(pivot.position);
                angleToMouse = Mathf.Atan2(directionToMouse.y, directionToMouse.x) * Mathf.Rad2Deg;

                if (directionToMouse.magnitude > minDragDist)
                {
                    Vector3 newRotationVector = (previousAngleToMouse - angleToMouse) * axisDirection;
                    targetToSpin.Rotate(newRotationVector);
                    previousAngleToMouse = angleToMouse;
                }
            }
        }

        private void SnapSpinner()
        {
            isSpinning = false;

            RoundToRightAngles(targetToSpin);

            if (snapEvent != null)
            {
                snapEvent.Invoke();
            }
        }

        private void RoundToRightAngles(Transform xform)
        {
            float roundedXAngle = Mathf.Round(xform.eulerAngles.x / 90f) * 90f;
            float roundedYAngle = Mathf.Round(xform.eulerAngles.y / 90f) * 90f;
            float roundedZAngle = Mathf.Round(xform.eulerAngles.z / 90f) * 90f;

            xform.eulerAngles = new Vector3(roundedXAngle, roundedYAngle, roundedZAngle);
        }

        public void EnableSpinner(bool state)
        {
            isActive = state;

            if (!isActive)
            {
                SnapSpinner();
            }
        }


    }

}