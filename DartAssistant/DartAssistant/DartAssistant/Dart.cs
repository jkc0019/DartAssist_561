using System;

namespace DartAssistant
{
    /// <summary>
    /// Represents a Dart that has been or is to be thrown including the board segment hit or to be hit.
    /// </summary>
    public class Dart
    {
        #region Members and Properties

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
    }
}
