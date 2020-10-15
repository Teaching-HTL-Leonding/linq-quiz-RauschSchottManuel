using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace LinqQuiz.Library
{
    public static class Quiz
    {
        /// <summary>
        /// Returns all even numbers between 1 and the specified upper limit.
        /// </summary>
        /// <param name="exclusiveUpperLimit">Upper limit (exclusive)</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Thrown if <paramref name="exclusiveUpperLimit"/> is lower than 1.
        /// </exception>
        public static int[] GetEvenNumbers(int exclusiveUpperLimit)
        {
            return exclusiveUpperLimit > 0 ? Enumerable.Range(1, (exclusiveUpperLimit - 2) > 0 ? exclusiveUpperLimit - 2 : 1)
                .Where(n => n % 2 == 0)
                .ToArray()
                : throw new ArgumentOutOfRangeException($"{exclusiveUpperLimit} is invalid");
        }

        /// <summary>
        /// Returns the squares of the numbers between 1 and the specified upper limit 
        /// that can be divided by 7 without a remainder (see also remarks).
        /// </summary>
        /// <param name="exclusiveUpperLimit">Upper limit (exclusive)</param>
        /// <exception cref="OverflowException">
        ///     Thrown if the calculating the square results in an overflow for type <see cref="System.Int32"/>.
        /// </exception>
        /// <remarks>
        /// The result is an empty array if <paramref name="exclusiveUpperLimit"/> is lower than 1.
        /// The result is in descending order.
        /// </remarks>
        public static int[] GetSquares(int exclusiveUpperLimit)
        {
            return exclusiveUpperLimit < Math.Sqrt(int.MaxValue) ? exclusiveUpperLimit > 0 ? Enumerable.Range(1, (exclusiveUpperLimit - 1))
                .Where(n => n % 7 == 0)
                .Select(n => n * n)
                .OrderByDescending(n => n)
                .ToArray()
                : Array.Empty<int>()
                : throw new OverflowException("Overflow detected");
        }

        /// <summary>
        /// Returns a statistic about families.
        /// </summary>
        /// <param name="families">Families to analyze</param>
        /// <returns>
        /// Returns one statistic entry per family in <paramref name="families"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     Thrown if <paramref name="families"/> is <c>null</c>.
        /// </exception>
        /// <remarks>
        /// <see cref="FamilySummary.AverageAge"/> is set to 0 if <see cref="IFamily.Persons"/>
        /// in <paramref name="families"/> is empty.
        /// </remarks>
        public static FamilySummary[] GetFamilyStatistic(IReadOnlyCollection<IFamily> families)
        {
            return families != null ? families.Select(family => family.Persons.Count != 0 ? new FamilySummary()
                {
                    FamilyID = family.ID,
                    NumberOfFamilyMembers = family.Persons.Count,
                    AverageAge = family.Persons.Average(p => p.Age)
                } 
                : new FamilySummary() { FamilyID = family.ID, NumberOfFamilyMembers = 0, AverageAge = 0})
                .ToArray()
                : throw new ArgumentNullException("families is null");
        }

        /// <summary>
        /// Returns a statistic about the number of occurrences of letters in a text.
        /// </summary>
        /// <param name="text">Text to analyze</param>
        /// <returns>
        /// Collection containing the number of occurrences of each letter (see also remarks).
        /// </returns>
        /// <remarks>
        /// Casing is ignored (e.g. 'a' is treated as 'A'). Only letters between A and Z are counted;
        /// special characters, numbers, whitespaces, etc. are ignored. The result only contains
        /// letters that are contained in <paramref name="text"/> (i.e. there must not be a collection element
        /// with number of occurrences equal to zero.
        /// </remarks>
        public static (char letter, int numberOfOccurrences)[] GetLetterStatistic(string text)
        {
            return text.ToUpper()
                .ToCharArray()
                .GroupBy(c => c)
                .Where(c => Regex.IsMatch(c.Key.ToString(), @"^[A-Z]+$"))
                .Select(c => (c.Key, c.Count()))
                .ToArray();
        }
    }
}
