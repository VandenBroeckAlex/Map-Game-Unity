public class PopJob
{
    public string type { get; }
    public string strata { get; }

    public PopJob(string type, string defaultStrata)
    {
        this.type = type;
        strata = defaultStrata;
    }
}