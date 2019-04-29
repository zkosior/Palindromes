namespace Palindromes
{
#pragma warning disable SA1008 // Opening parenthesis must be spaced correctly

	public static class Engine
	{
		public static (int, int) FindLargestPalindrome(
			string input,
			int startIndex,
			int endIndex)
		{
			var middle = (endIndex + startIndex) / 2;
			var hasMidpoint = ((endIndex + startIndex) % 2) == 0;
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

			return (i, j);
		}
	}

#pragma warning restore SA1008 // Opening parenthesis must be spaced correctly
}