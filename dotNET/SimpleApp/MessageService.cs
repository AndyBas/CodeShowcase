public class MessageService : IMessageService
{
    public void SendMessage(string message)
    {
        // Simulate sending a message
        Console.WriteLine($"Message sent: {message}");
    }
}