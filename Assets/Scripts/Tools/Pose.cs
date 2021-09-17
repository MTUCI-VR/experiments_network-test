using PopovRadio.Scripts.Gameplay.Hands;
using UnityEngine;

namespace PopovRadio.Scripts.Tools
{
    [CreateAssetMenu(fileName = "NewPose")]
    public class Pose : ScriptableObject
    {
        public HandPoseInfo leftHandInfo = HandPoseInfo.Empty;
        public HandPoseInfo rightHandInfo = HandPoseInfo.Empty;

        public HandPoseInfo GetHandInfo(HandType handType)
        {
            return handType switch
            {
                HandType.Left => leftHandInfo,
                HandType.Right => rightHandInfo,
                _ => HandPoseInfo.Empty
            };
        }
    }
}