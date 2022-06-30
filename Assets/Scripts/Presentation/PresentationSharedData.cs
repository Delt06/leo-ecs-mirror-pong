using Presentation.Interpolation;

namespace Presentation
{
    public class PresentationSharedData
    {
        public readonly InterpolationSettings InterpolationSettings;

        public PresentationSharedData(InterpolationSettings interpolationSettings) =>
            InterpolationSettings = interpolationSettings;
    }
}