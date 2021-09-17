using System;
using System.Collections;
using System.Collections.Generic;
using Hands;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using PopovRadio.Scripts.Tools;

namespace PopovRadio.Scripts.Gameplay.Hands
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(ActionBasedController))]
    public class PhysicsPoser : MonoBehaviour
    {
        #region PhysicValues

        [Tooltip("Радиус проверки на находжение коллайдеров")] [SerializeField]
        private float physicsRange = .1f;

        [Tooltip("Маска для определения коллайдеров, взаимодействующих с руками")] [SerializeField]
        private LayerMask physicsMask = 0;

        [Tooltip("Коэфициент замедления скорости при физическом взаимодействии")] [SerializeField] [Range(0, 1)]
        private float slowDownVelocity = .75f;

        [Tooltip("Коэфициент замедления угловой скорости при физическом взаимодействии")] [SerializeField] [Range(0, 1)]
        private float slowDownAngularVelocity = .75f;

        [Tooltip("Максимальное изменение позиции за один фрейм")] [SerializeField] [Range(0, 100)]
        private float maxPositionChange = 75f;

        [Tooltip("Максимальное изменение поворота за один фрейм")] [SerializeField] [Range(0, 100)]
        private float maxRotationChange = 75f;

        #endregion

        #region References

        private Rigidbody _rigidbody;
        private XRBaseInteractor _interactor;
        private ActionBasedController _controller;
        private Collider[] _handColliders;
        private Transform _parentTransform;

        #endregion

        #region ControllerTransformInfo

        private Vector3 _targetPos = Vector3.zero;
        private Quaternion _targetRot = Quaternion.identity;

        #endregion

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _interactor = GetComponent<XRBaseInteractor>();
            _controller = GetComponent<EmptyActionBasedController>();
            _handColliders = transform.GetComponentInChildren<GameHand>(true).GetComponentsInChildren<Collider>(true);
            _parentTransform = transform.GetComponentInParent<XRRig>().transform;
        }

        private void OnEnable()
        {
            _interactor.selectEntered.AddListener(DisableHandColliders);
            _interactor.selectExited.AddListener(EnableHandColliders);
        }

        private void OnDisable()
        {
            _interactor.selectEntered.RemoveListener(DisableHandColliders);
            _interactor.selectExited.RemoveListener(EnableHandColliders);
        }

        private void Start()
        {
            UpdateTracking();
            MoveUsingTransform();
            RotateUsingTransform();
        }

        private void Update()
        {
            UpdateTracking();
        }

        private void FixedUpdate()
        {
            if (IsHoldingObject() || !WithinPhysicsRange())
            {
                MoveUsingTransform();
                RotateUsingTransform();
            }
            else
            {
                MoveUsingPhysics();
                RotateUsingPhysics();
            }
        }

        private void UpdateTracking()
        {
            _targetPos = _controller.positionAction.action.ReadValue<Vector3>();
            _targetRot = _controller.rotationAction.action.ReadValue<Quaternion>();
        }

        private void MoveUsingTransform()
        {
            _rigidbody.velocity = Vector3.zero;
            transform.localPosition = _targetPos;
        }

        private void RotateUsingTransform()
        {
            _rigidbody.angularVelocity = Vector3.zero;
            transform.localRotation = _targetRot;
        }

        private void MoveUsingPhysics()
        {
            _rigidbody.velocity *= slowDownVelocity;

            var newVelocity = FindNewVelocity();
            if (!IsValidVelocity(newVelocity.x)) return;

            var maxChange = maxPositionChange * Time.deltaTime;
            _rigidbody.velocity = Vector3.MoveTowards(_rigidbody.velocity, newVelocity, maxChange);
        }

        private Vector3 FindNewVelocity()
        {
            var worldPosition = _parentTransform.TransformPoint(_targetPos);
            return (worldPosition - _rigidbody.position) / Time.deltaTime;
        }

        private void RotateUsingPhysics()
        {
            _rigidbody.angularVelocity *= slowDownAngularVelocity;

            var newVelocity = FindNewAngularVelocity();
            if (!IsValidVelocity(newVelocity.x)) return;

            var maxChange = maxRotationChange * Time.deltaTime;
            _rigidbody.angularVelocity = Vector3.MoveTowards(_rigidbody.angularVelocity, newVelocity, maxChange);
        }

        private Vector3 FindNewAngularVelocity()
        {
            var worldRotation = _parentTransform.rotation * _targetRot;
            var difference = worldRotation * Quaternion.Inverse(_rigidbody.rotation);
            difference.ToAngleAxis(out var angleInDegrees, out var rotationAxis);

            angleInDegrees = angleInDegrees > 180 ? angleInDegrees - 360 : angleInDegrees;

            return rotationAxis * (angleInDegrees * Mathf.Deg2Rad) / Time.deltaTime;
        }

        private bool IsValidVelocity(float velocity)
        {
            return !float.IsNaN(velocity) && !float.IsInfinity(velocity);
        }

        private bool IsHoldingObject()
        {
            return _interactor.selectTarget;
        }

        private bool WithinPhysicsRange()
        {
            return Physics.CheckSphere(transform.position, physicsRange, physicsMask, QueryTriggerInteraction.Ignore);
        }

        private void DisableHandColliders(SelectEnterEventArgs interactable)
        {
            foreach (var handCollider in _handColliders)
            {
                handCollider.enabled = false;
            }
        }

        private void EnableHandColliders(SelectExitEventArgs interactable)
        {
            StartCoroutine(WaitForRange());
        }

        private IEnumerator WaitForRange()
        {
            yield return new WaitWhile(WithinPhysicsRange);
            foreach (var handCollider in _handColliders)
            {
                handCollider.enabled = true;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, physicsRange);
        }

        private void OnValidate()
        {
            if (TryGetComponent(out Rigidbody rigidbodyComp))
            {
                rigidbodyComp.useGravity = false;
            }
        }
    }
}