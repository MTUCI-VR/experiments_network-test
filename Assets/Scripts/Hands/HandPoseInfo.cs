using System;
using System.Collections.Generic;
using UnityEngine;

namespace PopovRadio.Scripts.Gameplay.Hands
{
    [Serializable]
    public class HandPoseInfo
    {
        public Vector3 attachPosition = Vector3.zero;
        public Quaternion attachRotation = Quaternion.identity;
        public List<Quaternion> fingerRotations = new List<Quaternion>();

        public static HandPoseInfo Empty => new HandPoseInfo();

        public void Save(PreviewHand hand)
        {
            var transform = hand.transform;
            attachPosition = transform.localPosition;
            attachRotation = transform.localRotation;

            fingerRotations = hand.GetJointRotations();
        }
    }
}