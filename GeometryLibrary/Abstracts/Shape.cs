using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeometryLibrary.Abstracts
{
    public abstract class Shape : IShape
    {
        public abstract string Name { get; }
    }
}
