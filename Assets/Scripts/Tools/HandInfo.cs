using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace PopovRadio.Scripts.Tools
{
    [CreateAssetMenu]
    public class HandInfo : ScriptableObject
    {
        public GameObject HoldingObject { get; set; }
    }
}