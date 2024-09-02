using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeometryLibrary.Abstracts;
using Serilog;

namespace GeometryLibrary.Concretes
{
    public class Triangle : Shape, IAreaCalculable    
    {
        public override string Name => "Triangle";
        public double SideA { get; }
        public double SideB { get; }
        public double SideC { get; }

        public Triangle(double sideA, double sideB, double sideC)
        {
            if (sideA <= 0 || sideB <= 0 || sideC <= 0)
            {
                Log.Error("Sides entered not correctly. Sides must be greater than zero.");
                throw new ArgumentException("Sides must be greater than zero.");
            }
            if (sideA + sideB <= sideC || sideA + sideC <= sideB || sideB + sideC <= sideA)
            {
                Log.Error("Sides entered not correctly. Invalid triangle sides.");
                throw new ArgumentException("Invalid triangle sides.");
            }

            SideA = sideA;
            SideB = sideB;
            SideC = sideC;
        }
        public double CalculateArea()
        {

            try
            {            
                double s = (SideA + SideB + SideC) / 2;
                var area = Math.Sqrt(s * (s - SideA) * (s - SideB) * (s - SideC));
                Log.Information("Calculated area of triangle with sides {SideA}, {SideB}, {SideC}: {Area}", SideA, SideB, SideC, area);
                return area;
            }
            catch (Exception ex)
            {
                Log.Error(ex,"Something went wrong!");
                throw new InvalidOperationException("Something went wrong!" + ex.Message);
            }
        }

        public bool IsRightAngled()
        {
            try
            {
                var sides = new[] { SideA, SideB, SideC };
                Array.Sort(sides);
                var isRightAngled = Math.Abs(sides[0] * sides[0] + sides[1] * sides[1] - sides[2] * sides[2]) < 1e-10;
                Log.Information("Triangle with sides {SideA}, {SideB}, {SideC} is {Result}", SideA, SideB, SideC, isRightAngled ? "right-angled" : "not right-angled");
                return isRightAngled;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Something went wrong!");
                throw new InvalidOperationException("Something went wrong!" + ex.Message);
            }
        }
    }
}
