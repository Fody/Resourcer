
// ReSharper disable UnusedMember.Local
using System.IO;
using System.Reflection;
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
            string stringValue;
            var assembly = typeof(Sample).GetTypeInfo().Assembly;
            using (var stream = assembly.GetManifestResourceStream("AssemblyName.ResourceName"))
            using (var streamReader = new StreamReader(stream))
            {
                // ReSharper disable once RedundantAssignment
                stringValue = streamReader.ReadToEnd();
            }
        }

        void ReadResourceAsStream()
        {
            var assembly = typeof(Sample).GetTypeInfo().Assembly;
            var streamValue = assembly.GetManifestResourceStream("AssemblyName.ResourceName");
        }
    }
}


// ReSharper restore UnusedMember.Local