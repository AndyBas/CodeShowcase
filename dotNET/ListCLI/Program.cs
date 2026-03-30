using Microsoft.Extensions.Logging;

class Program
{
    static Dictionary<int, ToDoItem> tasks = new Dictionary<int, ToDoItem>();
    static bool isRunning = true;
    static int taskIdCounter = 1;
    static int Main()
    {
        using ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddConsole();
            builder.AddFile("logs/log.txt"); // Will add as a suffix the date of the log entry
        });
        ILogger logger = loggerFactory.CreateLogger<Program>();


        while (isRunning)
        {
            Console.WriteLine("\n--- Menu ---");
            Console.WriteLine("1. Ajouter une tâche");
            Console.WriteLine("2. Supprimer une tâche");
            Console.WriteLine("3. Lister les tâches");
            Console.WriteLine("4. Completer une tâche");
            Console.WriteLine("5. Quitter");
            Console.Write("Choix : ");

            string? choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                AddTask(logger);
                break;

                case "2":
                RemoveTask(logger);
                break;

                case "3":
                ListTasks(logger);
                break;

                
                case "4":
                CompleteTask(logger);
                break;
                
                case "5":
                default:
                logger.LogInformation("Exiting application");
                Quit();
                break;

                case "6":
                    try
                    {
                        throw new Exception("This is a test exception");
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "An error occurred while testing exception logging");
                    }
                break;
            }
        }
        return 0;
    }

    private static void CompleteTask(ILogger logger)
    {
        Console.Write("What task do you want to complete ? ");
        string? taskToComplete = Console.ReadLine();
        if(string.IsNullOrWhiteSpace(taskToComplete))
        {
            Console.WriteLine("Cannot complete a null value");
            return;
        }
        try
        {
            logger.LogInformation($"Attempting to complete task with ID: {taskToComplete}");
            int.TryParse(taskToComplete, out int taskId);
            if(tasks.ContainsKey(taskId))
            {
                tasks[taskId].IsCompleted = true;
                Console.WriteLine($"Task {taskToComplete} completed");
            }
            else
                Console.WriteLine($"There is no tasks entitled {taskToComplete}");
                
            logger.LogInformation($"Task completed with ID: {taskToComplete}");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"An error occurred while trying to complete task with ID: {taskToComplete}");
            return;
        }
    }

    static void AddTask(ILogger logger)
    {
        Console.Write("What task do you want to add ? ");
        string? newTask = Console.ReadLine();
        if(string.IsNullOrWhiteSpace(newTask))
        {
            Console.WriteLine("Cannot add a null value");
            return;
        }
        if(!tasks.ContainsKey(taskIdCounter))
        {
            tasks.Add(taskIdCounter, new ToDoItem { Title = newTask, CreatedAt = DateTime.Now, IsCompleted = false });
            Console.WriteLine($"Task {newTask} added");
            taskIdCounter++;
        }
        else Console.WriteLine("The task is already in the list");

        logger.LogInformation($"Task added with ID: {taskIdCounter - 1} and title: {newTask}");
    }

    static void RemoveTask(ILogger logger)
    {
        Console.Write("\nWhat task do you want to remove (enter ID) ? ");
        string? taskToRemove = Console.ReadLine();
        if(string.IsNullOrWhiteSpace(taskToRemove))
        {
            Console.WriteLine("Cannot remove a null value");
            return;
        }

        try
        {
            logger.LogInformation($"Attempting to remove task with ID: {taskToRemove}");
            int.TryParse(taskToRemove, out int taskId);
            if(tasks.ContainsKey(taskId))
            {
                tasks.Remove(taskId);
                Console.WriteLine($"Task {taskToRemove} removed");
            }
            else
                Console.WriteLine($"There is no tasks entitled {taskToRemove}");

            logger.LogInformation($"Task removed with ID: {taskToRemove}");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"An error occurred while trying to remove task with ID: {taskToRemove}");
            return;
        }
    }

    static void ListTasks(ILogger logger)
    {
        int count = tasks.Count;

        if(count == 0)
        {
            Console.WriteLine("There is no tasks yet");
            return;
        }

        for (int i = 0; i < tasks.Count; i++)
        {
            logger.LogInformation($"Listing task with ID: {tasks.Keys.ElementAt(i)}");
            Console.WriteLine($"At index {tasks.Keys.ElementAt(i)} we have this task: {tasks.Values.ElementAt(i).Title} created at {tasks.Values.ElementAt(i).CreatedAt} and is completed: {tasks.Values.ElementAt(i).IsCompleted}");
        }
    }

    static void Quit()
    {
        isRunning = false;
    }
}

