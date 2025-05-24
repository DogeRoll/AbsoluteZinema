
namespace AbsoluteZinema.GraphicalFixes
{
    internal interface IGraphicalFix
    {
        bool ShouldBeApplied();

        void Apply();

        void Remove();
    }
}
