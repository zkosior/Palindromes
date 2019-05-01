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
			if (startIndex == endIndex) return null;

			var middle = (endIndex + startIndex) / 2;
			var hasMidpoint = (endIndex + startIndex) % 2 == 0;
			int i = hasMidpoint ? middle - 1 : middle;
			int j = middle + 1;
			for (; i >= startIndex; i--, j++)
			{
				if (input[i] != input[j])
				{
					i++;
					j--;
					break;
				}
			}

			if (i >= j) return null;
			return (i, j);
		}

		public static (int Start, int End)? FindPalindrome(string input)
		{
			if (string.IsNullOrWhiteSpace(input)) return null;
			var zero = FindLargestPalindrome(input, 0, input.Length - 1);
			if (zero.HasValue) return zero.Value;

			for (int i = 1; i < input.Length - 2; i++)
			{
				var left = FindLargestPalindrome(input, 0, input.Length - 1 - i);
				if (left.HasValue) return left.Value;
				var right = FindLargestPalindrome(input, i, input.Length - 1);
				if (right.HasValue) return right.Value;
			}

			return null;
		}

		public static IEnumerable<string> FindLargestPalindromes(
			string input,
			int howMany)
		{
			if (string.IsNullOrWhiteSpace(input) ||
				input.Length == 1) return Array.Empty<string>();
			var found = new SortedLimittedList<int, (int Start, int End)>(howMany);

			var middle = FindLargestPalindrome(input, 0, input.Length - 1);
			if (middle.HasValue)
			{
				found.Add(middle.Value.End - middle.Value.Start, (middle.Value.Start, middle.Value.End));
			}

			for (int i = 1; i < input.Length - 2; i++)
			{
				if (found.Count == 3 &&
					found.SmallestKey() >= input.Length - 1 - i) break;
				var left = FindLargestPalindrome(input, 0, input.Length - 1 - i);
				if (left.HasValue)
					found.Add(left.Value.End - left.Value.Start, (left.Value.Start, left.Value.End));
				var right = FindLargestPalindrome(input, i, input.Length - 1);
				if (right.HasValue)
					found.Add(right.Value.End - right.Value.Start, (right.Value.Start, right.Value.End));
			}

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