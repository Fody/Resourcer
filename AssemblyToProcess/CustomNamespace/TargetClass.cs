using System.IO;
using Resourcer;

namespace AssemblyToProcess.CustomNamespace
{
	public class TargetClass
	{
		public Stream WithAsStream()
		{
			return Resource.AsStream("EmbeddedResource.txt");
		}
		public string WithAsString()
		{
			return Resource.AsString("EmbeddedResource.txt");
		}
	}
}