using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
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

        [SerializeField] private Transform wheel;

        [SerializeField] private SpinAxis spinAxis = SpinAxis.X;


        [SerializeField] private Transform pivot;

        [SerializeField] private int minDragDist = 10;

        [SerializeField] private float rotationDuration = 0.5f;

        private Vector2 directionToMouse;
        private bool isSpinning;
        private bool isActive;
        private float angleToMouse;
        private float previousAngleToMouse;
        private Vector3 axisDirection;
        public UnityEvent snapEvent;
        private bool keyState;
        private Coroutine rotationCoroutine;

        void Start()
        {
            switch (spinAxis)
            {
                case SpinAxis.X:
                    axisDirection = Vector3.right;
                    break;
                case SpinAxis.Y:
                    axisDirection = Vector3.up;
                    break;
                case SpinAxis.Z:
                    axisDirection = Vector3.forward;
                    break;
            }
            EnableSpinner(true);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.R)) {
                keyState = !keyState;
                if (rotationCoroutine == null) {
                    rotationCoroutine = StartCoroutine(RotatePressed(keyState ? 90f : 270f));
                }
            }
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

        private IEnumerator RotatePressed(float targetAngle)
        {
            float elapsedTime = 0f;
            Quaternion startRotation = targetToSpin.rotation;
            Quaternion targetRotation = Quaternion.Euler(targetToSpin.eulerAngles + (axisDirection * targetAngle));

            Quaternion startWheelRotation = wheel ? wheel.rotation : Quaternion.identity;
            Quaternion targetWheelRotation = wheel ? Quaternion.Euler(wheel.eulerAngles + (axisDirection * targetAngle)) : Quaternion.identity;

            while (elapsedTime < rotationDuration)
            {
                float t = elapsedTime / rotationDuration;
                targetToSpin.rotation = Quaternion.Slerp(startRotation, targetRotation, t);

                if (wheel)
                {
                    wheel.rotation = Quaternion.Slerp(startWheelRotation, targetWheelRotation, t);
                }

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            targetToSpin.rotation = targetRotation;
            if (wheel)
            {
                wheel.rotation = targetWheelRotation;
            }

            RoundToRightAngles(targetToSpin);

            if (snapEvent != null)
            {
                snapEvent.Invoke();
            }

            rotationCoroutine = null;
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
