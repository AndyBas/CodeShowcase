public class Mage : Character
{
    const int SPELL_COST = 10;
    public int Mana { get; set; }

    public Mage(string name, int health, int strength, ICombatStrategy attackStrategy, int mana)
        : base(name, health, strength, attackStrategy)
    {
        Mana = mana;
    }

    public override void Attack (Character target)
    {
        if (Mana >= SPELL_COST)
        {
            Console.WriteLine($"{Name} casts a spell on {target.Name} for {ComputeDamage(target)} damage!");
            base.Attack(target);
            Mana -= SPELL_COST;
        }
        else
        {
            Console.WriteLine($"{Name} does not have enough mana to cast a spell! It will do a simple attack");
            base.Attack(target);
        }
    }

    public override int ComputeDamage(Character target)
    {
        // Mages deal more damage with spells
        return base.ComputeDamage(target) + (Mana >= SPELL_COST ? _strength : 0);
    }
}