﻿using Cinemachine;
using UnityEngine;
using WG.GameX.Util;

namespace WG.GameX.Player
{
    public class PlayerCameraController : MonoBehaviour
    {
        [SerializeField] private CinemachineFreeLook _freeLookCamera;
        [SerializeField] private float _speedZoomMultipler;
        private float[] _initalOrbitRadii;
        [SerializeField] private Camera _mainCamera;

        public Camera CameraComponent => _mainCamera;

        private void Start()
        {
            _initalOrbitRadii = new float[_freeLookCamera.m_Orbits.Length];
            for (var i = 0; i < _freeLookCamera.m_Orbits.Length; i++)
            {
                _initalOrbitRadii[i] = _freeLookCamera.m_Orbits[i].m_Radius;
            }
        }

        public void Setup(Transform lookAtReferenceMiddle, Transform lookAtReferenceBottom)
        {
            _freeLookCamera.GetRig(0).m_LookAt = lookAtReferenceMiddle;
            _freeLookCamera.GetRig(1).m_LookAt = lookAtReferenceMiddle;
            _freeLookCamera.GetRig(2).m_LookAt = lookAtReferenceBottom;
        }

        public void UpdateCamera(float horizontal, float vertical, float mouseVertical, float speedFactor)
        {
            //_freeLookCamera.m_YAxis.Value = Mathf.Lerp(_freeLookCamera.m_YAxis.Value, 1 - (vertical + .5f), Time.deltaTime);
            var yInput = 0f;
            if (Mathf.Abs(vertical) > 0.01f)
                yInput = Mathf.Lerp(_freeLookCamera.m_YAxis.Value, 1 - (vertical + .5f), Time.deltaTime);
            else
                yInput = _freeLookCamera.m_YAxis.Value.GetSmoothDamping(mouseVertical, Time.deltaTime * 500);

            _freeLookCamera.m_YAxis.Value = yInput;
            _freeLookCamera.m_XAxis.Value = horizontal;

            for (var i = 0; i < _freeLookCamera.m_Orbits.Length; i++)
            {
                _freeLookCamera.m_Orbits[i].m_Radius =
                    _initalOrbitRadii[i] + (_initalOrbitRadii[i] * speedFactor * _speedZoomMultipler);
            }
        }

        private void OnDestroy()
        {
            for (var i = 0; i < _freeLookCamera.m_Orbits.Length; i++)
            {
                _freeLookCamera.m_Orbits[i].m_Radius = _initalOrbitRadii[i];
            }
        }
    }
}