
public class Research
{
    public string id;
    public string modifierID;
    public int modifierValue;

    public void ApplyResearch(Country country, Research r)
    {
        country.stats.modifiers.AddModifier(new Modifier(modifierID, r.modifierValue, ModifierType.Multiplicative));

    }
}
