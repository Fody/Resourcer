[![Chat on Gitter](https://img.shields.io/gitter/room/fody/fody.svg?style=flat&max-age=86400)](https://gitter.im/Fody/Fody)
[![NuGet Status](http://img.shields.io/nuget/v/Resourcer.Fody.svg?style=flat&max-age=86400)](https://www.nuget.org/packages/Resourcer.Fody/)


## This is an add-in for [Fody](https://github.com/Fody/Home/)

![Icon](https://raw.githubusercontent.com/Fody/Resourcer/master/package_icon.png)

Simplifies reading embedded resources from an Assembly.

Static resource names are checked at compile time. Use `Resource.AsString` and `Resource.AsStream`.

Runtime resource names are not check but can still make use of the helper code. Use `Resource.AsStringUnChecked` and `Resource.AsStreamUnChecked`. 


## Usage

See also [Fody usage](https://github.com/Fody/Home/blob/master/pages/usage.md).


### NuGet installation

Install the [Resourcer.Fody NuGet package](https://nuget.org/packages/Resourcer.Fody/) and update the [Fody NuGet package](https://nuget.org/packages/Fody/):

```powershell
PM> Install-Package Fody
PM> Install-Package Resourcer.Fody
```

The `Install-Package Fody` is required since NuGet always defaults to the oldest, and most buggy, version of any dependency.


### Add to FodyWeavers.xml

Add `<Resourcer/>` to [FodyWeavers.xml](https://github.com/Fody/Home/blob/master/pages/usage.md#add-fodyweaversxml)

```xml
<?xml version="1.0" encoding="utf-8" ?>
<Weavers>
  <Resourcer/>
</Weavers>
```


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
