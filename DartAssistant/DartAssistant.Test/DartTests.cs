using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace DartAssistant.Test
{
    [TestClass]
    public class DartTests
    {
        #region BaseValue Property

        /// <summary>
        /// WHEN a Dart is instantiated with valid BaseValue
        /// THEN the BaseValue property will return the value passed in to the constructor
        /// </summary>
        [TestMethod]
        public void BaseValueProperty_ValidBaseValue_ReturnsValueFromConstructor()
        {
            // Arrange
            int expectedBaseValue = 10;

            // Act
            Dart subject = new Dart(expectedBaseValue, SegmentMultiplier.Triple);

            // Assert
            Assert.AreEqual(expectedBaseValue, subject.BaseValue, "BaseValue was not the expected value");
        }

        /// <summary>
        /// WHEN a Dart is instantiated with valid BaseValue but with a SegmentMultiplier.Miss
        /// THEN the BaseValue property will return 0
        /// </summary>
        [TestMethod]
        public void BaseValueProperty_ValidBaseValueWithMiss_Returns0()
        {
            // Arrange
            int expectedBaseValue = 0;

            // Act
            Dart subject = new Dart(10, SegmentMultiplier.Miss);

            // Assert
            Assert.AreEqual(expectedBaseValue, subject.BaseValue, "BaseValue was not the expected value");
        }

        /// <summary>
        /// WHEN a Dart is attempted to be instantiated with a BaseValue of -1
        /// THEN an Exception will be thrown
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void BaseValueProperty_SetToNegative1_ThrowsAnException()
        {
            // Arrange
            int expectedBaseValue = -1;

            // Act
            Dart subject = new Dart(expectedBaseValue, SegmentMultiplier.Single);

            // Assert
            // Expect Exception - see method attribute.
        }

        /// <summary>
        /// WHEN a Dart is attempted to be instantiated with a BaseValue of 21
        /// THEN an Exception will be thrown
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void BaseValueProperty_SetTo21_ThrowsAnException()
        {
            // Arrange
            int expectedBaseValue = 21;

            // Act
            Dart subject = new Dart(expectedBaseValue, SegmentMultiplier.Single);

            // Assert
            // Expect Exception - see method attribute.
        }

        #endregion

        #region Multiplier Property

        /// <summary>
        /// WHEN a Dart is instantiated with valid baseValue and multiplier
        /// THEN the Multiplier property will return the value passed in to the constructor
        /// </summary>
        [TestMethod]
        public void MultiplierProperty_ValidBaseValueAndMultiplier_ReturnsValueFromConstructor()
        {
            // Arrange
            SegmentMultiplier expectedMultiplier = SegmentMultiplier.Triple;

            // Act
            Dart subject = new Dart(5, expectedMultiplier);

            // Assert
            Assert.AreEqual(expectedMultiplier, subject.Multiplier, "Multiplier was not the expected value");
        }

        /// <summary>
        /// WHEN a Dart is instantiated with valid baseValue of 0 and multiplier other than miss
        /// THEN the Multiplier property will return Miss
        /// </summary>
        [TestMethod]
        public void MultiplierProperty_BaseValue0MultiplierNotMiss_ReturnsMiss()
        {
            // Arrange
            SegmentMultiplier expectedMultiplier = SegmentMultiplier.Miss;

            // Act
            Dart subject = new Dart(0, SegmentMultiplier.Single);

            // Assert
            Assert.AreEqual(expectedMultiplier, subject.Multiplier, "Multiplier was not the expected value");
        }

        /// <summary>
        /// WHEN a Dart is instantiated with valid baseValue of 25 and multiplier is Triple
        /// THEN the Multiplier property will return Double
        /// </summary>
        [TestMethod]
        public void MultiplierProperty_BaseValue25MultiplierTriple_ReturnsDouble()
        {
            // Arrange
            SegmentMultiplier expectedMultiplier = SegmentMultiplier.Double;

            // Act
            Dart subject = new Dart(25, SegmentMultiplier.Triple);

            // Assert
            Assert.AreEqual(expectedMultiplier, subject.Multiplier, "Multiplier was not the expected value");
        }

        #endregion

        #region Value Property

        /// <summary>
        /// WHEN a Dart is instantiated with a SegmentMultiplier.Miss
        /// THEN the Value property will always return 0
        /// </summary>
        [TestMethod]
        public void ValueProperty_DartWithMissMultiplier_Returns0()
        {
            // Arrange
            int expectedValue = 0;

            // Act
            Dart subject = new Dart(5, SegmentMultiplier.Miss);

            // Assert
            Assert.AreEqual(expectedValue, subject.Value, "Value was not the expected value");
        }

        /// <summary>
        /// WHEN a Dart is instantiated with a SegmentMultiplier.Single
        /// THEN the Value property will always return the BaseValue
        /// </summary>
        [TestMethod]
        public void ValueProperty_DartWithSingleMultiplier_ReturnsBaseValue()
        {
            // Arrange
            int expectedValue = 5;

            // Act
            Dart subject = new Dart(expectedValue, SegmentMultiplier.Single);

            // Assert
            Assert.AreEqual(expectedValue, subject.Value, "Value was not the expected value");
        }

        /// <summary>
        /// WHEN a Dart is instantiated with a SegmentMultiplier.Double
        /// THEN the Value property will always return the BaseValue multiplied by two
        /// </summary>
        [TestMethod]
        public void ValueProperty_DartWithDoubleMultiplier_ReturnsBaseValueTimesTwo()
        {
            // Arrange
            int baseValue = 5;
            int expectedValue = baseValue * 2;

            // Act
            Dart subject = new Dart(baseValue, SegmentMultiplier.Double);

            // Assert
            Assert.AreEqual(expectedValue, subject.Value, "Value was not the expected value");
        }

        /// <summary>
        /// WHEN a Dart is instantiated with a SegmentMultiplier.Triple
        /// THEN the Value property will always return the BaseValue multiplied by three
        /// </summary>
        [TestMethod]
        public void ValueProperty_DartWithTripleMultiplier_ReturnsBaseValueTimesTwo()
        {
            // Arrange
            int baseValue = 5;
            int expectedValue = baseValue * 3;

            // Act
            Dart subject = new Dart(baseValue, SegmentMultiplier.Triple);

            // Assert
            Assert.AreEqual(expectedValue, subject.Value, "Value was not the expected value");
        }

        #endregion

        #region SegmentName Property

        /// <summary>
        /// WHEN a Dart is instantiated with a BaseValue of 1
        /// THEN the SegmentName Property will return a string representation of the BaseValue.
        /// </summary>
        [TestMethod]
        public void SegmentNameProperty_BaseValueOf1_ReturnsBaseValueAsString()
        {
            // Arrange
            int baseValue = 1;

            // Act
            Dart subject = new Dart(baseValue, SegmentMultiplier.Double);

            // Assert
            Assert.AreEqual(baseValue.ToString(), subject.SegmentName, "SegmentName was not the expected value");
        }

        /// <summary>
        /// WHEN a Dart is instantiated with a BaseValue of 20
        /// THEN the SegmentName Property will return a string representation of the BaseValue.
        /// </summary>
        [TestMethod]
        public void SegmentNameProperty_BaseValueOf20_ReturnsBaseValueAsString()
        {
            // Arrange
            int baseValue = 20;

            // Act
            Dart subject = new Dart(baseValue, SegmentMultiplier.Double);

            // Assert
            Assert.AreEqual(baseValue.ToString(), subject.SegmentName, "SegmentName was not the expected value");
        }

        /// <summary>
        /// WHEN a Dart is instantiated with a BaseValue of 25
        /// THEN the SegmentName Property will return 'Bull'.
        /// </summary>
        [TestMethod]
        public void SegmentNameProperty_BaseValueOf25_ReturnsBull()
        {
            // Arrange
            int baseValue = 25;

            // Act
            Dart subject = new Dart(baseValue, SegmentMultiplier.Double);

            // Assert
            Assert.AreEqual("Bull", subject.SegmentName, "SegmentName was not the expected value");
        }

        /// <summary>
        /// WHEN a Dart is instantiated with a BaseValue of 0
        /// THEN the SegmentName Property will return 'Miss'.
        /// </summary>
        [TestMethod]
        public void SegmentNameProperty_BaseValueOf0_ReturnsMiss()
        {
            // Arrange
            int baseValue = 0;

            // Act
            Dart subject = new Dart(baseValue, SegmentMultiplier.Double);

            // Assert
            Assert.AreEqual("Miss", subject.SegmentName, "SegmentName was not the expected value");
        }

        #endregion
    }
}
