public class StealHealthStrategy : ICombatStrategy
{
    const float STEAL_RATIO = 0.5f;
    public void AttackStrategy(Character from, Character target)
    {
        int damage = from.ComputeDamage(target);
        int healthStealed = (int)(damage * STEAL_RATIO);

        Console.WriteLine($"{from.Name} is stealing {healthStealed} health to {target.Name}");
        target.Hit(from, healthStealed);
        from.Heal(healthStealed);
    }

    public int DefenseStrategy(int damage, Character target)
    {
        return (int)(damage / STEAL_RATIO);
    }
}