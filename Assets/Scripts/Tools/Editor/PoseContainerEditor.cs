using UnityEditor;
using UnityEngine;

namespace PopovRadio.Scripts.Tools.Editor
{
    [CustomEditor(typeof(PoseContainer))]
    public class PoseContainerEditor : UnityEditor.Editor
    {
        private PoseContainer _poseContainer;

        private void OnEnable()
        {
            _poseContainer = (PoseContainer) target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Open Pose Editor"))
                PoseWindow.Open(_poseContainer.Pose);
        }
    }
}