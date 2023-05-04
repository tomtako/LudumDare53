using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class WheelTrail : MonoBehaviour
    {
        private CarController m_controller;
        private TrailRenderer m_trailRenderer;

        private FMOD.Studio.EventInstance playerBrake;
        private bool playerBrakeSwitch = true;

        private bool m_failedAudioLoad;
        private void Awake()
        {
            m_controller = GetComponentInParent<CarController>();
            m_trailRenderer = GetComponent<TrailRenderer>();

            m_trailRenderer.emitting = false;


        }

        private void Start()
        {
            LoadAudio();
        }

        private void LoadAudio()
        {
            try
            {
                playerBrake = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/brake");

                m_failedAudioLoad = false;
            }
            catch (Exception e)
            {
                m_failedAudioLoad = true;
            }
        }

        private void Update()
        {
            if (m_controller.IsTireScreeching(out float lateralVelocity, out bool isBreaking))
            {
                m_trailRenderer.emitting = true;
                if (playerBrakeSwitch)
                {
                    playerBrake.start();
                    playerBrakeSwitch = false;
                }
            }
            else
            {
                m_trailRenderer.emitting = false;
                playerBrake.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                playerBrakeSwitch = true;
            }
        }
    }
}