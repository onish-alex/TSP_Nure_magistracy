using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GA.Core.Models
{
	public class Individual<TGene> : List<TGene>
	{
		private IList<TGene> genotype;

		public Individual(IEnumerable<TGene> genes) 
		{
			genotype = genes.ToList();
		}

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

		public virtual TGene this[int index] { get => genotype[index]; set => genotype[index] = value; }

		public virtual int Count => genotype.Count;

		public virtual bool IsReadOnly => genotype.IsReadOnly;

		public virtual void Add(TGene item)
		{
			genotype.Add(item);
		}

		public virtual void Clear()
		{
			genotype.Clear();
		}

		public virtual bool Contains(TGene item)
		{
			return genotype.Contains(item);
		}

		public virtual void CopyTo(TGene[] array, int arrayIndex)
		{
			genotype.CopyTo(array, arrayIndex);
		}

		public virtual IEnumerator<TGene> GetEnumerator()
		{
			return genotype.GetEnumerator();
		}

		public virtual int IndexOf(TGene item)
		{
			return genotype.IndexOf(item);
		}

		public virtual void Insert(int index, TGene item)
		{
			genotype.Insert(index, item);
		}

		public virtual bool Remove(TGene item)
		{
			return genotype.Remove(item);
		}

		public virtual void RemoveAt(int index)
		{
			genotype.RemoveAt(index);
		}
	}
}
