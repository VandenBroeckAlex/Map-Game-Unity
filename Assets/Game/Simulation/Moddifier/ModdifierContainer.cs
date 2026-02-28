using System.Collections.Generic;

public class ModdifierContainer
{
    public List<Modifier> activeModifiers = new();

    public float GetModifierValue(string statKey)
    {
        float additive = 0f;
        float multiplicative = 1f;

        foreach (var m in activeModifiers)
        {
            if (m.id != statKey)
                continue;  // skip unrelated modifiers

            switch (m.type)
            {
                case ModifierType.Additive:
                    additive += m.value;
                    break;

                case ModifierType.Multiplicative:
                    multiplicative *= 1 + (m.value / 100);
                    break;
            }
        }

        return (1 + additive) * multiplicative - 1;
    }

    public void UpdateModifiers(int timeintick)
    {
        for (int i = activeModifiers.Count - 1; i >= 0; i--)
        {
            if (activeModifiers[i].duration > 0)
            {
                activeModifiers[i].duration -= timeintick;
                if (activeModifiers[i].duration <= 0)
                    activeModifiers.RemoveAt(i);
            }
        }
    }

    public void AddModifier(Modifier mod)
    {
        activeModifiers.Add(mod);
    }
}