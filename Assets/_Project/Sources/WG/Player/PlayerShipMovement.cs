﻿using UnityEngine;
using WG.GameX.Util;

using System;
using System.Collections.Generic;
using UnityEngine.Events;
using WG.GameX.Enemy;

namespace WG.GameX.Player
{
    public class PlayerShipMovement
    {
        private readonly Transform _shipTransform;
        private readonly Transform _pivotTransform;
        private readonly float _minSpeed;
        private readonly float _maxSpeed;
        private readonly float _accelaration;
        private readonly Rigidbody _rigidBody;
        private float _forwardSpeed;
        private float _lerpedForwardMovementSpeed;
		private float horizontalValueMax = 0.03f;

        public float SpeedFactor => _forwardSpeed / _maxSpeed;

        public PlayerShipMovement(Transform shipTransform, Transform pivotTransform,
            float minSpeed, float maxSpeed, float accleration, Rigidbody rigidBody)
        {
            _shipTransform = shipTransform;
            _minSpeed = minSpeed;
            _maxSpeed = maxSpeed;
            _accelaration = accleration;
            _rigidBody = rigidBody;
            _pivotTransform = pivotTransform;
            _lerpedForwardMovementSpeed = minSpeed;
        }

        public void Update(float mouseScroll, float horizontalValue)
        {
            // Rotation
			horizontalValue = Mathf.Max(Mathf.Min(horizontalValue, horizontalValueMax),(-1)*horizontalValueMax);
            _shipTransform.Rotate(Vector3.up * horizontalValue);
            _lerpedForwardMovementSpeed += mouseScroll;
            _lerpedForwardMovementSpeed = _lerpedForwardMovementSpeed.Clamp(_minSpeed, _maxSpeed);
            _forwardSpeed = _forwardSpeed.GetLinearDamping(_lerpedForwardMovementSpeed, _accelaration);
            var position = Vector3.Normalize(_pivotTransform.forward);
            //_shipTransform.position += position * _forwardSpeed;
            _rigidBody.velocity = (position * _forwardSpeed * 100);
        }

        public void StopMovement()
        {
            _forwardSpeed = 0;
        }
    }
}