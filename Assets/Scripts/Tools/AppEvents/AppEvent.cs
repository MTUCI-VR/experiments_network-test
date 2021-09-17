using System;
using System.Collections.Generic;
using UnityEngine;

namespace PopovRadio.Scripts.Tools.AppEvents
{
    /// <summary>
    /// Менеджер события приложения
    /// </summary>
    [CreateAssetMenu(menuName = "App Event", fileName = "New App Event")]
    public class AppEvent : ScriptableObject
    {
        #region Fields

        private readonly HashSet<IAppEventListener> _listeners = new HashSet<IAppEventListener>();

        #endregion

        #region Public Methods

        /// <summary>
        /// Вызывает всех слушателей события
        /// </summary>
        public void Invoke()
        {
            foreach (var listener in _listeners)
            {
                listener.RaiseEvent();
            }
        }

        /// <summary>
        /// Добавляет нового слушателя события
        /// </summary>
        /// <param name="appEventListener"></param>
        public void Register(IAppEventListener appEventListener) => _listeners.Add(appEventListener);

        /// <summary>
        /// Удаляет существующего слушателя события 
        /// </summary>
        /// <param name="appEventListener"></param>
        public void Deregister(IAppEventListener appEventListener) => _listeners.Remove(appEventListener);

        public void DeregisterAll() => _listeners.Clear();

        #endregion
    }
}