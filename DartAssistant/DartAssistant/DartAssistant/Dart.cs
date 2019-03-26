using System;
using System.Collections.Generic;
using System.Text;

namespace DartAssistant
{
    /// <summary>
    /// Represents a Dart that has been or is to be thrown including the board segment hit or to be hit.
    /// </summary>
    public class Dart
    {
        #region Members and Properties

        /// <summary>
        /// Gets an Abbreviation that describes the SegmentMulitplier and BaseValue of the dart board segment that is to be hit or was hit.
        /// </summary>
        public string Abbreviation
        {
            get
            {
                if(0 < BaseValue)
                {
                    StringBuilder sb = new StringBuilder();

                    switch (Multiplier)
                    {
                        case SegmentMultiplier.Single:
                            {
                                sb.Append("S");
                                break;
                            }
                        case SegmentMultiplier.Double:
                            {
                                sb.Append("D");
                                break;
                            }
                        case SegmentMultiplier.Triple:
                            {
                                sb.Append("T");
                                break;
                            }
                    }

                    if (1 <= BaseValue && 20 >= BaseValue)
                    {
                        sb.Append(BaseValue);
                    }
                    else if (25 == BaseValue)
                    {
                        sb.Append("B");
                    }

                    return sb.ToString();
                }

                return "Miss";
            }
        }

        /// <summary>
        /// Gets the Base Value of the dart board segment that is to be hit or was hit.
        /// </summary>
        public int BaseValue { get; protected set; }

        /// <summary>
        /// Gets the Multiplier of the scoring area of the dart board segment that is to be hit or was hit.
        /// </summary>
        public SegmentMultiplier Multiplier { get; protected set; }

        /// <summary>
        /// Gets the name of the dart board segment that was hit or is to be hit.
        /// </summary>
        public string SegmentName
        {
            get
            {
                if(25 == BaseValue)
                {
                    return "Bull";
                }
                else if(0 == BaseValue)
                {
                    return "Miss";
                }

                return BaseValue.ToString();
            }
        }

        /// <summary>
        /// Gets the point value of the dart board segment hit or is to be hit.
        /// </summary>
        public int Value
        {
            get
            {
                switch(Multiplier)
                {
                    case SegmentMultiplier.Single:
                        {
                            return BaseValue;
                        }
                    case SegmentMultiplier.Double:
                        {
                            return BaseValue * 2;
                        }
                    case SegmentMultiplier.Triple:
                        {
                            return BaseValue * 3;
                        }
                    default:
                        {
                            return 0;
                        }
                }
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Creates an instance of a Dart.
        /// </summary>
        /// <param name="baseValue">The number of the dart board segment. Can be 0-20 or 25. If multiplier is set to Miss, BaseValue will always be set to 0</param>
        /// <param name="multiplier">The scoring multiplier of the scoring area of the dart board segment. If BaseValue is set to 0, then Multiplier will be set to Miss</param>
        public Dart(int baseValue, SegmentMultiplier multiplier)
        {
            if ((0 <= baseValue && 20 >= baseValue) || 25 == baseValue)
            {
                BaseValue = SegmentMultiplier.Miss == multiplier ? 0 : baseValue;
                Multiplier = (0 == BaseValue && SegmentMultiplier.Miss != multiplier) ? SegmentMultiplier.Miss :
                    (25 == BaseValue && SegmentMultiplier.Triple == multiplier) ? SegmentMultiplier.Double : multiplier;
            }
            else
            {
                throw new ArgumentOutOfRangeException("baseValue", "baseValue must be 0 - 20 or 25");
            }
        }

        #endregion

        #region Methods

        public override string ToString()
        {
            if (0 < BaseValue)
            {
                StringBuilder sb = new StringBuilder();

                switch (Multiplier)
                {
                    case SegmentMultiplier.Single:
                        {
                            sb.Append("Single ");
                            break;
                        }
                    case SegmentMultiplier.Double:
                        {
                            sb.Append("Double ");
                            break;
                        }
                    case SegmentMultiplier.Triple:
                        {
                            sb.Append("Triple ");
                            break;
                        }
                }

                if (1 <= BaseValue && 20 >= BaseValue)
                {
                    sb.Append(BaseValue);
                }
                else if (25 == BaseValue)
                {
                    sb.Append("Bull");
                }

                return sb.ToString();
            }

            return "Miss";
        }

        /// <summary>
        /// Determines if the provided points are valid for a single dart.
        /// </summary>
        /// <param name="points">The value that is to be checked.</param>
        /// <returns>True if the points are valid, otherwise false.</returns>
        public static bool IsValidScore(int points)
        {
            // Account for miss, and bull values.
            List<int> validValues = new List<int> { 0, 25, 50 };

            // Loop through the others.
            for(int i = 1; i <= 20; i++)
            {
                Dart single = new Dart(i, SegmentMultiplier.Single);
                Dart dbl = new Dart(i, SegmentMultiplier.Double);
                Dart triple = new Dart(i, SegmentMultiplier.Triple);

                validValues.Add(single.Value);
                validValues.Add(dbl.Value);
                validValues.Add(triple.Value);
            }

            // Make the determination if it is a valid value.
            return validValues.Contains(points);
        }

        /// <summary>
        /// Determines if the provided points are a valid double value for a single dart.
        /// </summary>
        /// <param name="points">The points that are to be checked.</param>
        /// <returns>True if the points are valid, otherwise false.</returns>
        public static bool IsValidDouble(int points)
        {
            // List of the valid values.
            List<int> validDoubles = new List<int>();

            // Double bull.
            Dart dblBull = new Dart(25, SegmentMultiplier.Double);
            validDoubles.Add(dblBull.Value);

            // Loop through the others.
            for(int i = 1; i <= 20; i++)
            {
                Dart dbl = new Dart(i, SegmentMultiplier.Double);

                validDoubles.Add(dbl.Value);
            }

            // Make the determination if it is a valid double.
            return validDoubles.Contains(points);
        }

        #endregion
    }
}
