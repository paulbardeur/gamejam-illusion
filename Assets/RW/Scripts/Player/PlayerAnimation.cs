using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RW.MonumentValley
{
    public class PlayerAnimation : MonoBehaviour
    {

        [Range(0.5f, 3f)]
        [SerializeField] private float walkAnimSpeed = 1f;

        [SerializeField] private Animator animator;


        void Start()
        {
            if (animator != null)
            {
                animator.SetFloat("walkSpeedMultiplier", walkAnimSpeed);
            }
        }

        public void ToggleAnimation(bool state)
        {
            if (animator != null)
            {
                animator?.SetBool("isMoving", state);
            }

        }
    }
}