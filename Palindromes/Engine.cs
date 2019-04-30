namespace Palindromes
{
#pragma warning disable SA1008 // Opening parenthesis must be spaced correctly
#pragma warning disable SA1009 // Closing parenthesis must be spaced correctly

	public static class Engine
	{
		public static (int, int)? FindLargestPalindrome(
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
					i++;
					j--;
					break;
				}
			}

			if (i >= j) return null;
			return (i, j);
		}
	}

#pragma warning restore SA1009 // Closing parenthesis must be spaced correctly
#pragma warning restore SA1008 // Opening parenthesis must be spaced correctly
}