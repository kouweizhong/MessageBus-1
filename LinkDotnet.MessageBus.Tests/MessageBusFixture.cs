using LinkDotNet.MessageHandling.Contracts;
using NUnit.Framework;

namespace LinkDotNet.MessageHandling.Tests
{
    [TestFixture]
    public class MessageBusFixture
    {
        private MessageBus _messageBus;

        [SetUp]
        public void SetUp()
        {
            _messageBus = new MessageBus();
        }

        [Test]
        public void Should_call_handler_on_registered_message()
        {
            var iWasCalled = false;
            _messageBus.Subscribe<FakeMessage>(() => iWasCalled = true);

            _messageBus.Send(new FakeMessage());

            Assert.That(iWasCalled, Is.True);
        }

        [Test]
        public void Should_not_call_handler_when_is_not_registered_message()
        {
            var iWasCalled = false;
            _messageBus.Subscribe<AnotherFakeMessage>(() => iWasCalled = true);

            _messageBus.Send(new FakeMessage());

            Assert.That(iWasCalled, Is.False);
        }

        [Test]
        public void Should_pass_in_parameters()
        {
            var id = 0;
            _messageBus.Subscribe<AnotherFakeMessage>((msg) => id = msg.Id);

            _messageBus.Send(new AnotherFakeMessage(3));

            Assert.That(id, Is.EqualTo(3));
        }
    }

    public class FakeMessage : IMessage
    {
    }

    public class AnotherFakeMessage : IMessage
    {
        public int Id { get; private set; }
        public AnotherFakeMessage(int id)
        {
            Id = id;
        }
    }
}
