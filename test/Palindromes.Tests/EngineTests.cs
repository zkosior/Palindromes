namespace Palindromes.Tests
{
	using FluentAssertions;
	using System.Collections.Generic;
	using System.Linq;
	using Xunit;

	[Trait("TestCategory", "Unit")]
	public class EngineTests
	{
		public static IEnumerable<object[]> SuccessfulPalindromes()
		{
			yield return new object[] { "asdfgfdsa", new List<(string, int, int)> { ("asdfgfdsa", 0, 9) } }; // 100% match with two central elements
			yield return new object[] { "asdfggfdsa", new List<(string, int, int)> { ("asdfggfdsa", 0, 10) } }; // 100% match with central element
			yield return new object[] { "sqrrqabccbatudefggfedvwhijkllkjihxymnnmzpop", new List<(string, int, int)> { ("hijkllkjih", 23, 10), ("defggfed", 13, 8), ("abccba", 5, 6) } }; // example from assignment
			yield return new object[] { "qasdfgfdsaf", new List<(string, int, int)> { ("asdfgfdsa", 1, 9) } }; // central match with margins
			yield return new object[] { "qasdfgfdsawert", new List<(string, int, int)> { ("asdfgfdsa", 1, 9) } }; // not central match
			yield return new object[] { "qwerabcabcbaoppoabcbacbatyu", new List<(string, int, int)> { ("abcabcbaoppoabcbacba", 4, 20), ("abcba", 7, 5) } }; // one palindrome inside another
			yield return new object[] { "qwabccbaoppoabqw", new List<(string, int, int)> { ("baoppoab", 6, 8), ("abccba", 2, 6) } }; // intersection of two palindromes
			yield return new object[] { "qwerabcbatyuasdabcbafgh", new List<(string, int, int)> { ("abcba", 4, 5) } }; // twice the same palindrome
			yield return new object[] { "qwerab c batyui", new List<(string, int, int)> { ("ab c ba", 4, 7) } }; // are those even allowed?
			yield return new object[] { "fffff", new List<(string, int, int)> { ("fffff", 0, 5), ("ffff", 0, 4), ("fff", 0, 3) } }; // this is a weird rule, but makes sense and excluding intersections requires additional requirements
			yield return new object[] { "qwerabcbatyuiqwewqopas", new List<(string, int, int)> { ("abcba", 4, 5), ("qwewq", 13, 5) } }; // same length, different palindromes
			yield return new object[] { "sqrrqabccbatudefggfedvwhijkllkjihxymnnmzpopoopop", new List<(string, int, int)> { ("hijkllkjih", 23, 10), ("defggfed", 13, 8), ("popoopop", 40, 8) } }; // more than three palindromes
		}

		[Theory]
		[InlineData("aasdfgfdsaf", 0, 10, "asdfgfdsa")]
		[InlineData("aaasdfgfdsaff", 1, 11, "asdfgfdsa")]
		[InlineData("aasdffdsaf", 0, 9, "asdffdsa")]
		[InlineData("hhhh", 0, 3, "hhhh")]
		public void FindsLargestPalindromeInGivenRange(
			string input,
			int startIndex,
			int endIndex,
			string largest)
		{
			var result = Engine.FindLargestPalindrome(input, startIndex, endIndex);

			Assert.Equal(largest, result.Value.Text);
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
			var result = Engine.FindNLargestPalindromes(input, 3);

			Assert.Empty(result);
		}

		[Theory]
		[MemberData(nameof(SuccessfulPalindromes))]
		public void FindsNLargestPalindromes(
			string input, IEnumerable<(string, int, int)> expectedResults)
		{
			var result = Engine.FindNLargestPalindromes(input, 3);

			result.Select(p => (p.Text, p.Index, p.Length))
				.Should().BeEquivalentTo(expectedResults);
		}
	}
}