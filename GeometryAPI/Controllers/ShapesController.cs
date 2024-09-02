using GeometryAPI.DTOs;
using GeometryLibrary.Abstracts;
using GeometryLibrary.Concretes;
using Microsoft.AspNetCore.Mvc;

namespace GeometryAPI.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/shapes")]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    public class ShapesController : Controller
    {
        private readonly IShapeAreaCalculator<Circle> _circleCalculator;
        private readonly IShapeAreaCalculator<Triangle> _triangleCalculator;

        public ShapesController(IShapeAreaCalculator<Circle> circleCalculator, IShapeAreaCalculator<Triangle> triangleCalculator)
        {
            _circleCalculator = circleCalculator;
            _triangleCalculator = triangleCalculator;
        }

        // GET endpoint - Circle Area (v1.0)
        [HttpGet("circle/area")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetCircleArea(double radius)
        {
            var circle = new Circle(radius);
            var area = await _circleCalculator.CalculateAreaAsync(circle);
            return Ok(area);
        }

        // GET endpoint - Triangle Area (v1.0)
        [HttpGet("triangle/area")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetTriangleArea(TriangleSidesDTO sides)
        {
            var triangle = new Triangle(sides.SideA,sides.SideB,sides.SideC);
            var area = await _triangleCalculator.CalculateAreaAsync(triangle);
            return Ok(area);
        }

        // POST endpoint - Calculate Circle Area (v2.0)
        [HttpPost("circle/area")]
        [MapToApiVersion("2.0")]
        public async Task<IActionResult> CalculateCircleArea([FromBody] Circle circle)
        {
            var area = await _circleCalculator.CalculateAreaAsync(circle);
            return Ok(new { shape = circle.Name, area });
        }

        // POST endpoint - Calculate Triangle Area (v2.0)
        [HttpPost("triangle/area")]
        [MapToApiVersion("2.0")]
        public async Task<IActionResult> CalculateTriangleArea([FromBody] Triangle triangle)
        {
            var area = await _triangleCalculator.CalculateAreaAsync(triangle);
            return Ok(new { shape = triangle.Name, area, isRightAngled = triangle.IsRightAngled() });
        }
    }
}
