public class Inventory<T> where T : IItem
{
    private List<T> _list = new List<T>();

    public void AddItem(T newItem)
    {
        if(_list.Where(x => x.Equals(newItem)).Count() <= 0)
        {
            _list.Add(newItem);
            Console.WriteLine($"Add {newItem} with id {newItem.GetID()}");
        }
        else
            Console.WriteLine($"Cannot add item {newItem} because one with similar id {newItem.GetID()} is already into the inventory");
    }

    public void RemoveItem(int id)
    {
        IItem? itemToRemove = GetItemByID(id);

        if(itemToRemove == null)
            return;

        _list.Remove((T)itemToRemove);
        Console.WriteLine($"Remove {itemToRemove} with id {itemToRemove.GetID()}");
        
    }

    public T? GetItemByID(int idToFind)
    {
        T? element = _list.FirstOrDefault(x => x.GetID() == idToFind);

        return element;
    }
}