using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

namespace RW.MonumentValley
{
    [RequireComponent(typeof(AudioSource))]
    public class GameManager : MonoBehaviour
    {
        private PlayerController playerController;
        private bool isGameOver;
        public bool IsGameOver => isGameOver;

        public float delayTime = 2f;

        public UnityEvent awakeEvent;
        public UnityEvent initEvent;
        public UnityEvent endLevelEvent;

        private AudioSource audioSource;

        [SerializeField] private AudioClip backgroundMusic;
        [SerializeField] private AudioClip winSound;

        private void Awake()
        {
            awakeEvent.Invoke();

            playerController = FindObjectOfType<PlayerController>();
            audioSource = GetComponent<AudioSource>();

            PlayBackgroundMusic();
        }

        private void Start()
        {
            initEvent.Invoke();
        }

        private void Update()
        {
            if (playerController != null && playerController.HasReachedGoal())
            {
                Win();
            }
        }

        private void Win()
        {
            if (isGameOver || playerController == null)
            {
                return;
            }
            isGameOver = true;

            playerController.EnableControls(false);

            PlayWinSound();
            StartCoroutine(WinRoutine());
        }

        private IEnumerator WinRoutine()
        {
            if (endLevelEvent != null)
                endLevelEvent.Invoke();

            yield return new WaitForSeconds(delayTime);
        }

        public void Restart(float delay)
        {
            StartCoroutine(RestartRoutine(delay));
        }

        private IEnumerator RestartRoutine(float delay)
        {
            yield return new WaitForSeconds(delay);

            Scene activeScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(activeScene.buildIndex);
        }

        private void PlayBackgroundMusic()
        {
            if (backgroundMusic != null) {
                audioSource.clip = backgroundMusic;
                audioSource.loop = true;
                audioSource.volume = 0.5f;
                audioSource.Play();
            }
        }

        private void PlayWinSound()
        {
            if (winSound != null) {
                audioSource.PlayOneShot(winSound);
            }
        }
    }
}

