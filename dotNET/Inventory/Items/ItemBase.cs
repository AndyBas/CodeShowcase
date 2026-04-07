public abstract class ItemBase : IItem
{
    protected int _id;
    public int GetID() => _id;

    public void SetID(int id)
    {
        _id = id;
    }

    public override bool Equals(object? obj)
    {
        return obj is IItem item && item.GetID() == _id;
    }

    public override int GetHashCode()
    {
        return _id.GetHashCode();
    }

    public abstract void Use();
}