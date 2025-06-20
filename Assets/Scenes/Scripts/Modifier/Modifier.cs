using System.Collections.Generic;

public enum ModifierTarget
{
    Country,
    Province,
    Pop
}

public enum ModifierType
{
    Additive,
    Multiplicative
}


[System.Serializable]
public class ModifierEffect
{
    public string affectedStat;     // e.g., "taxIncome", "productionOutput"
    public float value;
    public ModifierType type;       // Additive or Multiplicative
}

[System.Serializable]
public class Modifier
{
    public string id;
    public string name;
    public List<ModifierEffect> effects = new();


    public ModifierTarget target;
    public float duration; // in days or ticks; -1 for permanent

    // Optional: Source information (laws, events, buildings etc.)
    public string sourceId;

    public bool IsExpired(float currentTime)
    {
        return duration > 0 && currentTime >= duration;
    }
}

/*
var taxModifier = new Modifier
{
    id = "tech_tax_efficiency",
    name = "Improved Tax Collection",
    affectedStat = "taxIncome",
    value = 0.2f,
    type = ModifierType.Multiplicative,
    target = ModifierTarget.Country,
    duration = -1 // permanent
};

country.modifiers.AddModifier(taxModifier);

float actualTax = country.GetTaxIncome(baseTax);
*/

/*
 * Steam-Driven Reform (365 days)
- +10% Tax Income
- +20 Factory Output

 {
  "id": "steam_tax_factory_boost",
  "name": "Steam-Driven Reform",
  "target": "Country",
  "duration": 365,
  "sourceId": "tech_steamworks",
  "effects": [
    {
      "affectedStat": "taxIncome",
      "value": 0.1,
      "type": "Multiplicative"
    },
    {
      "affectedStat": "factoryOutput",
      "value": 0.2,
      "type": "Additive"
    }
  ]
}
 */