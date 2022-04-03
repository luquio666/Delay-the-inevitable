using System;
using System.Collections;
using System.Collections.Generic;
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
    public float KnockedOutTime = 2f;

    private BodyPart _hips;
    private Vector3 _currentDirection;
    private float _normalizedSpeed;
    private bool _isBalancing = true;


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

        _currentDirection = Vector3.forward;
    }

    private void OnDestroy()
    {
        CollisionDetector.OnRagdollCollisionEnter -= OnRagdollCollisionEnter;
    }

    private void Update()
    {
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
            _hips.Balance(UprightTorque, RotationTorque, _currentDirection);
            _hips.Rigidbody.velocity = _currentDirection * _normalizedSpeed * Speed;
        }
        
        Vector3 position = _hips.Rigidbody.transform.position;
        position.y -= _hips.Rigidbody.transform.localPosition.y;
        PositionMarker.position = position;
        PositionMarker.rotation = Quaternion.LookRotation(_currentDirection);
    }
    
    private void OnRagdollCollisionEnter(Collision collision)
    {
        Debug.Log("collision magnitude: " + collision.relativeVelocity.magnitude);
        if(collision.relativeVelocity.magnitude > KnockingHitMagnitudeThreshold)
        {
            _isBalancing = false;
            CancelInvoke();
            Invoke("EnableBalancing", KnockedOutTime);
        }
        
    }
    
    private void EnableBalancing()
    {
        _isBalancing = true;
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