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
			var found = new List<(int Start, int End, int Length)>(howMany);

			FindAndSave(input, howMany, 0, input.Length - 1, found);

			for (int i = 1; i < input.Length - 2; i++)
			{
				if (NoPotentialForLargeEnoughPalindrome(
					found,
					howMany,
					input.Length - 1 - i)) break;

				FindAndSave(input, howMany, 0, input.Length - 1 - i, found);
				FindAndSave(input, howMany, i, input.Length - 1, found);
			}

			return ParseResults(input, found);
		}

		private static bool NoPotentialForLargeEnoughPalindrome(
			List<(int Start, int End, int Length)> found,
			int lookingForHowMany,
			int maxPossibleLength)
		{
			return found.Count == lookingForHowMany &&
				   found.FirstOrDefault().Length >= maxPossibleLength;
		}

		private static bool InvalidInput(string input)
		{
			return string.IsNullOrWhiteSpace(input) ||
				   input.Length == 1;
		}

		private static void FindAndSave(
			string input,
			int maxSize,
			int start,
			int end,
			List<(int Start, int End, int Length)> found)
		{
			var result = FindLargest(input, start, end);
			if (result.HasValue)
			{
				Add(
					input,
					maxSize,
					found,
					result.Value.Start,
					result.Value.End);
			}
		}

		private static void Add(
			string input,
			int maxSize,
			List<(int Start, int End, int Length)> found,
			int start,
			int end)
		{
			var length = end - start + 1;
			if (NeedsToBeSaved(input, maxSize, found, start, length))
			{
				if (found.Count == maxSize)
				{
					found.RemoveAt(0);
				}

				found.Insert(
					FindIndex(found, length),
					(start, end, length));
			}
		}

		private static int FindIndex(
			List<(int Start, int End, int Length)> found,
			int length)
		{
			var i = 0;
			for (; i < found.Count; i++)
			{
				var currentItemLength = found[i].Length;
				if (currentItemLength >= length) return i;
			}

			return i;
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

		private static bool NeedsToBeSaved(
			string input,
			int maxSize,
			List<(int Start, int End, int Length)> found,
			int start,
			int length)
		{
			if (found.Count < maxSize ||
				length > found[0].Length)
			{
				var currentText = input.Substring(start, length);
				return !found.Any(p => input.Substring(p.Start, p.Length) == currentText);
			}

			return false;
		}

		private static List<string> ParseResults(
			string input,
			List<(int Start, int End, int Length)> found)
		{
			var result = new List<string>();
			foreach (var p in found)
			{
				result.Insert(0, input.Substring(p.Start, p.End - p.Start + 1));
			}

			return result;
		}
	}

#pragma warning restore SA1009 // Closing parenthesis must be spaced correctly
#pragma warning restore SA1008 // Opening parenthesis must be spaced correctly
}