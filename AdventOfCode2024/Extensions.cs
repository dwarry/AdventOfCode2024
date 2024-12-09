using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2024;
internal static class Extensions
{
    /// <summary>
    /// Generates the Cartesian Product of an arbitrary set of sequences. 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="sequences"></param>
    /// <returns></returns>
    /// <remarks>See https://ericlippert.com/2010/06/28/computing-a-cartesian-product-with-linq/</remarks>
    public static IEnumerable<IEnumerable<T>> CartesianProduct<T>
        (this IEnumerable<IEnumerable<T>> sequences)
    {
        IEnumerable<IEnumerable<T>> emptyProduct = new[] { Enumerable.Empty<T>() };

        return sequences.Aggregate(
          emptyProduct,
          (accumulator, sequence) =>
            from accseq in accumulator
            from item in sequence
            select accseq.Concat(new[] { item }));
    }
}
