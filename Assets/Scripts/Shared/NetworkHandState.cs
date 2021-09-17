using MLAPI;
using MLAPI.NetworkVariable;

namespace Shared
{
    public class NetworkHandState : NetworkBehaviour, INetworkHandAnimationTriggers
    {
        public NetworkVariableFloat GripValue { get; } = new NetworkVariableFloat(new NetworkVariableSettings()
            {WritePermission = NetworkVariablePermission.OwnerOnly});

        public NetworkVariableFloat TriggerValue { get; } = new NetworkVariableFloat(new NetworkVariableSettings()
            {WritePermission = NetworkVariablePermission.OwnerOnly});
    }
}