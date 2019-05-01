namespace Palindromes.Tests
{
	using FluentAssertions;
	using System.Collections.Generic;
	using Xunit;

	public class EngineTests
	{
		public static IEnumerable<object[]> SuccessfulPalindromes()
		{
			yield return new object[] { "sqrrqabccbatudefggfedvwhijkllkjihxymnnmzpop", new List<string> { "hijkllkjih", "defggfed", "abccba" } };
		}

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
			var result = Engine.FindLargestPalindrome(input, startIndex, endIndex);

			Assert.Equal(palindromeStart, result.Value.Start);
			Assert.Equal(palindromeEnd, result.Value.End);
		}

		[Theory]
		[InlineData("a", 0, 0)]
		[InlineData("ab", 0, 1)]
		[InlineData("ab", 1, 1)]
		[InlineData("abc", 0, 2)]
		[InlineData("abc", 1, 1)]
		[InlineData("abcde", 1, 3)]
		[InlineData("", 0, 0)]
		[InlineData(" ", 0, 0)]
		[InlineData("   ", 0, 2)]
		public void WhenNoPalindrome_ReturnsNull(
			string input,
			int startIndex,
			int endIndex)
		{
			var result = Engine.FindLargestPalindrome(input, startIndex, endIndex);

			Assert.Null(result);
		}

		[Theory]
		[InlineData("aasdfgfdsaf", 1, 9)]
		[InlineData("aasdfgfdsaffff", 1, 9)]
		public void FindsPalindromeOutsideCenter(
			string input,
			int palindromeStart,
			int palindromeEnd)
		{
			(int start, int end) = Engine.FindPalindrome(input).Value;

			Assert.Equal(palindromeStart, start);
			Assert.Equal(palindromeEnd, end);
		}

		[Theory]
		[InlineData("abcdefghijk")]
		[InlineData("a")]
		[InlineData("")]
		public void WhenNoPalindromeAtAll_ReturnsNull(
			string input)
		{
			var result = Engine.FindPalindrome(input);

			Assert.Null(result);
		}

		[Theory]
		[InlineData("abcdefghijklmnopqrstuvwxyz")]
		public void WhenNoPalindromes_ReturnsEmptyCollection(string input)
		{
			var result = Engine.FindLargestPalindromes(input, 3);

			Assert.Empty(result);
		}

		[Theory]
		[MemberData(nameof(SuccessfulPalindromes))]
		public void WhenPalindromesExist_ReurnsThem(
			string input, IEnumerable<string> expectedResults)
		{
			var result = Engine.FindLargestPalindromes(input, 3);

			result.Should().Contain(expectedResults);
		}
	}
}