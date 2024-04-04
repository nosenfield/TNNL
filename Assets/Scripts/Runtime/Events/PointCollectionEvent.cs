namespace TNNL.Events
{
    public class PointCollectionEvent
    {
        public int Points;
        public object AssociatedObject;
        private static readonly PointCollectionEvent Instance = new();
        private PointCollectionEvent() { }
        public static void Dispatch(int points, object associatedObject)
        {
            Instance.Points = points;
            Instance.AssociatedObject = associatedObject;
            EventAggregator.Publish(Instance);
        }

    }
}