using UnityEngine.Events;

namespace PopovRadio.Scripts.Tools.AppEvents
{
    public class AppEventScriptListener : IAppEventListener
    {
        public UnityEvent Event = new UnityEvent();

        public void RaiseEvent()
        {
            Event.Invoke();
        }
    }
}