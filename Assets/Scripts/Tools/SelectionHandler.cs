using PopovRadio.Scripts.Gameplay.Hands;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;
using Pose = PopovRadio.Scripts.Tools.Pose;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;

#endif

namespace PopovRadio.Scripts.Tools
{
    [ExecuteInEditMode]
    public class SelectionHandler : MonoBehaviour
    {
        public XRBaseInteractable CurrentInteractable { get; private set; } = null;

        public bool CheckForNewInteractable()
        {
            var newInteractable = GetInteractable();

            var isDifferent = IsDifferentInteractable(CurrentInteractable, newInteractable);
            CurrentInteractable = isDifferent ? newInteractable : CurrentInteractable;

            return isDifferent;
        }

        private XRBaseInteractable GetInteractable()
        {
            XRBaseInteractable newInteractable = null;
            GameObject selectedObject = null;

#if UNITY_EDITOR
            selectedObject = Selection.activeGameObject;
#endif

            if (!selectedObject) return null;

            if (selectedObject.TryGetComponent(out XRBaseInteractable interactable))
                newInteractable = interactable;

            return newInteractable;
        }

        private bool IsDifferentInteractable(XRBaseInteractable currentInteractable, XRBaseInteractable newInteractable)
        {
            var isDifferent = !currentInteractable;

            if (currentInteractable && newInteractable)
                isDifferent = currentInteractable != newInteractable;

            return isDifferent;
        }

        public GameObject SetObjectPose(Pose pose)
        {
            GameObject selectedObject = null;

#if UNITY_EDITOR
            selectedObject = Selection.activeGameObject;
#endif

            if (!selectedObject) return selectedObject;
            if (!selectedObject.TryGetComponent(out PoseContainer poseContainer)) return selectedObject;

            poseContainer.Pose = pose;

            MarkActiveSceneAsDirty();

            return selectedObject;
        }

        public Pose TryGetPose(GameObject targetObject)
        {
            Pose pose = null;
            if (!targetObject) return null;

            if (targetObject.TryGetComponent(out PoseContainer poseContainer))
                pose = poseContainer.Pose;

            return pose;
        }

        private void MarkActiveSceneAsDirty()
        {
#if UNITY_EDITOR
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
#endif
        }
    }
}