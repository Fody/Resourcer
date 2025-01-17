# <img src="/package_icon.png" height="30px"> Resourcer.Fody

[![NuGet Status](https://img.shields.io/nuget/v/Resourcer.Fody.svg)](https://www.nuget.org/packages/Resourcer.Fody/)

Simplifies reading embedded resources from an Assembly.

**See [Milestones](../../milestones?state=closed) for release notes.**

Static resource names are checked at compile time. Use `Resource.AsString` and `Resource.AsStream`.

Runtime resource names are not check but can still make use of the helper code. Use `Resource.AsStringUnChecked` and `Resource.AsStreamUnChecked`.


### This is an add-in for [Fody](https://github.com/Fody/Home/)

**It is expected that all developers using Fody [become a Patron on OpenCollective](https://opencollective.com/fody/contribute/patron-3059). [See Licensing/Patron FAQ](https://github.com/Fody/Home/blob/master/pages/licensing-patron-faq.md) for more information.**


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

[Box](https://thenounproject.com/noun/box/#icon-No11029) designed by [Mourad Mokrane](https://thenounproject.com/molumen) from [The Noun Project](https://thenounproject.com).