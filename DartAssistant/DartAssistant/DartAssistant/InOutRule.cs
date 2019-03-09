namespace DartAssistant
{
    /// <summary>
    /// Describes the rule for an 01 game of darts that a player must follow to start(In)/finish(Out) the game.
    /// </summary>
    public enum InOutRule
    {
        /// <summary>
        /// Indicates that a user must hit a double to start(In)/finish(Out) the game. Also currently implies split bull configuration.
        /// </summary>
        Double,
        /// <summary>
        /// Indicates that a user can hit any segment and multiplier combination to start(In)/finish(Out) the game. Also currently implies single bull configuration.
        /// </summary>
        Open
    }
}
