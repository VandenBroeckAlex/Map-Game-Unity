using UnityEngine;

public class Culture : IHaveTag 
{
    public string tag { get; set; }
    public string name;

    public Culture(string tag, string name)
    {
        this.tag = tag;
        this.name = name;
    }
}
