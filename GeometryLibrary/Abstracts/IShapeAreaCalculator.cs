namespace GeometryLibrary.Abstracts
{
    public interface IShapeAreaCalculator<T> where T : IAreaCalculable
    {
        Task<double> CalculateAreaAsync(T shape);
    }
}
