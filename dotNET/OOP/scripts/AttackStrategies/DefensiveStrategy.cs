public class DefensiveStrategy : ICombatStrategy
{
    const float DEFENSE_FACTOR = 0.2f;
    const float ATTACK_FACTOR = 0.1f;
    public void AttackStrategy(Character from, Character target)
    {
        target.Hit(from, (int)(from.ComputeDamage(target) * ATTACK_FACTOR));
    }

    public int DefenseStrategy(int damage, Character target)
    {
        Console.WriteLine($"{target.Name} defends itself");
        return (int)(damage * DEFENSE_FACTOR);
    }
}