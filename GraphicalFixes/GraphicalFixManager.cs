using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AbsoluteZinema.GraphicalFixes
{
    internal static class GraphicalFixManager
    {
        private static List<IGraphicalFix> _fixes = new();

        public static void ApplyAllFixes()
        {
            var fixClasses = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => typeof(IGraphicalFix).IsAssignableFrom(t) && !t.IsAbstract);

            foreach (var cls in fixClasses)
            {
                var instance = (IGraphicalFix)Activator.CreateInstance(cls);
                instance.Apply();
                _fixes.Add(instance);
            }
        }
    }
}
