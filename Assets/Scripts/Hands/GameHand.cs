using PopovRadio.Scripts.Gameplay.Hands;
using PopovRadio.Scripts.Tools;
using Shared;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

namespace Hands
{
    [RequireComponent(typeof(NetworkHandState))]
    public class GameHand : BaseHand
    {
        [SerializeField] private Animator handAnimator;
        
        [SerializeField] private HandInfo info;

        private XRBaseInteractor _interactor;

        private bool _isGripAllowed = true;

        protected override void Awake()
        {
            base.Awake();

            _interactor = GetComponentInParent<XRBaseInteractor>();
        }

        private void OnEnable()
        {
            _interactor.selectEntered.AddListener(OnSelectEntered);
            _interactor.selectExited.AddListener(OnSelectExited);

            _interactor.hoverEntered.AddListener(DisableGrip);
            _interactor.hoverExited.AddListener(EnableGrip);
        }

        private void OnDisable()
        {
            _interactor.selectEntered.RemoveListener(OnSelectEntered);
            _interactor.selectExited.RemoveListener(OnSelectExited);

            _interactor.hoverEntered.RemoveListener(DisableGrip);
            _interactor.hoverExited.RemoveListener(EnableGrip);
        }

        private void OnSelectEntered(SelectEnterEventArgs eventArgs)
        {
            TryApplyObjectPose(eventArgs.interactable);
            info.HoldingObject = eventArgs.interactable.gameObject;
        }

        private void OnSelectExited(SelectExitEventArgs eventArgs)
        {
            TryApplyDefaultPose(eventArgs.interactable);
            info.HoldingObject = null;
        }

        private void TryApplyObjectPose(XRBaseInteractable interactable)
        {
            handAnimator.enabled = false;
            if (interactable.TryGetComponent(out PoseContainer poseContainer))
            {
                ApplyPose(poseContainer.Pose);
            }
        }

        private void TryApplyDefaultPose(XRBaseInteractable interactable)
        {
            if (interactable.TryGetComponent(out PoseContainer _))
            {
                ApplyDefaultPose();
            }

            handAnimator.enabled = true;
        }

        private void EnableGrip(HoverExitEventArgs eventArgs)
        {
            _isGripAllowed = true;
        }

        private void DisableGrip(HoverEnterEventArgs eventArgs)
        {
            _isGripAllowed = false;
        }

        /// <summary>
        /// Применяет смещение к точке схатывания объекта
        /// </summary>
        /// <param name="position">Координаты позы руки</param>
        /// <param name="rotation">Поворот позы руки</param>
        protected override void ApplyOffset(Vector3 position, Quaternion rotation)
        {
            var finalPosition = position * -1f;
            var finalRotation = Quaternion.Inverse(rotation);

            finalPosition = finalPosition.RotatePointAroundPivot(Vector3.zero, finalRotation.eulerAngles);

            _interactor.attachTransform.localPosition = finalPosition;
            _interactor.attachTransform.localRotation = finalRotation;
        }
    }
}