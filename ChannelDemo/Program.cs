var channel = Channel.CreateUnbounded<Message>();

using var cts = new CancellationTokenSource();

var sender = SendMessageThreadAsync(channel.Writer, 1);
var receiver = ReceiveMessageThreadAsync(channel.Reader, 1, cts.Token);

await sender;
// make sure all messages are received
await Task.Delay(100);
cts.Cancel();
await receiver;

Console.WriteLine("Press any key to exit...");
Console.ReadKey();

async Task SendMessageThreadAsync(ChannelWriter<Message> writer, int id)
{
    for (int i = 1; i <= 20; i++)
    {
        await writer.WriteAsync(new Message(id, i.ToString()));
        Console.WriteLine($"Thread {id} sent {i}");
        await Task.Delay(100);
    }
}

async Task ReceiveMessageThreadAsync(ChannelReader<Message> reader, int id, CancellationToken token)
{
    try
    {
        while (!token.IsCancellationRequested)
        {
            var message = await reader.ReadAsync(token);
            Console.WriteLine($"Thread {id} received {message.Content} from {message.FromId}");
        }
    }
    catch (OperationCanceledException)
    {
        Console.WriteLine($"Thread {id} task canceled.");
    }
}

record Message(int FromId, string Content);
