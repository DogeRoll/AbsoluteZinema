using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AbsoluteZinema.GraphicalFixes
{
    /// <summary>
    /// Loader class for all graphical fixes<br/>
    /// Just declare a class inherited from IGraphicalFix interface and loader will automatically find and apply them<br/>
    /// </summary>
    internal static class GraphicalFixManager
    {
        private static List<IGraphicalFix> _fixes = new();

        /// <summary>
        /// Find and apply all fixes, inherited from IGraphicalFix interface<br/>
        /// </summary>
        public static void ApplyAllFixes()
        {
            var fixClasses = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => typeof(IGraphicalFix).IsAssignableFrom(t) && !t.IsAbstract);

            foreach (var cls in fixClasses)
            {
                var instance = (IGraphicalFix)Activator.CreateInstance(cls);
                if (instance.ShouldBeApplied())
                {
                    instance.Apply();
                    _fixes.Add(instance);
                }
            }
        }
    }
}
