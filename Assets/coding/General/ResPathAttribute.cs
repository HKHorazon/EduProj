
using System;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class ResPathAttribute : Attribute
{
    public string ResourcePath { get; set; }

    public ResPathAttribute(string resPath) 
    {
        this.ResourcePath = resPath;
    }
}