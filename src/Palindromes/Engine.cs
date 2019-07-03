namespace Palindromes
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

#pragma warning disable SA1008 // Opening parenthesis must be spaced correctly
#pragma warning disable SA1009 // Closing parenthesis must be spaced correctly

	public static class Engine
	{
		public static IList<(string Text, int Index, int Length)> FindNLargestPalindromes(
			string input,
			int howMany)
		{
			if (InvalidInput(input)) return Array.Empty<(string, int, int)>();
			var found = new List<(int Start, int Length)>(howMany);

			FindPalindromes(input, howMany, found);

			return ParseResults(
				input,
				found.Select(p => (p.Start, p.Length)).ToList());
		}

		public static (string Text, int Start, int Length)? FindLargestPalindrome(
			string input,
			int startIndex,
			int endIndex)
		{
			if (string.IsNullOrWhiteSpace(input)) return null;
			var found = FindLargest(input, startIndex, endIndex - startIndex);
			if (!found.HasValue) return null;
			return (input.Substring(
				found.Value.Start,
				found.Value.Length + 1),
				found.Value.Start,
				found.Value.Length);
		}

		private static void FindPalindromes(
			string input,
			int howMany,
			List<(int Start, int Length)> found)
		{
			var spanInput = input.AsSpan();
			FindAndSave(spanInput, howMany, 0, input.Length - 1, found);

			for (int i = 1; i < input.Length - 2; i++)
			{
				if (NoPotentialForLargeEnoughPalindrome(
					found,
					howMany,
					input.Length - 1 - i)) break;

				FindAndSave(spanInput, howMany, 0, input.Length - 1 - i, found);
				FindAndSave(spanInput, howMany, i, input.Length - 1, found);
			}
		}

		private static (int Start, int Length)? FindLargest(
			ReadOnlySpan<char> input,
			int startIndex,
			int length)
		{
			var endIndex = startIndex + length;
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
						: (i, j - i);
				}
			}

			return (++i, --j - i);
		}

		private static bool NoPotentialForLargeEnoughPalindrome(
			List<(int Start, int Length)> found,
			int howMany,
			int maxPossibleLength)
		{
			return found.Count == howMany &&
				   found.FirstOrDefault().Length >= maxPossibleLength;
		}

		private static bool InvalidInput(string input)
		{
			return string.IsNullOrWhiteSpace(input) ||
				   input.Length == 1;
		}

		private static void FindAndSave(
			ReadOnlySpan<char> input,
			int maxSize,
			int start,
			int end,
			List<(int Start, int Length)> found)
		{
			var result = FindLargest(input, start, end - start);
			if (result.HasValue)
			{
				Add(
					input,
					maxSize,
					found,
					result.Value.Start,
					result.Value.Length + 1);
			}
		}

		private static void Add(
			ReadOnlySpan<char> input,
			int maxSize,
			List<(int Start, int Length)> found,
			int start,
			int length)
		{
			if (NeedsToBeSaved(input, maxSize, found, start, length))
			{
				if (found.Count == maxSize)
				{
					found.RemoveAt(0);
				}

				found.Insert(
					FindIndex(found, length),
					(start, length));
			}
		}

		private static int FindIndex(
			List<(int Start, int Length)> found,
			int length)
		{
			var i = 0;
			for (; i < found.Count; i++)
			{
				if (found[i].Length >= length) return i;
			}

			return i;
		}

		private static bool NeedsToBeSaved(
			ReadOnlySpan<char> input,
			int maxSize,
			List<(int Start, int Length)> found,
			int start,
			int length)
		{
			if (found.Count >= maxSize &&
				length <= found[0].Length)
			{
				return false;
			}

			var currentText = input.Slice(start, length);
			foreach (var f in found)
			{
				if (input.Slice(f.Start, f.Length)
					.Equals(currentText, StringComparison.Ordinal))
					return false;
			}

			return true;
		}

		private static List<(string, int, int)> ParseResults(
			string input,
			List<(int Start, int Length)> found)
		{
			return found
				.Select(p => (input.Substring(p.Start, p.Length), p.Start, p.Length))
				.Reverse()
				.ToList();
		}
	}

#pragma warning restore SA1009 // Closing parenthesis must be spaced correctly
#pragma warning restore SA1008 // Opening parenthesis must be spaced correctly
}