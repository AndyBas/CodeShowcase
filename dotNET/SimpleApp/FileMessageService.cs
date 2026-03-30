public class FileMessageService : IMessageService
{
    public void SendMessage(string message)
    {
        // Simulate sending a message by writing it to a file
        System.IO.File.WriteAllText("message.txt", message);
        Console.WriteLine($"Message written to file: {message}");
    }
}