namespace Palindromes
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

#pragma warning disable SA1008 // Opening parenthesis must be spaced correctly
#pragma warning disable SA1009 // Closing parenthesis must be spaced correctly

	public static class Engine
	{
		public static (int Start, int End)? FindLargestPalindrome(
			string input,
			int startIndex,
			int endIndex)
		{
			if (string.IsNullOrWhiteSpace(input)) return null;
			return FindLargest(input, startIndex, endIndex);
		}

		public static IEnumerable<string> FindLargestPalindromes(
			string input,
			int howMany)
		{
			if (InvalidInput(input)) return Array.Empty<string>();
			var found = new SortedLimittedList<int, (int Start, int End)>(howMany);

			FindAndSave(input, 0, input.Length - 1, found);

			for (int i = 1; i < input.Length - 2; i++)
			{
				if (NoPotentialForLargeEnoughPalindrome(
					found,
					howMany,
					input.Length - 1 - i)) break;

				FindAndSave(input, 0, input.Length - 1 - i, found);
				FindAndSave(input, i, input.Length - 1, found);
			}

			return ParseResults(input, found);
		}

		private static bool NoPotentialForLargeEnoughPalindrome(
			SortedLimittedList<int, (int Start, int End)> found,
			int lookingForHowMany,
			int maxPossibleLength)
		{
			return found.Count == lookingForHowMany &&
				   found.SmallestKey() >= maxPossibleLength;
		}

		private static bool InvalidInput(string input)
		{
			return string.IsNullOrWhiteSpace(input) ||
				   input.Length == 1;
		}

		private static void FindAndSave(
			string input,
			int start,
			int end,
			SortedLimittedList<int, (int Start, int End)> found)
		{
			var result = FindLargest(input, start, end);
			if (result.HasValue)
			{
				found.Add(
					result.Value.End - result.Value.Start,
					(result.Value.Start, result.Value.End));
			}
		}

		private static (int Start, int End)? FindLargest(
			string input,
			int startIndex,
			int endIndex)
		{
			if (startIndex == endIndex) return null;

			var middle = (endIndex + startIndex) / 2;
			var hasMidpoint = (endIndex + startIndex) % 2 == 0;
			int i = hasMidpoint ? middle - 1 : middle;
			int j = middle + 1;
			for (; i >= startIndex; i--, j++)
			{
				if (input[i] != input[j])
				{
					return ++i >= --j
						? default((int, int)?)
						: (i, j);
				}
			}

			return (++i, --j);
		}

		private static List<string> ParseResults(
			string input,
			SortedLimittedList<int, (int Start, int End)> found)
		{
			var result = new List<string>();
			foreach (var p in found.Values.Reverse())
			{
				result.Add(input.Substring(p.Start, p.End - p.Start + 1));
			}

			return result;
		}
	}

#pragma warning restore SA1009 // Closing parenthesis must be spaced correctly
#pragma warning restore SA1008 // Opening parenthesis must be spaced correctly
}