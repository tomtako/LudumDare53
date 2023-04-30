using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DefaultNamespace
{
    public class TimesUpScreen : MonoBehaviour
    {
        public GameObject pressAnyButtonText;
        public float blinkDelay = 2;
        public float blinkOutDelay = 0.24f;

        private float blinkTimer;
        private bool blink;

        public event Action OnContinue;

        private void Update()
        {
            blinkTimer -= Time.deltaTime;

            if (blinkTimer <= 0)
            {
                blink = !blink;
                pressAnyButtonText.SetActive(blink);
                blinkTimer = blink ? blinkDelay : blinkOutDelay;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                OnContinue?.Invoke();
            }
        }
    }
}