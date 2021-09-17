using System.Collections.Generic;
using UnityEngine;

namespace PopovRadio.Scripts.Gameplay.Hands
{
    [SelectionBase]
    [ExecuteInEditMode]
    public class PreviewHand : BaseHand
    {
        public void MirrorAndApplyPose(PreviewHand sourceHand)
        {
            var mirroredRotations = MirrorJoints(sourceHand.Joints);
            ApplyFingerRotations(mirroredRotations);

            var mirroredPosition = MirrorPosition(sourceHand.transform);
            var mirroredRotation = MirrorRotation(sourceHand.transform);
            ApplyOffset(mirroredPosition, mirroredRotation);
        }

        private List<Quaternion> MirrorJoints(List<Transform> joints)
        {
            var mirroredJoints = new List<Quaternion>();

            foreach (var joint in joints)
            {
                var inversedRotation = MirrorJoint(joint);
                mirroredJoints.Add(inversedRotation);
            }

            return mirroredJoints;
        }

        private Quaternion MirrorJoint(Transform sourceTransform)
        {
            var mirrorRotation = sourceTransform.localRotation;
            // mirrorRotation.x *= -1f;

            return mirrorRotation;
        }

        private Quaternion MirrorRotation(Transform sourceTransform)
        {
            var mirrorRotation = sourceTransform.localRotation;
            mirrorRotation.y *= -1.0f;

            return mirrorRotation;
        }

        private Vector3 MirrorPosition(Transform sourceTransform)
        {
            var mirroredPosition = sourceTransform.localPosition;
            mirroredPosition.x *= -1f;

            return mirroredPosition;
        }

        protected override void ApplyOffset(Vector3 position, Quaternion rotation)
        {
            transform.localPosition = position;
            transform.localRotation = rotation;
        }
    }
}