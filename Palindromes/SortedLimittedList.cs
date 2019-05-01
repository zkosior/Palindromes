namespace Palindromes
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	public class SortedLimittedList<TKey, TValue>
		where TKey : IComparable<TKey>
	{
		private readonly SortedList<TKey, TValue> items;
		private readonly int size;

		public SortedLimittedList(int size)
		{
			this.items = new SortedList<TKey, TValue>(size);
			this.size = size;
		}

		public int Count => this.items.Count;

		public IList<TValue> Values => this.items.Values;

		public void Add(TKey key, TValue value)
		{
			if ((this.items.Keys.Count < this.size ||
				key.CompareTo(this.items.Keys.FirstOrDefault()) > 0) &&
				!this.items.ContainsKey(key))
			{
				if (this.items.Count == this.size)
				{
					this.items.RemoveAt(0);
				}

				this.items.Add(key, value);
			}
		}

		public TKey SmallestKey()
		{
			return this.items.Keys.FirstOrDefault();
		}
	}
}