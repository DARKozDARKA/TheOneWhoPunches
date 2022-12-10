using System;
using System.Collections.Generic;
using CodeBase.Messenger;

namespace CodeBase.Infrastructure.States.GameLoop
{
    public class PlayerGateaway
    {
        private Dictionary<Type, Action<ToServerMessage>> _actions = new();
        
        public void HandleMessage<T>(T message) where T : ToServerMessage
        {
            if (_actions.ContainsKey(typeof(T)) == false)
                throw new Exception($"No listeners for message {typeof(T)}");
            
            _actions[typeof(T)]?.Invoke(message);
        }
        
        public void RegisterToMessage<T>(Action<ToServerMessage> action) where T : ToServerMessage
        {
            if (_actions.ContainsKey(typeof(T)) == false)
                _actions.Add(typeof(T), action);
        }
    }
}