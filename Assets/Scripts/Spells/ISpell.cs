public interface ISpell
{
    void Init(SpellCaster spell, float chargePercent = 1);
    void Dispose();
}
