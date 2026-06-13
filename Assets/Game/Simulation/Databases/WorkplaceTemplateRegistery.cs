using System.Collections.Generic;

public class WorkplaceTemplateRegistery
{
    private List<WorkplaceTemplate> allTemplate = new List<WorkplaceTemplate>();

    public List<WorkplaceTemplate> GetAllTemplate()
    {
        return allTemplate;
    }

    //pass data -> check validity -> give list of template
    public void GetAvailableTemplate()
    {

    }
}
