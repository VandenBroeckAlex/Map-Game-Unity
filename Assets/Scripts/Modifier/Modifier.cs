public class Modifier
{
    public string id;             // "pop_growth", "movement_speed", "tax_income", "morale"
    public int value;           
    public ModifierType type;     // Additive, Multiplicative, Override
    public float duration;
    public string title;
    public string description;
  
    public    Modifier(string id, int value, ModifierType type)
    {
    this.id = id;
    this.value = value;
    this.type = type;
    //this.duration = duration;
    //this.title = title;
    //this.description = description;
    }
}
public enum ModifierType
{
    Additive,        // +5% +10% = +15%
    Multiplicative,  // ×1.05 ×1.10
    Override         // direct replace (rare)
}
