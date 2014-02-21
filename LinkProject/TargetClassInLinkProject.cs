using Resourcer;

public class TargetClassInLinkProject
{
    public string WithAsString()
    {
        return Resource.AsString("ResourceInLinkProject.txt");
    }
}