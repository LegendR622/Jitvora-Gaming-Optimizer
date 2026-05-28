namespace GamingBooster_Pro
{
    /// <summary>Pro-Funktionen (Master-Key, Entwickler, Lifetime).</summary>
    internal static class RedlineFeatureGate
    {
        public static bool InAppDriverUpdateEnabled => RedlineAppData.Current.IsProActive;
    }
}
