using System;
using MLAPI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Shared
{
    public class HandAnimation : NetworkBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private NetworkHandState networkState;
        [SerializeField] private InputActionProperty gripAction;
        [SerializeField] private InputActionProperty triggerAction;

        private static readonly int Trigger = Animator.StringToHash("Trigger");
        private static readonly int Grip = Animator.StringToHash("Grip");

        private bool _isInitialized;

        public override void NetworkStart()
        {
            _isInitialized = true;
        }

        private void Update()
        {
            if (!_isInitialized) return;

            if (IsOwner)
            {
                UpdateNetworkButtonStates();
            }

            UpdateTriggers();
        }

        private void UpdateNetworkButtonStates()
        {
            networkState.TriggerValue.Value = triggerAction.action.ReadValue<float>();
            networkState.GripValue.Value = gripAction.action.ReadValue<float>();
        }

        private void UpdateTriggers()
        {
            animator.SetFloat(Trigger, networkState.TriggerValue.Value);
            animator.SetFloat(Grip, networkState.GripValue.Value);
        }
    }
}