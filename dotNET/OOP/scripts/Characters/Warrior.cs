public class Warrior : Character
{
    const int PHYSICAL_ATTACK_BONUS = 5;
    public Warrior(string name, int health, int strength, ICombatStrategy attackStrategy)
        : base(name, health, strength, attackStrategy)
    {
    }

    public override void Attack(Character target)
    {
        Console.WriteLine($"{Name} performs a powerful strike on {target.Name}!");
        base.Attack(target);
    }

    public override int ComputeDamage(Character target)
    {
        // Warriors deal more damage with physical attacks
        return base.ComputeDamage(target) + PHYSICAL_ATTACK_BONUS; // Add a flat bonus for warriors
    }
}