namespace Palindromes.Tests
{
	using Xunit;

	public class EngineTests
	{
		[Theory]
		[InlineData("aasdfgfdsaf", 0, 10, 1, 9)]
		[InlineData("aaasdfgfdsaff", 1, 11, 2, 10)]
		[InlineData("aasdffdsaf", 0, 9, 1, 8)]
		public void FindsLargestPalindromeInGivenRange(
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