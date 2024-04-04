namespace TNNL.Events
{
    public class ShieldInvincibilityUpdateEvent
    {
        public float SecondsInvincibleRemaining;
        private static readonly ShieldInvincibilityUpdateEvent Instance = new();
        private ShieldInvincibilityUpdateEvent() { }
        public static void Dispatch(float secondsInvincibleRemaining)
        {
            Instance.SecondsInvincibleRemaining = secondsInvincibleRemaining;
            EventAggregator.Publish(Instance);
        }

    }
}