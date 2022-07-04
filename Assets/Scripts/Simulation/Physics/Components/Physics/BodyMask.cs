namespace Simulation.Physics.Components.Physics
{
    public struct BodyMask
    {
        public int ShapeTypeId;
        public int TagTypeId;

        public BodyMask(int shapeTypeId, int tagTypeId)
        {
            ShapeTypeId = shapeTypeId;
            TagTypeId = tagTypeId;
        }
    }
}