using System;
using Hands;
using MLAPI;
using PopovRadio.Scripts.Gameplay.Hands;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Simulation;

namespace Shared
{
    public class Player : NetworkBehaviour
    {
        [SerializeField] private XRBaseController leftHandController;
        [SerializeField] private GameHand leftGameHand;
        [SerializeField] private XRBaseController rightHandController;
        [SerializeField] private GameHand rightGameHand;
        [SerializeField] private bool useSimulator;
        [SerializeField] private GameObject simulatorPrefab;
        [SerializeField] private Camera cameraObject;
        [SerializeField] private TrackedPoseDriver cameraPoseDriver;

        private void Start()
        {
            if (IsLocalPlayer)
            {
                if (useSimulator) EnableSimulator();
            }
            else
            {
                DisableLocalComponents();
            }
        }

        private void EnableSimulator()
        {
            if (!simulatorPrefab)
            {
                throw new NullReferenceException("Player: simulatorPrefab не задан");
            }

            var simulator = Instantiate(simulatorPrefab, transform);
            simulator.GetComponent<XRDeviceSimulator>().cameraTransform = cameraObject.transform;
        }

        private void DisableLocalComponents()
        {
            leftHandController.enabled = false;
            leftGameHand.enabled = false;
            rightHandController.enabled = false;
            rightGameHand.enabled = false;
            cameraObject.enabled = false;
            cameraPoseDriver.enabled = false;
        }
    }
}