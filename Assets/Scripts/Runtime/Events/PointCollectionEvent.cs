namespace TNNL.Events
{
    public class PointCollectionEvent
    {
        public static void Dispatch(int points, object associatedObject)
        {
            Instance.Points = points;
            Instance.AssociatedObject = associatedObject;
            EventAggregator.Publish(Instance);
        }

        private static readonly PointCollectionEvent Instance = new() { Points = 0, AssociatedObject = null };
        public int Points;
        public object AssociatedObject;
    }
}