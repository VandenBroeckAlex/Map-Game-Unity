using Mono.Cecil;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifierContainer
{
    public List<Modifier> activeModifiers = new();

    public float GetModifiedValue(string statName, float baseValue)
    {
        float additive = 0f;
        float multiplier = 1f;

        foreach (var mod in activeModifiers)
        {
            foreach (var effect in mod.effects)
            {
                if (effect.affectedStat == statName)
                {
                    if (effect.type == ModifierType.Additive)
                        additive += effect.value;
                    else if (effect.type == ModifierType.Multiplicative)
                        multiplier *= (1 + effect.value);
                }
            }
        }

        return (baseValue + additive) * multiplier;
    }

    public void UpdateModifiers(float deltaTime)
    {
        for (int i = activeModifiers.Count - 1; i >= 0; i--)
        {
            if (activeModifiers[i].duration > 0)
            {
                activeModifiers[i].duration -= deltaTime;
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

