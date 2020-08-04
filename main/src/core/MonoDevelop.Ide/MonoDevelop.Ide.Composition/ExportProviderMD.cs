
// tommih 2020.
// a simple "IoC container" to get exports from assembies.

using System;
using System.Collections.Generic;
using System.Composition;
using System.Composition.Hosting;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace MonoDevelop.Ide.Composition
{
	public class ExportProviderMD
	{
		private static System.Composition.Hosting.CompositionHost container = null;

		public static void Init( System.Reflection.Assembly[] assemblies )
		{
			if ( container != null ) throw new InvalidOperationException( "already initialized!" );

			var configuration = new ContainerConfiguration().WithAssemblies( assemblies );
			container = configuration.CreateContainer();
		}

		public static T GetExport<T>()
		{
			if ( container == null ) throw new InvalidOperationException( "not initialized!" );

			T result = container.GetExport<T>();
if ( result == null) Console.WriteLine( "oeDEBUG ExportProviderMD.GetExport :: ERROR NOT FOUND : " + typeof(T).FullName );
			return result;
		}

		public static IEnumerable<T> GetExports<T>()
		{
			if ( container == null ) throw new InvalidOperationException( "not initialized!" );

			IEnumerable<T> result = container.GetExports<T>();
if ( result == null || result.Count() < 1 ) Console.WriteLine( "oeDEBUG ExportProviderMD.GetExports :: ERROR NOT FOUND : " + typeof(T).FullName );
			return result;
		}
	}
}

