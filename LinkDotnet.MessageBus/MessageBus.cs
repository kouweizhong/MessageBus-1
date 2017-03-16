using System;
using System.Collections.Generic;
using LinkDotNet.MessageHandling.Contracts;

namespace LinkDotNet.MessageHandling
{
    /// <summary>
    /// Lean and mean implementation of the messagebus
    /// </summary>
    public class MessageBus : IMessageBus
    {
        /// <summary>
        /// Holds every message which is send through this messagebus mapped to there actions
        /// </summary>
        private readonly Dictionary<Type, List<Delegate>> _handler = new Dictionary<Type, List<Delegate>>();

        /// <summary>
        /// Sends an message
        /// </summary>
        /// <param name="message">The message-object to be send</param>
        public void Send<T>(T message) where T : IMessage
        {
            var type = typeof(T);
            if (!_handler.ContainsKey(type))
            {
                return;
            }

            // Call every handler
            foreach (var actionHandler in _handler[type])
            {
                var first = actionHandler as Action<T>;
                if (first != null)
                {
                    first.Invoke(message);
                    continue;
                }

                var second = actionHandler as Action;
                if (second != null)
                {
                    second.Invoke();
                }
            }
        }

        /// <summary>
        /// Subscribes to the specific message and executes the action, when this messagebus sends the message
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action">Action to be called, when the message is received</param>
        public void Subscribe<T>(Action action) where T : IMessage
        {
            AddHandlerToMessageType(typeof(T), action);
        }

        /// <summary>
        /// Subscribes to the specific message and executes the action, when this messagebus sends the message
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action">Action to be called, when the message is received</param>
        public void Subscribe<T>(Action<T> action) where T : IMessage
        {
            AddHandlerToMessageType(typeof(T), action);
        }

        private void AddHandlerToMessageType(Type messageType, Delegate action)
        {
            if (_handler.ContainsKey(messageType))
            {
                _handler[messageType].Add(action);
            }
            else
            {
                _handler[messageType] = new List<Delegate> { action };
            }
        }
    }
}
