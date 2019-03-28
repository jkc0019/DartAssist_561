namespace DartAssistant
{
    /// <summary>
    /// Represents an individuals turn at an 01 game of darts (3 darts).
    /// </summary>
    public class Turn
    {
        #region Member and Properties

        // Constants
        public const string START_KEY_WORD = "start";
        public const string SCORED_KEY_WORD = "scored";

        /// <summary>
        /// Gets the current score after any number of darts has been recorded.
        /// </summary>
        public int CurrentScore { get; set; }

        /// <summary>
        /// Gets the number of darts remaining to be thrown/recorded for the turn.
        /// </summary>
        public int DartsRemaining { get; set; }

        /// <summary>
        /// Gets the Out Rule that is being utilized for the turn.
        /// </summary>
        public InOutRule Rule { get; set; }

        /// <summary>
        /// Gets the value of the score at the start of the turn.
        /// </summary>
        public int StartingScore { get; set; }

        /// <summary>
        /// Gets the value that indicates the current state of the turn.
        /// </summary>
        public TurnState State { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor to allow for Deserialization.
        /// </summary>
        public Turn()
        {

        }

        /// <summary>
        /// Constructor to allow for setting of the InOutRule during initialization.
        /// </summary>
        /// <param name="rule">The Out rule to follow for calculations.</param>
        public Turn(InOutRule rule)
        {
            Rule = rule;
        }

        #endregion

        #region Methods

        public bool RecordPointsScored(string value)
        {
            // Split the given value on spaces.
            string[] parts = value.Split(' ');

            // Make sure there are at least two parts, the second part matches the keyword, and the first part can be parsed to an int.
            if (2 <= parts.Length && SCORED_KEY_WORD == parts[1] && int.TryParse(parts[0], out int outScore))
            {
                // Record the score.
                return RecordPointsScored(outScore);
            }

            // Unable to parse string, return false.
            return false;
        }

        /// <summary>
        /// Takes the points scored and updates the current score. Also determines the state of the turn based on result of the calculation.
        /// </summary>
        /// <param name="points">The points scored by the player.</param>
        /// <returns>True if the points are can be applied to the turn, otherwise false.</returns>
        public bool RecordPointsScored(int points)
        {
            // Make sure the state of the turn is appropriate for recording a score.
            // Make sure the points provided are valid for one dart.
            if(TurnState.InProgress != State || !Dart.IsValidScore(points)) { return false; }

            // Update the current score.
            CurrentScore -= points;

            // Check for possible win.
            if(0 == CurrentScore)
            {
                DartsRemaining = 0;
                State = Dart.IsValidDouble(points) ? TurnState.Win : TurnState.Bust;
                return true;
            }
            // Check for Bust scenarios.
            else if(1 == CurrentScore || 0 > CurrentScore)
            {
                DartsRemaining = 0;
                State = TurnState.Bust;
                return true;
            }

            // Update the count of remaining darts.
            DartsRemaining--;

            // Check to see if the turn is done because all darts have been thrown.
            if(0 == DartsRemaining)
            {
                State = TurnState.Done;
            }

            return true;
        }

        /// <summary>
        /// Sets the starting score and "starts"/"resets" the turn.
        /// </summary>
        /// <param name="score">The score at the start of the turn.</param>
        /// <returns>True if the score can successfully be set, otherwise false.</returns>
        public bool SetStartingScore(int score)
        {
            if(0 >= score) { return false; }

            StartingScore = score;
            CurrentScore = score;
            DartsRemaining = 3;
            State = TurnState.InProgress;
            return true;
        }

        /// <summary>
        /// Sets the starting score and "starts"/"resets" the turn.
        /// </summary>
        /// <param name="value">The string of the starting score command. example "170 start"</param>
        /// <returns>True if the score can successfully be set, otherwise false.</returns>
        public bool SetStartingScore(string value)
        {
            // Split the given value on spaces.
            string[] parts = value.Split(' ');

            // Make sure there are at least two parts, the second part matches the keyword, and the first part can be parsed to an int.
            if(2 <= parts.Length && START_KEY_WORD == parts[1] && int.TryParse(parts[0], out int outScore))
            {
                // Set the starting score.
                return SetStartingScore(outScore);
            }

            // Unable to parse string, return false.
            return false;
        }

        #endregion
    }
}
