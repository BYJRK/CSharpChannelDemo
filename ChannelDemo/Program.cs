var channel = Channel.CreateUnbounded<Message>();

var sender = SendMessageThreadAsync(channel.Writer, 1);
var receiver = ReceiveMessageThreadAsync(channel.Reader, 1);

await sender;
// make sure all messages are received
await Task.Delay(100);
channel.Writer.Complete();
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

async Task ReceiveMessageThreadAsync(ChannelReader<Message> reader, int id)
{
    try
    {
        while (!reader.Completion.IsCompleted)
        {
            var message = await reader.ReadAsync();
            Console.WriteLine($"Thread {id} received {message.Content} from {message.FromId}");
        }
    }
    // this is needed since the while condition cannot detect the completion of the channel in time
    catch (ChannelClosedException)
    {
        Console.WriteLine($"Thread {id} task stopped.");
    }
}

record Message(int FromId, string Content);
