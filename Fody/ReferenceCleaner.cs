using System.Linq;

public partial class ModuleWeaver
{

    public void CleanReferences()
    {
        var referenceToRemove = ModuleDefinition.AssemblyReferences.FirstOrDefault(x => x.Name == "Resourcer");
        if (referenceToRemove == null)
        {
            LogInfo("\tNo reference to 'Resourcer' found. References not modified.");
            return;
        }

        ModuleDefinition.AssemblyReferences.Remove(referenceToRemove);
        LogInfo("\tRemoving reference to 'Resourcer'.");
    }
}