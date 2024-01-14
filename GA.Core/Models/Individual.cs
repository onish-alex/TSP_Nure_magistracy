using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace GA.Core.Models
{
	public class Individual<TGene> : IList<TGene>
	{
		private readonly List<TGene> genotype;

		public Individual(IEnumerable<TGene> genes)
		{
			genotype = genes.ToList();
		}

		public TGene this[int index] { get => genotype[index]; set => genotype[index] = value; }

		public int Count => genotype.Count;

		public bool IsReadOnly => ((ICollection<TGene>)genotype).IsReadOnly;

		/// <summary>
		/// return instance of Individual (or inherited class), using the constructor with IList<TGene> parameter
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="genes"></param>
		/// <returns></returns>
		public static T GetInstance<T>(IList<TGene> genes) where T : Individual<TGene>
		{
			return Activator.CreateInstance(typeof(T), new object[] { genes }) as T;
		}

		public void RemoveRange(int index, int count)
		{
			genotype.RemoveRange(index, count);
		}

		public List<TGene> GetRange(int index, int count)
		{
			var range = genotype.GetRange(index, count);
			return range;
		}

		public void Add(TGene item)
		{
			genotype.Add(item);
		}

		public void Clear()
		{
			genotype.Clear();
		}

		public bool Contains(TGene item)
		{
			return genotype.Contains(item);
		}

		public void CopyTo(TGene[] array, int arrayIndex)
		{
			genotype.CopyTo(array, arrayIndex);
		}

		public IEnumerator<TGene> GetEnumerator()
		{
			return genotype.GetEnumerator();
		}

		public int IndexOf(TGene item)
		{
			return genotype.IndexOf(item);
		}

		public void Insert(int index, TGene item)
		{
			genotype.Insert(index, item);
		}

		public bool Remove(TGene item)
		{
			return genotype.Remove(item);
		}

		public void RemoveAt(int index)
		{
			genotype.RemoveAt(index);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable)genotype).GetEnumerator();
		}

		public class IndividualComparer : IEqualityComparer<Individual<TGene>>
		{
			public bool Equals(Individual<TGene> x, Individual<TGene> y)
			{
				if (x.Equals(y))
					return true;

				return x.genotype.SequenceEqual(y.genotype);
			}

			public int GetHashCode([DisallowNull] Individual<TGene> obj)
			{
				var hc = 0;
				foreach (var item in obj.genotype)
					hc ^= item.GetHashCode();

				return hc;
			}
		}
	}
}
