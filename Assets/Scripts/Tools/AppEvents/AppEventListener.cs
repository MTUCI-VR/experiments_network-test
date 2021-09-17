using System;
using UnityEngine;
using UnityEngine.Events;

namespace PopovRadio.Scripts.Tools.AppEvents
{
    /// <summary>
    /// Слушатель события приложения
    /// </summary>
    public class AppEventListener : MonoBehaviour, IAppEventListener
    {
        #region Settings

        [Tooltip("Событие приложения")] [SerializeField]
        private AppEvent appEvent;

        [Tooltip("События, вызываемые при срабатывании события")] [SerializeField]
        private UnityEvent unityEvent;

        #endregion

        #region LifeCycle

        private void Awake() => appEvent.Register(this);

        private void OnDestroy() => appEvent.Deregister(this);

        #endregion

        #region Public Methods

        public void RaiseEvent()
        {
            if (!gameObject.activeSelf) return;
            unityEvent.Invoke();
        }

        #endregion
    }
}