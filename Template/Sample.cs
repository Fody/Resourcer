
// ReSharper disable UnusedMember.Local
using System;
using System.IO;
using System.Reflection;

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
            var assembly = Assembly.GetExecutingAssembly();
            using (var stream = assembly.GetManifestResourceStream("AssemblyName.ResourceName"))
            using (var streamReader = new StreamReader(stream))
            {
                stringValue = streamReader.ReadToEnd();
            }
        }

        void ReadResourceAsStream()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var streamValue = assembly.GetManifestResourceStream("AssemblyName.ResourceName");
        }
    }
}


// ReSharper restore UnusedMember.Local