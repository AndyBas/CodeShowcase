public interface IPersonRepository : IRepository<Person>
{
    public IEnumerable<Person> OrderByAge();
    public IEnumerable<Person> GetRelatives(Person person);
}