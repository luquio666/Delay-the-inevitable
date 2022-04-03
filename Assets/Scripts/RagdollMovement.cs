using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class RagdollMovement : MonoBehaviour
{
    public BodyPart[] BodyParts;
    public Animator Animator;
    public Transform PositionMarker;
    public float UprightTorque = 10000f;
    public float RotationTorque = 500f;
    public float AccelerationDuration = 1f;
    public float Speed = 2f;
    public float RotationSpeed = 90;
    public float KnockingHitMagnitudeThreshold = 1f;
    public float KnockedOutDuration = 2f;
    public float GetUpDuration = 1f;
    [Range(0f, 2f)] public float DizzinessIntensity = 1f;
    public float DizzinessFrecuency = 1f;

    public float Dizziness => _dizziness;

    private BodyPart _hips;
    private Vector3 _currentDirection;
    private float _normalizedSpeed;
    private bool _isBalancing => _balanceMultiplier > 0.01f;
    private float _balanceMultiplier = 1f;
    private Tween _enableBalanceTween;
    private float _dizziness = 0f;


    private void Awake()
    {
        CollisionDetector.OnRagdollCollisionEnter += OnRagdollCollisionEnter;

        for (int i = 0; i < BodyParts.Length; i++)
        {
            BodyParts[i].Initialize();
            if (BodyParts[i].Name == "Hips")
            {
                _hips = BodyParts[i];
            }
        }

        _currentDirection = this.transform.forward;
    }

    private void OnDestroy()
    {
        CollisionDetector.OnRagdollCollisionEnter -= OnRagdollCollisionEnter;
    }

    private void Update()
    {
        if (!_isBalancing)
        {
            return;
        }

        if (Input.GetKey(KeyCode.W))
        {
            _normalizedSpeed = Mathf.Min(_normalizedSpeed + 1 / AccelerationDuration * Time.deltaTime, 1f);
        }
        else
        {
            _normalizedSpeed = Mathf.Max(_normalizedSpeed - 1 / AccelerationDuration * Time.deltaTime, 0f);
        }

        Animator.SetFloat("Speed", _normalizedSpeed);

        float horizontalAxis = 0f;
        if (Input.GetKey(KeyCode.A)) horizontalAxis -= 1f;
        if (Input.GetKey(KeyCode.D)) horizontalAxis += 1f;
        
        _dizziness = Mathf.PerlinNoise(Time.time * DizzinessFrecuency, 0f) - 0.5f;
        float finalDizziness = DizzinessIntensity * _dizziness;
        horizontalAxis += finalDizziness;
        
        _currentDirection = Quaternion.AngleAxis(horizontalAxis * RotationSpeed * Time.deltaTime, Vector3.up) *
                            _currentDirection;
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < BodyParts.Length; i++)
        {
            BodyParts[i].Sync();
        }

        if (_isBalancing)
        {
            _hips.Balance(UprightTorque * _balanceMultiplier, RotationTorque, _currentDirection);
            _hips.Rigidbody.velocity = _currentDirection * _normalizedSpeed * Speed;
        }

        Vector3 position = _hips.Rigidbody.transform.position;
        position.y -= _hips.Rigidbody.transform.localPosition.y;
        PositionMarker.position = position;
        PositionMarker.rotation = Quaternion.LookRotation(_currentDirection);
    }

    private void OnRagdollCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude > KnockingHitMagnitudeThreshold)
        {
            _balanceMultiplier = 0f;
            _normalizedSpeed = 0f;
            Animator.SetFloat("Speed", _normalizedSpeed);
            _enableBalanceTween?.Kill();
            _enableBalanceTween = DOVirtual.DelayedCall(KnockedOutDuration, EnableBalancing);

            Debug.Log(("collision magnitude: " + collision.relativeVelocity.magnitude).Color(Color.green));
        }
    }

    private void EnableBalancing()
    {
        _enableBalanceTween?.Kill();
        _enableBalanceTween = DOTween.To(() => _balanceMultiplier, x => _balanceMultiplier = x, 1f, GetUpDuration);
    }

    public void Grab(Rigidbody objectRigidbody)
    {
        objectRigidbody.isKinematic = true;
        objectRigidbody.transform.position = PositionMarker.position + PositionMarker.forward * 3f + Vector3.up * 1f;
        objectRigidbody.transform.SetParent(PositionMarker);
    }

    public void Drop(Rigidbody objectRigidbody)
    {
        objectRigidbody.isKinematic = false;
        objectRigidbody.transform.SetParent(null);
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(_hips.Rigidbody.transform.position,
            _hips.Rigidbody.transform.position + _currentDirection * 2f);
    }

    [System.Serializable]
    public class BodyPart
    {
        public string Name;
        public Transform AnimatedTransform;
        public ConfigurableJoint Joint;
        public Rigidbody Rigidbody;

        private Quaternion _initialJointRotation;
        private bool _shouldSync;

        public void Initialize()
        {
            _shouldSync = Joint != null;
            if (_shouldSync)
            {
                _initialJointRotation = Joint.transform.localRotation;
            }
        }

        public void Sync()
        {
            if (_shouldSync)
                ConfigurableJointExtensions.SetTargetRotationLocal(Joint, AnimatedTransform.localRotation,
                    _initialJointRotation);
        }

        public void Balance(float uprightTorque, float rotationTorque, Vector3 targetDirection)
        {
            var rot = Quaternion.FromToRotation(Rigidbody.transform.up,
                Vector3.up).normalized;

            Rigidbody.AddTorque(new Vector3(rot.x, rot.y, rot.z)
                                * uprightTorque);

            var directionAnglePercent = Vector3.SignedAngle(Rigidbody.transform.forward,
                targetDirection, Vector3.up) / 180;
            Rigidbody.AddRelativeTorque(0, directionAnglePercent * rotationTorque, 0);
        }
    }
}