var channel = Channel.CreateUnbounded<Message>();

var sender = SendMessageThreadAsync(channel.Writer, 1);
var receiver = ReceiveMessageThreadAsync(channel.Reader, 1);

await sender;
// make sure all messages are received
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
    // we don't complete the writer here since there may be more than one senders
}

async Task ReceiveMessageThreadAsync(ChannelReader<Message> reader, int id)
{
    await foreach (var message in reader.ReadAllAsync())
    {
        Console.WriteLine($"Thread {id} received {message.Content}");
    }
}

record Message(int FromId, string Content);
