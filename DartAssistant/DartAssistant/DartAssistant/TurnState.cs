namespace DartAssistant
{
    public enum TurnState
    {
        /// <summary>
        /// Indicates that the turn has not be started yet.
        /// </summary>
        NotStarted,
        /// <summary>
        /// Indicates that the turn is in progress.
        /// </summary>
        InProgress,
        /// <summary>
        /// Indicates that the turn is over without resulting in a bust or win.
        /// </summary>
        Done,
        /// <summary>
        /// Indicates that the turn resulted in a bust (can occur on any dart).
        /// </summary>
        Bust,
        /// <summary>
        /// Indicates that the turn resulted in a win (can occur on any dart).
        /// </summary>
        Win
    }
}
