using Microsoft.EntityFrameworkCore;

public class PersonRepository : Repository<Person>, IPersonRepository
{
    public PersonRepository(DbContext context) : base(context)
    {
    }

    public IEnumerable<Person> GetRelatives(Person person)
    {
        return _context.Set<Person>().Where(p => p.Surname == person.Surname && p.Name != person.Name).ToList();
    }

    public IEnumerable<Person> OrderByAge()
    {
        return _context.Set<Person>().OrderBy(p => p.Age).ToList();
    }
}