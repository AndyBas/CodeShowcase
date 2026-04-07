static class Program
{
    static void Main()
    {
        Character a = new Mage("Merlin", 70, 15, new StealHealthStrategy(), 100);
        Character b = new Warrior("Hercule", 150, 10, new DefensiveStrategy());

        bool isATurn = true;

        Console.WriteLine($"{a.Name} will face {b.Name}");

        while (!a.IsDead && !b.IsDead)
        {
            if(isATurn)
            {
                a.Attack(b);
            }
            else
                b.Attack(a);

            isATurn = !isATurn;
        }

        Console.WriteLine($"And the winner is {(a.IsDead ? b.Name : a.Name)}");

    }
}
