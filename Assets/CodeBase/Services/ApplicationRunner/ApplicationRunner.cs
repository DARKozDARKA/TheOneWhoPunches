using System;
using UnityEngine;

namespace CodeBase.Services.ApplicationRunner
{
    public class ApplicationRunner : IApplicationRunner
    {
        public Action OnApplicationQuit { get; set; }

        private bool _isQuitting;
        
        public ApplicationRunner() =>
            Application.quitting += OnQuit;

        public void Quit() =>
            Application.Quit();

        public bool IsQuitting() =>
            _isQuitting;

        private void OnQuit()
        {
            _isQuitting = true;
            OnApplicationQuit?.Invoke();
            Application.quitting -= OnApplicationQuit;
        }
    }
}