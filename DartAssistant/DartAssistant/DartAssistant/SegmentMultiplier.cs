namespace DartAssistant
{
    /// <summary>
    /// Describes the scoring multiplier for a dart board segment.
    /// </summary>
    public enum SegmentMultiplier
    {
        /// <summary>
        /// Indicates that no dart board segment was hit.
        /// </summary>
        Miss = 0,
        /// <summary>
        /// Indicates that one of the Single scoring areas of a dart board segment was hit.
        /// </summary>
        Single,
        /// <summary>
        /// Indicates that the Double scoring areas of a dart board segment was hit.
        /// </summary>
        Double,
        /// <summary>
        /// Indicates that the Triple scoring areas of a dart board segment was hit.
        /// </summary>
        Triple
    }
}
