using System.Collections.Generic;
using System.Linq;

namespace DartAssistant
{
    /// <summary>
    /// Provides calulations for outs to an 01 game of darts.
    /// </summary>
    public class OutCalculator
    {
        #region Members and Properties

        public InOutRule Rule { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Creates an instance of the OutCalculator
        /// </summary>
        /// <param name="outRule">Indicates the out rule to use to determine appropriate outs</param>
        public OutCalculator(InOutRule outRule)
        {
            Rule = outRule;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns the appropriate Dictionary of Outs for the given Out Rule.
        /// </summary>
        /// <returns>Dictionary with an int key and List of Darts as the value.</returns>
        public Dictionary<int, List<Dart>> GetAllOuts()
        {
            return new DoubleOutSplitBullOuts();
        }

        /// <summary>
        /// Returns the List of Darts that represent the recommend darts to throw to take the given score out.
        /// </summary>
        /// <param name="score">The score to find an out for.</param>
        /// <param name="dartsAvailable">The number of darts available to accomplish the out. Defaults to 3.</param>
        /// <returns>List of Darts that represents the recommended darts to throw to take out the given score. Will be empty list if there is no possible out.</returns>
        public List<Dart> GetDartsForOut(int score, int dartsAvailable = 3)
        {
            Dictionary<int, List<Dart>> outChart = new DoubleOutSplitBullOuts();

            List<Dart> outDarts = outChart.Where(x => x.Key == score && x.Value.Count <= dartsAvailable)
                .Select(x => x.Value)
                .SingleOrDefault();

            return outDarts ?? new List<Dart>();
        }

        #endregion
    }
}
