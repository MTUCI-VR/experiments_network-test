using PopovRadio.Scripts.Gameplay.Hands;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PopovRadio.Scripts.Tools.Editor
{
    public class PoseWindow : EditorWindow
    {
        private Pose _activePose;
        private GameObject _poseHelper;

        private HandManager _handManager;
        private SelectionHandler _selectionHandler;

        private void OnEnable()
        {
            CreatePoseHelper();
            Selection.selectionChanged += UpdateSelection;
            EditorApplication.playModeStateChanged += CloseWindow;
            EditorSceneManager.sceneClosing += CloseWindow;
        }

        private void OnDisable()
        {
            DestroyPoseHelper();
            Selection.selectionChanged -= UpdateSelection;
            EditorApplication.playModeStateChanged -= CloseWindow;
            EditorSceneManager.sceneClosing -= CloseWindow;
        }

        void CreatePoseHelper()
        {
            if (_poseHelper) return;

            var helperPrefab = Resources.Load("PoseHelper");

            _poseHelper = (GameObject) PrefabUtility.InstantiatePrefab(helperPrefab);
            _poseHelper.hideFlags = HideFlags.DontSave;

            _selectionHandler = _poseHelper.GetComponent<SelectionHandler>();
            _handManager = _poseHelper.GetComponent<HandManager>();

            UpdateSelection();
        }

        private void DestroyPoseHelper()
        {
            DestroyImmediate(_poseHelper);
        }

        private void CloseWindow(PlayModeStateChange stateChange)
        {
            if (stateChange == PlayModeStateChange.ExitingEditMode)
                Close();
        }

        private void CloseWindow(Scene scene, bool removingScene)
        {
            if (removingScene)
                Close();
        }

        private void OnInspectorUpdate()
        {
            Repaint();
        }

        private void OnGUI()
        {
            var labelStyle = EditorStyles.label;
            labelStyle.alignment = TextAnchor.MiddleCenter;

            var poseName = _activePose ? _activePose.name : "No Pose";
            GUILayout.Label(poseName, labelStyle);

            using (new EditorGUI.DisabledScope(_activePose))
            {
                if (GUILayout.Button("Create Pose"))
                    CreatePose();

                if (GUILayout.Button("Refresh Pose"))
                    RefreshPose();
            }

            using (new EditorGUI.DisabledScope(!_activePose))
            {
                if (GUILayout.Button("Clear Pose"))
                    ClearPose();
            }

            using (new EditorGUI.DisabledScope(!_handManager.HandsExist))
            {
                var leftHand = _handManager.LeftHand;
                var rightHand = _handManager.RightHand;
                var objectWidth = EditorGUIUtility.currentViewWidth * 0.5f;

                using (new GUILayout.HorizontalScope())
                {
                    GUILayout.Label("Left Hand", labelStyle, GUILayout.Width(objectWidth));
                    GUILayout.Label("Right Hand", labelStyle, GUILayout.Width(objectWidth));
                }

                using (new GUILayout.HorizontalScope())
                {
                    if (GUILayout.Button("Toggle", GUILayout.Width(objectWidth)))
                        ToggleHand(leftHand);

                    if (GUILayout.Button("Toggle", GUILayout.Width(objectWidth)))
                        ToggleHand(rightHand);
                }

                using (new EditorGUI.DisabledScope(!_activePose))
                {
                    using (new GUILayout.HorizontalScope())
                    {
                        if (GUILayout.Button("Mirror L > R", GUILayout.Width(objectWidth)))
                            MirrorPose(leftHand, rightHand);

                        if (GUILayout.Button("Mirror R > L", GUILayout.Width(objectWidth)))
                            MirrorPose(rightHand, leftHand);
                    }

                    using (new GUILayout.HorizontalScope())
                    {
                        if (GUILayout.Button("Undo Changes", GUILayout.Width(objectWidth)))
                            UndoChanges(leftHand);

                        if (GUILayout.Button("Undo Changes", GUILayout.Width(objectWidth)))
                            UndoChanges(rightHand);
                    }

                    using (new GUILayout.HorizontalScope())
                    {
                        if (GUILayout.Button("Reset", GUILayout.Width(objectWidth)))
                            ResetPose(leftHand);

                        if (GUILayout.Button("Reset", GUILayout.Width(objectWidth)))
                            ResetPose(rightHand);
                    }
                }
            }

            using (new EditorGUI.DisabledScope(!_activePose))
            {
                GUILayout.Label("Remember to Save!", labelStyle);

                if (GUILayout.Button("Save Pose"))
                    _handManager.SavePose(_activePose);
            }
        }

        private void CreatePose()
        {
            _activePose = CreatePoseAsset();

            var targetObject = _selectionHandler.SetObjectPose(_activePose);
            _handManager.UpdateHands(_activePose, targetObject.transform);
        }

        private Pose CreatePoseAsset()
        {
            var pose = CreateInstance<Pose>();

            var path = AssetDatabase.GenerateUniqueAssetPath("Assets/NewPoseData.asset");
            AssetDatabase.CreateAsset(pose, path);

            return pose;
        }

        private void UpdateSelection()
        {
            if (_selectionHandler.CheckForNewInteractable())
                UpdateActivePose(Selection.activeGameObject);
        }

        private void RefreshPose()
        {
            var currentObject = _selectionHandler.CurrentInteractable.gameObject;
            UpdateActivePose(currentObject);
        }

        private void UpdateActivePose(GameObject targetObject)
        {
            _activePose = _selectionHandler.TryGetPose(targetObject);

            if (_activePose)
                _handManager.UpdateHands(_activePose, targetObject.transform);
        }

        private void ClearPose()
        {
            _selectionHandler.SetObjectPose(null);
            _activePose = null;
        }

        private void ToggleHand(PreviewHand hand)
        {
            Undo.RecordObject(hand.gameObject, "Toggle Hand");
            var isActive = !hand.gameObject.activeSelf;
            hand.gameObject.SetActive(isActive);
        }

        private void ResetPose(PreviewHand hand)
        {
            Undo.RecordObject(hand.transform, "Reset Pose");
            hand.ApplyDefaultPose();
        }

        private void UndoChanges(PreviewHand hand)
        {
            Undo.RecordObject(hand.transform, "Undo Changes");
            hand.ApplyPose(_activePose);
        }

        private void MirrorPose(PreviewHand sourceHand, PreviewHand targetHand)
        {
            Undo.RecordObject(targetHand.transform, "Mirror Pose");
            targetHand.MirrorAndApplyPose(sourceHand);
        }

        public static void Open(Pose pose)
        {
            PoseWindow window = GetWindow<PoseWindow>("Hand Poser");
            window._activePose = pose;
        }
    }
}