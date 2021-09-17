using MLAPI.NetworkVariable;

namespace Shared
{
    public interface INetworkHandAnimationTriggers
    {
        public NetworkVariableFloat GripValue { get; }

        public NetworkVariableFloat TriggerValue { get; }
    }
}