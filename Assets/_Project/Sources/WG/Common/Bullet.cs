﻿using UnityEngine;
using WG.GameX.Util;

namespace WG.GameX.Common
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private float _selfDestructTime;
        [SerializeField] private BulletExplode _bulletExplode;
        private Rigidbody _rigidbody;
        private LineRenderer _lineRenderer;
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _lineRenderer = GetComponentInChildren<LineRenderer>();
            _lineRenderer.enabled = false;
        }
        
        private void OnEnable()
        {
            Invoke(nameof(SelfDestruct), _selfDestructTime);
        }

        public void Fire(Vector3 bulletVelocity)
        {
            _lineRenderer.enabled = true;
            _rigidbody.isKinematic = false;
            _rigidbody.velocity = bulletVelocity;
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log($"bullet hit {other}");
            if (other.CompareTag(Constants.EnemyTag))
            {
                _lineRenderer.enabled = false;
                _bulletExplode.Play();
                _rigidbody.isKinematic = true;
                Invoke(nameof(SelfDestruct), 1);   
            }
        }

        private void SelfDestruct()
        {
            gameObject.SetActive(false);
        }
    }
}