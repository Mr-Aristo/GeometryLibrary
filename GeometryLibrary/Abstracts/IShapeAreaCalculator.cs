using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeometryLibrary.Abstracts
{
    public interface IShapeAreaCalculator<T> where T : IAreaCalculable
    {
        Task<double> CalculateAreaAsync(T shape);
    }
}
