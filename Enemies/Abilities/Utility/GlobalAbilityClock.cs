public class GlobalAbilityClock
{
    public float TimeElapsed { get; private set; } = 0f;

    public void Tick(float deltaTime)
    {
        TimeElapsed += deltaTime;
    }

    public void Reset()
    {
        TimeElapsed = 0f;
    }
}
