using GenericEventBus;

namespace Revenaant.Project.Messages
{
    public interface IMessage { }

    public class CentralMessageBus
    {
        private static GenericEventBus<IMessage> instance;
        public static GenericEventBus<IMessage> Instance => instance ??= new GenericEventBus<IMessage>();
    }
}
