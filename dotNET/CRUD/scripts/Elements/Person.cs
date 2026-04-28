public class Person
{
    public int Id { get; private set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public int Age { get; set; }

    public Person(string name, string surname, string email, int age)
    {
        Name = name;
        Surname = surname;
        Email = email;
        Age = age;
    }
}