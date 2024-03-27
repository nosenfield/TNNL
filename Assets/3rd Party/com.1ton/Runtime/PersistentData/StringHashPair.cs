namespace nosenfield.PersistentData
{
    [System.Serializable]
    internal class StringHashPair
    {
        internal StringHashPair(string dataString)
        {
            String = dataString;
            Hash = PersistentDataService.ComputeHash(dataString);
        }
        internal readonly string String;
        internal readonly string Hash;
    }
}