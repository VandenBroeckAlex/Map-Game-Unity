public class PopulationSystem
{
    HandleGet getter = new HandleGet();
    IIntentBuffer _intent;

    public PopulationSystem(IntentBuffer intent)
    {
        _intent = intent;
    }
}