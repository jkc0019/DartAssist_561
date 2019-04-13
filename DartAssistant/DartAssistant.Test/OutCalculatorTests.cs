using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace DartAssistant.Test
{
    [TestClass]
    public class OutCalculatorTests
    {
        #region Rule Property

        /// <summary>
        /// WHEN ever an OutCalculator is instantiated
        /// THEN the Rule property has the value that was passed into the constructor
        /// </summary>
        [TestMethod]
        public void RuleProperty_Always_ReturnsWhatWasPassedIntoConstructor()
        {
            // Arrange
            InOutRule expectedRule = InOutRule.Open;

            // Act
            OutCalculator subject = new OutCalculator(expectedRule);

            // Assert
            Assert.AreEqual(expectedRule, subject.Rule, "Rule was not the expected value");
        }

        #endregion

        #region GetAllOuts()

        /// <summary>
        /// WHEN ever GetAllOuts is called when the calculator rule is Double out
        /// THEN the expected outs are returned.
        /// </summary>
        [TestMethod]
        public void GetAllOuts_WithDoubleOutRule_GetsTheExpectedOuts()
        {
            // Arrange
            OutCalculator calculator = new OutCalculator(InOutRule.Double);

            // Act
            var subject = calculator.GetAllOuts();

            // Assert
            Assert.AreEqual("DoubleOutSplitBullOuts", subject.GetType().Name, "Not expected class");
        }

        /// <summary>
        /// WHEN ever GetAllOuts is called when the calculator rule is Double out
        /// THEN the math works out for all expected outs returned.
        /// </summary>
        [TestMethod]
        public void GetAllOuts_WithDoubleOutRule_MathOfExpectedOutsWorks()
        {
            // Arrange
            OutCalculator calculator = new OutCalculator(InOutRule.Double);

            // Act
            var subject = calculator.GetAllOuts();

            // Assert
            foreach(var keyValue in subject)
            {
                int total = 0;

                foreach(Dart dart in keyValue.Value)
                {
                    total += dart.Value;
                }

                Assert.AreEqual(keyValue.Key, total, string.Format("Total did not match for {0}", keyValue.Key));
                Assert.AreEqual(SegmentMultiplier.Double, keyValue.Value.LastOrDefault().Multiplier, "Last dart multiplier was not Double");
            }
        }


        #endregion

        #region GetDartsForOut(int)

        /// <summary>
        /// WHEN ever GetDartsForOut(170) is called when the calculator rule is Double out
        /// THEN the expected out is returned.
        /// </summary>
        [TestMethod]
        public void GetDartsForOut_170WithDoubleOutRule_GetsTheExpectedOut()
        {
            // Arrange
            OutCalculator calculator = new OutCalculator(InOutRule.Double);

            // Act
            List<Dart> subject = calculator.GetDartsForOut(170);

            // Assert
            Assert.AreEqual(3, subject.Count, "Unexpected number of darts");
            Assert.AreEqual("T20", subject[0].Abbreviation, "Unexpected first dart");
            Assert.AreEqual("T20", subject[1].Abbreviation, "Unexpected second dart");
            Assert.AreEqual("DB", subject[2].Abbreviation, "Unexpected third dart");
        }

        /// <summary>
        /// WHEN ever GetDartsForOut() is called with a number where there is no out
        /// THEN an empty List of Darts is returned.
        /// </summary>
        [TestMethod]
        public void GetDartsForOut_NoOutForProvidedScore_ReturnsEmptyList()
        {
            // Arrange
            OutCalculator calculator = new OutCalculator(InOutRule.Double);

            // Act
            List<Dart> subject = calculator.GetDartsForOut(181);

            // Assert
            Assert.IsNotNull(subject, "Return was null when not expected");
            Assert.AreEqual(0, subject.Count, "Unexpected number of darts");
        }

        /// <summary>
        /// WHEN ever GetDartsForOut() is called with a number that only takes 2 darts with default 3 darts remaining
        /// THEN a proper two dart out is returned.
        /// </summary>
        [TestMethod]
        public void GetDartsForOut_OnlyTwoDartOutWithThreeDarts_ReturnsExpectedTwoDartOut()
        {
            // Arrange
            OutCalculator calculator = new OutCalculator(InOutRule.Double);

            // Act
            List<Dart> subject = calculator.GetDartsForOut(60);

            // Assert
            Assert.IsNotNull(subject, "Return was null when not expected");
            Assert.AreEqual(2, subject.Count, "Unexpected number of darts");
        }

        /// <summary>
        /// WHEN ever GetDartsForOut() is called with darts remaining less than 3
        /// THEN an appropriate recommended out is returned.
        /// </summary>
        [TestMethod]
        public void GetDartsForOut_Provide2DartsRemaining_ReturnsExpectedOut()
        {
            // Arrange
            OutCalculator calculator = new OutCalculator(InOutRule.Double);

            // Act
            List<Dart> subject = calculator.GetDartsForOut(90, 2);

            // Assert
            Assert.IsNotNull(subject, "Return was null when not expected");
            Assert.AreEqual(2, subject.Count, "Unexpected number of darts");
            Assert.AreEqual("T18", subject[0].Abbreviation, "Unexpected second dart");
            Assert.AreEqual("D18", subject[1].Abbreviation, "Unexpected third dart");
        }

        /// <summary>
        /// WHEN ever GetDartsForOut() is called with darts remaining less than 3 with no matching out
        /// THEN an empty List of Darts is returned.
        /// </summary>
        [TestMethod]
        public void GetDartsForOut_Provide2DartsRemainingWithNoPossibleOut_ReturnsExpectedOut()
        {
            // Arrange
            OutCalculator calculator = new OutCalculator(InOutRule.Double);

            // Act
            List<Dart> subject = calculator.GetDartsForOut(121, 2);

            // Assert
            Assert.IsNotNull(subject, "Return was null when not expected");
            Assert.AreEqual(0, subject.Count, "Unexpected number of darts");
        }

        /// <summary>
        /// WHEN ever GetDartsForOut() is called with darts remaining less than 3
        /// THEN an appropriate recommended out is returned.
        /// </summary>
        [TestMethod]
        public void GetDartsForOut_Provide1DartsRemaining_ReturnsExpectedOut()
        {
            // Arrange
            OutCalculator calculator = new OutCalculator(InOutRule.Double);

            // Act
            List<Dart> subject = calculator.GetDartsForOut(50, 1);

            // Assert
            Assert.IsNotNull(subject, "Return was null when not expected");
            Assert.AreEqual(1, subject.Count, "Unexpected number of darts");
            Assert.AreEqual("DB", subject[0].Abbreviation, "Unexpected dart");
        }

        /// <summary>
        /// WHEN ever GetDartsForOut() is called with darts remaining less than 3 with no matching out
        /// THEN an empty List of Darts is returned.
        /// </summary>
        [TestMethod]
        public void GetDartsForOut_Provide1DartsRemainingWithNoPossibleOut_ReturnsExpectedOut()
        {
            // Arrange
            OutCalculator calculator = new OutCalculator(InOutRule.Double);

            // Act
            List<Dart> subject = calculator.GetDartsForOut(51, 1);

            // Assert
            Assert.IsNotNull(subject, "Return was null when not expected");
            Assert.AreEqual(0, subject.Count, "Unexpected number of darts");
        }

        #endregion
    }
}
