using Mmicovic.RPSSL.Service.Models;

namespace Mmicovic.RPSSL.Service
{
    public interface IShapeProvider
    {
        Shape GetShape(int shapeId);
        IEnumerable<Shape> GetAllShapes();

        int GetMaxShapeId();
        bool IsValidShapeId(int shapeId);
    }

    public class ShapeProvider : IShapeProvider
    {
        private readonly int maxShapeId;

        private readonly List<Shape> allShapes = [new(1, "rock"),
                                                  new(2, "paper"),
                                                  new(3, "scissors"),
                                                  new(4, "spock"),
                                                  new(5, "lizard")];

        public ShapeProvider()
        {
            maxShapeId = allShapes.Select(s => s.Id).Max();
        }

        public IEnumerable<Shape> GetAllShapes() => allShapes;
        public Shape GetShape(int shapeId) => allShapes.First(s => s.Id == shapeId);

        public int GetMaxShapeId() => maxShapeId;
        public bool IsValidShapeId(int shapeId) => allShapes.Any(s => s.Id == shapeId);
    }
}
