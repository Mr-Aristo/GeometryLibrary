using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeometryLibrary.Abstracts;
using Microsoft.Extensions.Logging;
using Serilog;
namespace GeometryLibrary.Concretes
{
    public class Circle : Shape, IAreaCalculable
    {
        public override string Name => "Circle";
        public double Radius { get; }

        public Circle(double radius)
        {
            if (radius <= 0)
            {
                Log.Error("Radius enterred not correctly! Radius must be greater than zero");
                throw new Exception("Radius must be greater than zero");
            }
            Radius = radius;

        }

        public double CalculateArea()
        {
            try
            {                
                var area = Math.PI * Radius * Radius;
                Log.Information("Calculated area of circle with radius {Radius}: {Area}", Radius, area);
                return area;

            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, "Something went wrong!");
                throw new InvalidOperationException("Something went wrong! " + ex.Message);
            }
        }

    }
}
