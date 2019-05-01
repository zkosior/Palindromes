namespace Palindromes.Tests
{
	using FluentAssertions;
	using System.Collections.Generic;
	using Xunit;

	public class EngineTests
	{
		public static IEnumerable<object[]> SuccessfulPalindromes()
		{
			yield return new object[] { "asdfgfdsa", new List<string> { "asdfgfdsa" } }; // 100% match with two central elements
			yield return new object[] { "asdfggfdsa", new List<string> { "asdfggfdsa" } }; // 100% match with central element
			yield return new object[] { "sqrrqabccbatudefggfedvwhijkllkjihxymnnmzpop", new List<string> { "hijkllkjih", "defggfed", "abccba" } }; // example from assignment
			yield return new object[] { "qasdfgfdsaf", new List<string> { "asdfgfdsa" } }; // central match with margins
			yield return new object[] { "qasdfgfdsawert", new List<string> { "asdfgfdsa" } }; // not central match
			yield return new object[] { "qwerabcabcbaoppoabcbacbatyu", new List<string> { "abcabcbaoppoabcbacba", "abcba" } }; // one palindrome inside another
			yield return new object[] { "qwabccbaoppoabqw", new List<string> { "baoppoab", "abccba" } }; // intersection of two palindromes
			yield return new object[] { "qwerabcbatyuasdabcbafgh", new List<string> { "abcba" } }; // twice the same palindrome
		}

		[Theory]
		[InlineData("aasdfgfdsaf", 0, 10, 1, 9)]
		[InlineData("aaasdfgfdsaff", 1, 11, 2, 10)]
		[InlineData("aasdffdsaf", 0, 9, 1, 8)]
		[InlineData("hhhh", 0, 3, 0, 3)]
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
		[InlineData("abcdefghijklmnopqrstuvwxyz")]
		[InlineData("a")]
		[InlineData("")]
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

			result.Should().BeEquivalentTo(expectedResults);
		}
	}
}