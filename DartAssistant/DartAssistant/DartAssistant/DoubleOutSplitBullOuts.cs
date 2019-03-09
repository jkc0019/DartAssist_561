using System.Collections.Generic;

namespace DartAssistant
{
    internal class DoubleOutSplitBullOuts: Dictionary<int, List<Dart>>
    {
        public DoubleOutSplitBullOuts()
        {
            Add(170, new List<Dart>
            {
                new Dart(20, SegmentMultiplier.Triple),
                new Dart(20, SegmentMultiplier.Triple),
                new Dart(25, SegmentMultiplier.Double)
            });

            Add(167, new List<Dart>
            {
                new Dart(20, SegmentMultiplier.Triple),
                new Dart(19, SegmentMultiplier.Triple),
                new Dart(25, SegmentMultiplier.Double)
            });

            Add(164, new List<Dart>
            {
                new Dart(20, SegmentMultiplier.Triple),
                new Dart(18, SegmentMultiplier.Triple),
                new Dart(25, SegmentMultiplier.Double)
            });

            Add(161, new List<Dart>
            {
                new Dart(20, SegmentMultiplier.Triple),
                new Dart(17, SegmentMultiplier.Triple),
                new Dart(25, SegmentMultiplier.Double)
            });

            Add(160, new List<Dart>
            {
                new Dart(20, SegmentMultiplier.Triple),
                new Dart(20, SegmentMultiplier.Triple),
                new Dart(20, SegmentMultiplier.Double)
            });
        }
    }
}
