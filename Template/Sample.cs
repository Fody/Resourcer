
// ReSharper disable UnusedMember.Local
using System.IO;
// ReSharper disable UnusedVariable

namespace Before
{
    class Sample
    {
        void ReadResourceAsString()
        {
            var stringValue = Resource.AsString("ResourceName");
        }

        void ReadResourceAsStream()
        {
            var streamValue = Resource.AsStream("ResourceName");
        }
    }
}

namespace After
{
    class Sample
    {
        void ReadResourceAsString()
        {
            var assembly = typeof(Sample).Assembly;
            using (var stream = assembly.GetManifestResourceStream("AssemblyName.ResourceName"))
            using (var streamReader = new StreamReader(stream))
            {
                // ReSharper disable once RedundantAssignment
                streamReader.ReadToEnd();
            }
        }

        void ReadResourceAsStream()
        {
            var assembly = typeof(Sample).Assembly;
            var streamValue = assembly.GetManifestResourceStream("AssemblyName.ResourceName");
        }
    }
}