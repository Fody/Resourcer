[![Chat on Gitter](https://img.shields.io/gitter/room/fody/fody.svg?style=flat)](https://gitter.im/Fody)
[![NuGet Status](http://img.shields.io/nuget/v/Resourcer.Fody.svg?style=flat)](https://www.nuget.org/packages/Resourcer.Fody/)


## This is an add-in for [Fody](https://github.com/Fody/Fody/) 

![Icon](https://raw.github.com/Fody/Resourcer/master/Icons/package_icon.png)

Simplifies reading embedded resources from an Assembly.

[Introduction to Fody](http://github.com/Fody/Fody/wiki/SampleUsage)

Static resource names are checked at compile time. Use `Resource.AsString` and `Resource.AsStream`.

Runtime resource names are not check but can still make use of the helper code. Use `Resource.AsStringUnChecked` and `Resource.AsStreamUnChecked`. 


## The nuget package

https://nuget.org/packages/Resourcer.Fody/

    PM> Install-Package Resourcer.Fody


## What it does 

Assuming you have an embedded resource at the root of your assembly named `ResourceName` and your assembly is named `AssemblyName`.


### Your Code

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


### What gets compiled

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


## Icon

<a href="http://thenounproject.com/noun/box/#icon-No11029" target="_blank">Box</a> designed by <a href="http://thenounproject.com/molumen" target="_blank">Mourad Mokrane</a> from The Noun Project
