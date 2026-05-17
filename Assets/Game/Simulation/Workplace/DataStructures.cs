
public struct ProductionEffect
{
    public OutputDomain type { get; set; }
    public int targetId { get; set; } // "steel", "admin_points", etc.
    public float baseAmount { get; set; } // Produced per Size level at 100% efficiency
}

public struct ResourceRequirement
{
    public int goodId { get; set; }
    public float baseAmount { get; set; } // Consumed per Size level at 100% throughput
}

public struct ProductionIntent
{
    public int buildingId { get; set; }
    public string goodId { get; set; }

}