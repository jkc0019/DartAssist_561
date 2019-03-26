using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DartAssistant.Test
{
    [TestClass]
    public class TurnTests
    {
        #region Constructor and Defaults

        /// <summary>
        /// WHEN a Turn is instantianted
        /// THEN the Rule property will return the value passed in to the constructor
        /// </summary>
        [TestMethod]
        public void Constructor_Always_SetsRuleToPassedInValue()
        {
            // Arrange
            InOutRule expectedRule = InOutRule.Open;

            // Act
            Turn subject = new Turn(expectedRule);

            // Assert
            Assert.AreEqual(expectedRule, subject.Rule, "Rule was not the expected value");
        }

        /// <summary>
        /// WHEN a Turn is instantianted
        /// THEN the value of StartingScore will be zero
        /// </summary>
        [TestMethod]
        public void Constructor_Always_SetsStartingScoreToExpectedValue()
        {
            // Arrange

            // Act
            Turn subject = new Turn(InOutRule.Double);

            // Assert
            Assert.AreEqual(0, subject.StartingScore, "StartingScore was not the expected value");
        }

        /// <summary>
        /// WHEN a Turn is instantianted
        /// THEN the value of CurrentScore will be zero
        /// </summary>
        [TestMethod]
        public void Constructor_Always_SetsCurrentScoreToExpectedValue()
        {
            // Arrange

            // Act
            Turn subject = new Turn(InOutRule.Double);

            // Assert
            Assert.AreEqual(0, subject.CurrentScore, "CurrentScore was not the expected value");
        }

        /// <summary>
        /// WHEN a Turn is instantianted
        /// THEN the value of DartsRemaining will be zero
        /// </summary>
        [TestMethod]
        public void Constructor_Always_SetsDartsRemainingToExpectedValue()
        {
            // Arrange

            // Act
            Turn subject = new Turn(InOutRule.Double);

            // Assert
            Assert.AreEqual(0, subject.DartsRemaining, "DartsRemaining was not the expected value");
        }

        /// <summary>
        /// WHEN a Turn is instantianted
        /// THEN the value of State will be NotStarted
        /// </summary>
        [TestMethod]
        public void Constructor_Always_SetsStateToExpectedValue()
        {
            // Arrange

            // Act
            Turn subject = new Turn(InOutRule.Double);

            // Assert
            Assert.AreEqual(TurnState.NotStarted, subject.State, "State was not the expected value");
        }

        #endregion

        #region SetStartingScore(int)

        /// <summary>
        /// WHEN SetStartingScore(int) is called with a valid value
        /// THEN the value of StartingScore will be the value passed in and true returned.
        /// </summary>
        [TestMethod]
        public void SetStartingScore_ValidValue_SetsStartingScorePropertyAsExpected()
        {
            // Arrange
            Turn subject = new Turn(InOutRule.Double);
            int expectedScore = 10;

            // Act
            bool status = subject.SetStartingScore(expectedScore);

            // Assert
            Assert.IsTrue(status, "Did not return expected value");
            Assert.AreEqual(expectedScore, subject.StartingScore, "StartingScore was not the expected value");
        }

        /// <summary>
        /// WHEN SetStartingScore(int) is called with an invalid value
        /// THEN the value of StartingScore will be 0 and false returned.
        /// </summary>
        [TestMethod]
        public void SetStartingScore_InvalidValue_SetsStartingScorePropertyAsExpected()
        {
            // Arrange
            Turn subject = new Turn(InOutRule.Double);

            // Act
            bool status = subject.SetStartingScore(0);

            // Assert
            Assert.IsFalse(status, "Did not return expected value");
            Assert.AreEqual(0, subject.StartingScore, "StartingScore was not the expected value");
        }

        /// <summary>
        /// WHEN SetStartingScore(int) is called with a valid value
        /// THEN the value of State will be updated to InProgress
        /// </summary>
        [TestMethod]
        public void SetStartingScore_ValidValue_SetsStatePropertyAsExpected()
        {
            // Arrange
            Turn subject = new Turn(InOutRule.Double);

            // Act
            subject.SetStartingScore(10);

            // Assert
            Assert.AreEqual(TurnState.InProgress, subject.State, "State was not the expected value");
        }

        /// <summary>
        /// WHEN SetStartingScore(int) is called with a valid value
        /// THEN the value of CurrentScore will be the expected value
        /// </summary>
        [TestMethod]
        public void SetStartingScore_ValidValue_SetsCurrentScorePropertyAsExpected()
        {
            // Arrange
            Turn subject = new Turn(InOutRule.Double);

            // Act
            subject.SetStartingScore(10);

            // Assert
            Assert.AreEqual(10, subject.CurrentScore, "CurrentScore was not the expected value");
        }

        /// <summary>
        /// WHEN SetStartingScore(int) is called with a valid value
        /// THEN the value of DartsRemaining will be the expected value
        /// </summary>
        [TestMethod]
        public void SetStartingScore_ValidValue_SetsDartsRemainingPropertyAsExpected()
        {
            // Arrange
            Turn subject = new Turn(InOutRule.Double);

            // Act
            subject.SetStartingScore(10);

            // Assert
            Assert.AreEqual(3, subject.DartsRemaining, "DartsRemaining was not the expected value");
        }

        /// <summary>
        /// WHEN SetStartingScore(int) is called after points have been recorded
        /// THEN the properties will be set to their expected values.
        /// </summary>
        [TestMethod]
        public void SetStartingScore_PointsRecorded_SetsPropertiesAsExpected()
        {
            // Arrange
            Turn subject = new Turn(InOutRule.Double);
            subject.SetStartingScore(20);
            subject.RecordPointsScored(20);

            // Act
            subject.SetStartingScore(10);

            // Assert
            Assert.AreEqual(10, subject.StartingScore, "StartingScore was not the expected value");
            Assert.AreEqual(TurnState.InProgress, subject.State, "State was not the expected value");
            Assert.AreEqual(10, subject.CurrentScore, "CurrentScore was not the expected value");
            Assert.AreEqual(3, subject.DartsRemaining, "DartsRemaining was not the expected value");
        }

        #endregion

        #region SetStartingScore(string)

        /// <summary>
        /// WHEN SetStartingScore(string) is called with a valid value
        /// THEN true is returned.
        /// </summary>
        [TestMethod]
        public void SetStartingScoreString_ValidValue_ReturnsTrue()
        {
            // Arrange
            Turn subject = new Turn(InOutRule.Double);
            string value = string.Format("170 {0}", Turn.START_KEY_WORD);

            // Act
            bool status = subject.SetStartingScore(value);

            // Assert
            Assert.IsTrue(status, "Did not return expected value");
        }

        /// <summary>
        /// WHEN SetStartingScore(string) is called with an invalid int value
        /// THEN false is returned.
        /// </summary>
        [TestMethod]
        public void SetStartingScoreString_InvalidIntValue_ReturnsFalse()
        {
            // Arrange
            Turn subject = new Turn(InOutRule.Double);
            string value = string.Format("big {0}", Turn.START_KEY_WORD);

            // Act
            bool status = subject.SetStartingScore(value);

            // Assert
            Assert.IsFalse(status, "Did not return expected value");
        }

        /// <summary>
        /// WHEN SetStartingScore(string) is called without keyword
        /// THEN false is returned.
        /// </summary>
        [TestMethod]
        public void SetStartingScoreString_MissingKeyWord_ReturnsFalse()
        {
            // Arrange
            Turn subject = new Turn(InOutRule.Double);
            string value = "170 phat";

            // Act
            bool status = subject.SetStartingScore(value);

            // Assert
            Assert.IsFalse(status, "Did not return expected value");
        }

        /// <summary>
        /// WHEN SetStartingScore(string) is called with a valid value
        /// THEN properties are updated as expected.
        /// </summary>
        [TestMethod]
        public void SetStartingScoreString_ValidValue_PropertiesAreUpdatedAsExpected()
        {
            // Arrange
            Turn subject = new Turn(InOutRule.Double);
            string value = string.Format("170 {0}", Turn.START_KEY_WORD);

            // Act
            subject.SetStartingScore(value);

            // Assert
            Assert.AreEqual(170, subject.StartingScore, "StartingScore was not expected value");
            Assert.AreEqual(TurnState.InProgress, subject.State, "State was not the expected value");
            Assert.AreEqual(170, subject.CurrentScore, "CurrentScore was not the expected value");
            Assert.AreEqual(3, subject.DartsRemaining, "DartsRemaining was not the expected value");
        }

        #endregion

        #region RecordPointsScored(int)

        /// <summary>
        /// WHEN RecordPointsScored(int) is called with a valid value
        /// THEN the value of CurrentPoints will be updated as expected and true returned.
        /// </summary>
        [TestMethod]
        public void RecordPointsScored_ValidValue_SetsCurrentPointsAsExpected()
        {
            // Arrange
            Turn subject = new Turn(InOutRule.Double);
            subject.SetStartingScore(10);

            // Act
            bool status = subject.RecordPointsScored(4);

            // Assert
            Assert.IsTrue(status, "Unexpected value returned");
            Assert.AreEqual(6, subject.CurrentScore, "CurrentScore was not the expected value");
        }

        /// <summary>
        /// WHEN RecordPointsScored(int) is called with a valid value less than the current score
        /// THEN the value of State will remain InProgress.
        /// </summary>
        [TestMethod]
        public void RecordPointsScored_ValidValueLessThanCurrentScore_SetsStateAsExpected()
        {
            // Arrange
            Turn subject = new Turn(InOutRule.Double);
            subject.SetStartingScore(10);

            // Act
            subject.RecordPointsScored(4);

            // Assert
            Assert.AreEqual(TurnState.InProgress, subject.State, "State was not expected value");
        }

        /// <summary>
        /// WHEN RecordPointsScored(int) is called with a valid value less than the current score
        /// THEN the value of DartsRemaining will be updated.
        /// </summary>
        [TestMethod]
        public void RecordPointsScored_ValidValueLessThanCurrentScore_UpdatesDartsRemainingAsExpected()
        {
            // Arrange
            Turn subject = new Turn(InOutRule.Double);
            subject.SetStartingScore(10);

            // Act
            subject.RecordPointsScored(4);

            // Assert
            Assert.AreEqual(2, subject.DartsRemaining, "DartsRemaining was not expected value");
        }

        /// <summary>
        /// WHEN RecordPointsScored(int) is called three times with a sum less than the current score
        /// THEN the values of DartsRemaining and State will be updated as expected.
        /// </summary>
        [TestMethod]
        public void RecordPointsScored_CalledThreeTimesSumLessThanCurrentScore_UpdatesDartsRemainingAndStateAsExpected()
        {
            // Arrange
            Turn subject = new Turn(InOutRule.Double);
            subject.SetStartingScore(10);

            // Act
            subject.RecordPointsScored(2);
            subject.RecordPointsScored(2);
            subject.RecordPointsScored(2);

            // Assert
            Assert.AreEqual(0, subject.DartsRemaining, "DartsRemaining was not expected value");
            Assert.AreEqual(TurnState.Done, subject.State, "State was not the expected value");
        }

        /// <summary>
        /// WHEN RecordPointsScored(int) is called when State == Done
        /// THEN false will be returned
        /// </summary>
        [TestMethod]
        public void RecordPointsScored_CalledWhenStateIsDone_ReturnsFalse()
        {
            // Arrange
            Turn subject = new Turn(InOutRule.Double);
            subject.SetStartingScore(10);

            // Act
            subject.RecordPointsScored(1);
            subject.RecordPointsScored(1);
            subject.RecordPointsScored(1);
            bool status = subject.RecordPointsScored(1);

            // Assert
            Assert.IsFalse(status, "Unexpected status returned");
        }

        /// <summary>
        /// WHEN RecordPointsScored(int) is called with an invalid value
        /// THEN properties will remain as expected and false will be returned
        /// </summary>
        [TestMethod]
        public void RecordPointsScored_InvalidValue_PropertiesRemainAndFalseReturned()
        {
            // Arrange
            Turn subject = new Turn(InOutRule.Double);
            subject.SetStartingScore(40);

            // Act
            bool status = subject.RecordPointsScored(23);

            // Assert
            Assert.IsFalse(status, "Expected status to be false");
            Assert.AreEqual(3, subject.DartsRemaining, "DartsRemaining was not expected value");
            Assert.AreEqual(TurnState.InProgress, subject.State, "State was not expected value");
            Assert.AreEqual(40, subject.CurrentScore, "CurrentScore was not expected value");
        }

        /// <summary>
        /// WHEN RecordPointsScored(int) is called with a valid double value equal to the CurrentScore
        /// THEN all properties are set as expected.
        /// </summary>
        [TestMethod]
        public void RecordPointsScored_ValidDoubleValueEqualToCurrentScore_AllPropertiesSetAsExpected()
        {
            // Arrange
            Turn subject = new Turn(InOutRule.Double);
            subject.SetStartingScore(10);

            // Act
            subject.RecordPointsScored(10);

            // Assert
            Assert.AreEqual(TurnState.Win, subject.State, "State was not expected value");
            Assert.AreEqual(0, subject.DartsRemaining, "DartsRemaining was not expeted value");
            Assert.AreEqual(0, subject.CurrentScore, "CurrentScore was not expected value");
        }

        /// <summary>
        /// WHEN RecordPointsScored(int) is called with a value that is not a valid double but is equal to the CurrentScore
        /// THEN all properties are set as expected
        /// </summary>
        [TestMethod]
        public void RecordPointsScored_NonDoubleValueEqualToCurrentScore_AllPropertiesSetAsExpected()
        {
            // Arrange
            Turn subject = new Turn(InOutRule.Double);
            subject.SetStartingScore(11);

            // Act
            subject.RecordPointsScored(11);

            // Assert
            Assert.AreEqual(TurnState.Bust, subject.State, "State was not expected value");
            Assert.AreEqual(0, subject.DartsRemaining, "DartsRemaining was not expeted value");
            Assert.AreEqual(0, subject.CurrentScore, "CurrentScore was not expected value");
        }

        /// <summary>
        /// WHEN RecordPointsScored(int) is called with a value that results in a CurrentScore of 1
        /// THEN all properties are set as expected
        /// </summary>
        [TestMethod]
        public void RecordPointsScored_CurrentScoreBecomes1_AllPropertiesSetAsExpected()
        {
            // Arrange
            Turn subject = new Turn(InOutRule.Double);
            subject.SetStartingScore(11);

            // Act
            subject.RecordPointsScored(10);

            // Assert
            Assert.AreEqual(TurnState.Bust, subject.State, "State was not expected value");
            Assert.AreEqual(0, subject.DartsRemaining, "DartsRemaining was not expeted value");
            Assert.AreEqual(1, subject.CurrentScore, "CurrentScore was not expected value");
        }

        /// <summary>
        /// WHEN RecordPointsScored(int) is called with a value that results in a CurrentScore less than zero
        /// THEN all properties are set as expected
        /// </summary>
        [TestMethod]
        public void RecordPointsScored_CurrentScoreLessThanZero_AllPropertiesSetAsExpected()
        {
            // Arrange
            Turn subject = new Turn(InOutRule.Double);
            subject.SetStartingScore(11);

            // Act
            subject.RecordPointsScored(12);

            // Assert
            Assert.AreEqual(TurnState.Bust, subject.State, "State was not expected value");
            Assert.AreEqual(0, subject.DartsRemaining, "DartsRemaining was not expeted value");
            Assert.AreEqual(-1, subject.CurrentScore, "CurrentScore was not expected value");
        }

        #endregion

        #region RecordPointsScored(string)

        /// <summary>
        /// WHEN RecordPointsScored(string) is called with a valid value
        /// THEN true is returned.
        /// </summary>
        [TestMethod]
        public void RecordPointsScoredString_ValidValue_ReturnsTrue()
        {
            // Arrange
            Turn subject = new Turn(InOutRule.Double);
            subject.SetStartingScore(170);
            string value = string.Format("60 {0}", Turn.SCORED_KEY_WORD);

            // Act
            bool status = subject.RecordPointsScored(value);

            // Assert
            Assert.IsTrue(status, "Did not return expected value");
        }

        /// <summary>
        /// WHEN RecordPointsScored(string) is called with an invalid int value
        /// THEN false is returned.
        /// </summary>
        [TestMethod]
        public void RecordPointsScoredString_InvalidIntValue_ReturnsFalse()
        {
            // Arrange
            Turn subject = new Turn(InOutRule.Double);
            subject.SetStartingScore(170);
            string value = string.Format("big {0}", Turn.SCORED_KEY_WORD);

            // Act
            bool status = subject.RecordPointsScored(value);

            // Assert
            Assert.IsFalse(status, "Did not return expected value");
        }

        /// <summary>
        /// WHEN RecordPointsScored(string) is called without keyword
        /// THEN false is returned.
        /// </summary>
        [TestMethod]
        public void RecordPointsScoredString_MissingKeyWord_ReturnsFalse()
        {
            // Arrange
            Turn subject = new Turn(InOutRule.Double);
            subject.SetStartingScore(170);
            string value = "60 phat";

            // Act
            bool status = subject.RecordPointsScored(value);

            // Assert
            Assert.IsFalse(status, "Did not return expected value");
        }

        /// <summary>
        /// WHEN RecordPointsScored(string) is called with a valid value
        /// THEN properties are updated as expected.
        /// </summary>
        [TestMethod]
        public void RecordPointsScoredString_ValidValue_PropertiesAreUpdatedAsExpected()
        {
            // Arrange
            Turn subject = new Turn(InOutRule.Double);
            subject.SetStartingScore(40);
            string value = string.Format("40 {0}", Turn.SCORED_KEY_WORD);

            // Act
            subject.RecordPointsScored(value);

            // Assert
            Assert.AreEqual(TurnState.Win, subject.State, "State was not the expected value");
            Assert.AreEqual(0, subject.CurrentScore, "CurrentScore was not the expected value");
            Assert.AreEqual(0, subject.DartsRemaining, "DartsRemaining was not the expected value");
        }

        #endregion
    }
}
