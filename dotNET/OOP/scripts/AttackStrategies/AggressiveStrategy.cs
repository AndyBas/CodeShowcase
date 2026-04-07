public class AggressiveStrategy : ICombatStrategy
{
    public void AttackStrategy(Character from, Character target)
    {
        target.Hit(from, from.ComputeDamage(target));
    }

    public int DefenseStrategy(int damage, Character target)
    {
        return damage;
    }
}