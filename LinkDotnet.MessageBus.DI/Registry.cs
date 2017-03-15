using LinkDotNet.MessageBus.Contracts;
using StructureMap;

namespace LinkDotNet.MessageBus.DI
{
    /// <summary>
    /// Class to register the components
    /// </summary>
    public class MessageBusRegistry : Registry
    {
        /// <summary>
        /// c'tor
        /// </summary>
        public MessageBusRegistry()
        {
            ForSingletonOf<IMessageBus>().Use<MessageBus>();
        }
    }
}
