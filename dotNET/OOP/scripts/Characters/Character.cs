public class Character
{
    public string Name { get; private set; }

    protected int _health;
    protected int _maxHealth;
    protected int _strength;
    protected ICombatStrategy _combatStrategy;

    public bool IsDead => _health == 0;

    public Character(string name, int health, int strength, ICombatStrategy attackStrategy)
    {
        Name = name;
        _health = _maxHealth = health;
        _strength = strength;
        _combatStrategy = attackStrategy;
    }

    virtual public void Attack(Character target)
    {
        _combatStrategy.AttackStrategy(this, target);
    }

    virtual public void Hit(Character from, int damage)
    {
        int finalDamage = _combatStrategy.DefenseStrategy(damage, this);
        _health -= finalDamage;

        if(_health < 0)
            _health = 0;

        Console.WriteLine($"{Name} is hit by {from.Name} for {finalDamage} damage! It remains {_health} of life");
    }

    virtual public void Heal(int amount)
    {
        _health += amount;
        Console.WriteLine($"{Name} heal itself of {amount} life points and has now {_health}");
        
        if(_health > _maxHealth)
            _health = _maxHealth;
    }

    virtual public int ComputeDamage(Character target)
    {
        // Base damage calculation (can be overridden by subclasses)
        return _strength;
    }
}