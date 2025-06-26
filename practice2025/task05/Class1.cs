using System;
using System.Reflection;
using System.Collections.Generic;

public class ClassAnalyzer
{
    private Type _type;

    public ClassAnalyzer(Type type)
    {
        _type = type;
    }

    public IEnumerable<string> GetPublicMethods()
    {
        return _type.GetMethods().Select(s => s.Name);
    }

    public IEnumerable<string> GetMethodParams(string methodname)
    {
        return _type.GetMethod(methodname).GetParameters().Select(parameter => parameter.Name);
    }

    public IEnumerable<string> GetAllFields()
    {
        return _type.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static)
            .Select(field => field.Name);
    }

    public IEnumerable<string> GetProperties()
    {
        return _type.GetProperties().Select(p => p.Name);
    }

    public bool HasAttribute<T>() where T : Attribute
    {
        return _type.GetCustomAttributes(typeof(T), false).Any();
    }
}