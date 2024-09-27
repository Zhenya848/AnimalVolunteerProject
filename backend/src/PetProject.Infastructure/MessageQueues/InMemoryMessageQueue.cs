using PetProject.Application.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using PetProject.Application.Files.Providers;

namespace PetProject.Infastructure.MessageQueues
{
    public class InMemoryMessageQueue<TMessage> : IMessageQueue<TMessage>
    {
        private readonly Channel<TMessage> _channel
            = Channel.CreateUnbounded<TMessage>();

        public async Task WriteAsync(TMessage message, CancellationToken cancellationToken)
        {
            await _channel.Writer.WriteAsync(message, cancellationToken);
        }

        public async Task<TMessage> ReadAsync(CancellationToken cancellationToken = default)
        {
            return await _channel.Reader.ReadAsync(cancellationToken);
        }
    }
}
