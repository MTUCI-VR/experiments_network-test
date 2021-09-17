using UnityEditor;
using UnityEngine;
using Pose = PopovRadio.Scripts.Tools.Pose;

#if UNITY_EDITOR

#endif

namespace PopovRadio.Scripts.Gameplay.Hands
{
    [ExecuteInEditMode]
    public class HandManager : MonoBehaviour
    {
        [SerializeField] private bool hideHands = true;
        [SerializeField] private GameObject leftHandPrefab;
        [SerializeField] private GameObject rightHandPrefab;

        public PreviewHand LeftHand { get; set; }
        public PreviewHand RightHand { get; set; }

        public bool HandsExist => LeftHand && RightHand;

        private void OnEnable()
        {
            CreateHandPreviews();
        }

        private void OnDisable()
        {
            DestroyHandPreviews();
        }

        private void CreateHandPreviews()
        {
            LeftHand = CreateHand(leftHandPrefab);
            RightHand = CreateHand(rightHandPrefab);
        }

        private PreviewHand CreateHand(GameObject prefab)
        {
            var handObject = Instantiate(prefab, transform);
            if (hideHands)
                handObject.hideFlags = HideFlags.HideInHierarchy | HideFlags.DontSave;

            return handObject.GetComponent<PreviewHand>();
        }

        private void DestroyHandPreviews()
        {
#if UNITY_EDITOR
            DestroyImmediate(LeftHand.gameObject);
            DestroyImmediate(RightHand.gameObject);
#endif
        }

        public void UpdateHands(Pose pose, Transform parentTransform)
        {
            LeftHand.transform.parent = parentTransform;
            RightHand.transform.parent = parentTransform;

            LeftHand.transform.localPosition = pose.leftHandInfo.attachPosition;
            RightHand.transform.localPosition = pose.rightHandInfo.attachPosition;

            LeftHand.ApplyPose(pose);
            RightHand.ApplyPose(pose);
        }

        public void SavePose(Pose pose)
        {
#if UNITY_EDITOR
            EditorUtility.SetDirty(pose);
#endif

            pose.leftHandInfo.Save(LeftHand);
            pose.rightHandInfo.Save(RightHand);
        }
    }
}