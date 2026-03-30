class Program
{
    static int Main(string[] args)
    {
        foreach (var arg in args)
        {
            if(arg == "file")
            {
                IMessageService fileMessageService = new FileMessageService();
                fileMessageService.SendMessage("Hello, File!");
                return 0;
            }
            else if(arg == "console")
            {
                IMessageService consoleMessageService = new MessageService();
                consoleMessageService.SendMessage("Hello, Console!");
                return 0;
            }
        }
        return 0;
    }
}
