public interface ICombatStrategy
{
    public void AttackStrategy(Character from, Character target);
    public int DefenseStrategy(int damage, Character target);
}