using PopovRadio.Scripts.Gameplay.Hands;
using UnityEditor;
using UnityEngine;

namespace PopovRadio.Scripts.Tools.Editor
{
    [CustomEditor(typeof(PreviewHand))]
    public class HandPreviewEditor : UnityEditor.Editor
    {
        private PreviewHand _previewHand;
        private Transform _activeJoint;

        private void OnEnable()
        {
            _previewHand = target as PreviewHand;
        }

        private void OnSceneGUI()
        {
            EditorUtility.SetDirty(target);
            DrawJointButtons();
            DrawJointHandle();
        }

        private void DrawJointButtons()
        {
            foreach (var joint in _previewHand.Joints)
            {
                var pressed = Handles.Button(joint.position, joint.rotation, 0.01f, 0.005f, Handles.SphereHandleCap);

                if (pressed)
                    _activeJoint = IsSelected(joint) ? null : joint;
            }
        }

        private bool IsSelected(Transform joint)
        {
            return joint == _activeJoint;
        }

        private void DrawJointHandle()
        {
            if (!HasActiveJoint()) return;

            var currentRotation = _activeJoint.rotation;
            var newRotation = Handles.RotationHandle(currentRotation, _activeJoint.position);

            if (!HandleRotated(currentRotation, newRotation)) return;

            _activeJoint.rotation = newRotation;
            Undo.RecordObject(target, "Joint Rotated");
        }

        private bool HasActiveJoint()
        {
            return _activeJoint;
        }

        private bool HandleRotated(Quaternion currentRotation, Quaternion newRotation)
        {
            return currentRotation != newRotation;
        }
    }
}