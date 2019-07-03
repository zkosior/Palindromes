namespace Palindromes
{
	using System;
	using System.Diagnostics.CodeAnalysis;
	using System.Linq;

	[ExcludeFromCodeCoverage]
	internal static class Program
	{
		private static void Main(string[] args)
		{
			if (args.Length == 0)
			{
				Console.WriteLine("Missing input string.");
				return;
			}

			var palindromes = Engine.FindNLargestPalindromes(args[0], 3);

			if (!palindromes.Any())
			{
				Console.WriteLine("No palindromes found.");
				return;
			}

			foreach (var p in palindromes)
			{
				Console.WriteLine($"Text: {p.Text}, Index: {p.Start}, Length: {p.Length}");
			}
		}
	}
}