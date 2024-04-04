namespace TNNL.Events
{
    public class ShieldHealthUpdateEvent
    {
        public float PercentHealth;
        private static readonly ShieldHealthUpdateEvent Instance = new();
        private ShieldHealthUpdateEvent() { }
        public static void Dispatch(float percentHealth)
        {
            Instance.PercentHealth = percentHealth;
            EventAggregator.Publish(Instance);
        }

    }
}