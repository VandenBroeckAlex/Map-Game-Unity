using UnityEngine;

public class Religion : IHaveTag
{
    public string tag { get; set; }
    public string name;

    public Religion(string tag, string name)
    {
        this.tag = tag;
        this.name = name;
    }
}
