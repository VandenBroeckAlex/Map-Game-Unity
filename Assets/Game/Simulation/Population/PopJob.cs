public class PopJob : IHaveTag
{
    public string type { get; }
    public int strata { get; }
    public string tag { get; set; }
    public PopJob(string type, int strata, string tag)
    {
        this.type = type;
        this.strata = strata;
        this.tag = tag;
    }
}