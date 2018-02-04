using System;

namespace SpanTest.RealWorldCases
{
    public class LevenshteinDistance
    {
        public static unsafe int DistanceUnsafe(string source, string target)
        {
            var v0 = stackalloc int[target.Length + 1];
            var v1 = stackalloc int[target.Length + 1];

            for (var i = 0; i < target.Length; i++)
            {
                v0[i] = i;
            }

            for (var i = 0; i < source.Length; i++)
            {
                v1[0] = i + 1;

                for (var j = 0; j < target.Length; j++)
                {
                    var deletionCost = v0[j + 1] + 1;
                    var insertionCost = v1[j] + 1;
                    int substitutionCost;
                    if (source[i] == target[j])
                    {
                        substitutionCost = v0[j];
                    }
                    else
                    {
                        substitutionCost = v0[j] + 1;
                    }

                    v1[j + 1] = Math.Min(Math.Min(deletionCost, insertionCost), substitutionCost);
                }

                var vt = v0;
                v0 = v1;
                v1 = vt;
            }

            return v0[target.Length];
        }

        public static int DistanceSpan(string source, string target)
        {
            Span<int> v0 = stackalloc int[target.Length + 1];
            Span<int> v1 = stackalloc int[target.Length + 1];

            for (var i = 0; i < v0.Length; i++)
            {
                v0[i] = i;
            }

            for (var i = 0; i < source.Length; i++)
            {
                v1[0] = i + 1;

                for (var j = 0; j < v0.Length - 1; j++)
                {
                    var deletionCost = v0[j + 1] + 1;
                    var insertionCost = v1[j] + 1;
                    int substitutionCost;
                    if (source[i] == target[j])
                    {
                        substitutionCost = v0[j];
                    }
                    else
                    {
                        substitutionCost = v0[j] + 1;
                    }

                    v1[j + 1] = Math.Min(Math.Min(deletionCost, insertionCost), substitutionCost);
                }

                var vt = v0;
                v0 = v1;
                v1 = vt;
            }

            return v0[target.Length];
        }
    }
}