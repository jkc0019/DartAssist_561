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

        #endregion
    }
}
