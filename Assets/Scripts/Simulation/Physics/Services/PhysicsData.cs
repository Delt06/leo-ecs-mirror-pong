namespace Simulation.Physics.Services
{
    public class PhysicsData
    {
        private readonly float _dt;
        private readonly int _physicsIterations;
        public readonly ContactSolverReference[][] ContactSolversMatrix;
        public readonly ContactTesterReference[][] ContactTestersMatrix;

        public PhysicsData(float dt, int physicsIterations, int shapeTypesAmount, int tagsAmount)
        {
            _dt = dt;
            _physicsIterations = physicsIterations;
            ContactTestersMatrix = CreateMatrix<ContactTesterReference>(shapeTypesAmount);
            ContactSolversMatrix = CreateMatrix<ContactSolverReference>(tagsAmount);
        }

        private static T[][] CreateMatrix<T>(int size)
        {
            var matrix = new T[size][];
            for (var i = 0; i < size; ++i)
            {
                matrix[i] = new T[size];
            }

            return matrix;
        }

        public float GetDt() => _dt / _physicsIterations;
    }
}