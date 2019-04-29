namespace Palindromes.Tests
{
	using System;
	using Xunit;

	public class EngineTests
	{
		[Theory]
		[InlineData("aasdfgfdsaf", 0, 9, 1, 8)]
		public void FindsLargestPalindromeWithCenterBetweenGivenRanges(
			string input,
			int startIndex,
			int endIndex,
			int palindromeStart,
			int palindromeEnd)
		{
			(int start, int end) = Engine
				.FindLargestPalindrome(input, startIndex, endIndex);

			Assert.Equal(palindromeStart, start);
			Assert.Equal(palindromeEnd, end);
		}
	}
}