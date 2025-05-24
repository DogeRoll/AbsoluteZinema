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
        /// Checks if requested fix is applied
        /// </summary>
        /// <param name="type"></param>
        /// <returns>'true' if fix is applied otherwise 'false'</returns>
        public static bool IsFixApplied(Type type) => _fixes.Any(obj => obj.GetType() == typeof(Type));

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

        public static void Clear()
        {
            foreach  (var fix in _fixes)
            {
                fix.Remove();
            }
            _fixes.Clear();
        }
    }
}
