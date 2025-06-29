using Newtonsoft.Json;
using NUnit.Framework.Constraints;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Xml.Linq;

namespace Documentation;

public class Specifier<T> : ISpecifier
{
    private Type type = typeof(T);
    private ILookup<string, MethodInfo> Lookup;
    public Specifier()
    {
        type = typeof(T);
        Lookup = type.GetMethods().ToLookup(method => method.Name);
    }

    public string GetApiDescription() => type.GetCustomAttribute<ApiDescriptionAttribute>()?.Description;

    public string[] GetApiMethodNames() =>
        type.GetMethods(BindingFlags.Instance | BindingFlags.Public)
            .Where(info => info.GetCustomAttribute<ApiMethodAttribute>() != null)
            .Select(attribute => attribute.Name)
            .ToArray();

    public string GetApiMethodDescription(string methodName) =>
        type.GetMethod(methodName)?
            .GetCustomAttribute<ApiDescriptionAttribute>()?
            .Description;

    public string[] GetApiMethodParamNames(string methodName) =>
        type.GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public)
            .GetParameters()
            .Select(parameter => parameter.Name)
            .ToArray(); //?? Array.Empty<string>;

    public string GetApiMethodParamDescription(string methodName, string paramName) =>
        type.GetMethod(methodName)?
            .GetParameters()
            .FirstOrDefault(p => p.Name == paramName)?
            .GetCustomAttribute<ApiDescriptionAttribute>()?
            .Description;

    public ApiParamDescription GetApiMethodParamFullDescription(string methodName, string paramName)
    {
        var param = type.GetMethods(BindingFlags.Public | BindingFlags.Instance)
       .Where(m => m.Name == methodName && m.GetCustomAttribute<ApiMethodAttribute>() != null)
       .SelectMany(m => m.GetParameters())
       .FirstOrDefault(p => p.Name == paramName);

        return param != null
            ? new ApiParamDescription
            {
                ParamDescription = new CommonDescription(
                    param.Name,
                    param.GetCustomAttribute<ApiDescriptionAttribute>()?.Description
                ),
                Required = param.GetCustomAttribute<ApiRequiredAttribute>()?.Required ?? false,
                MinValue = param.GetCustomAttribute<ApiIntValidationAttribute>()?.MinValue,
                MaxValue = param.GetCustomAttribute<ApiIntValidationAttribute>()?.MaxValue
            }
            : new ApiParamDescription
            {
                ParamDescription = new CommonDescription(paramName),
                Required = false,
                MinValue = null,
                MaxValue = null
            };
    }

    public ApiMethodDescription GetApiMethodFullDescription(string methodName)
    {
        return (Lookup[methodName].FirstOrDefault()?.GetCustomAttributes<ApiMethodAttribute>().Any() ?? false) ?
            new ApiMethodDescription
            {
                MethodDescription = new CommonDescription
                {
                    Name = methodName,
                    Description = GetApiMethodDescription(methodName)
                },
                ParamDescriptions = Lookup[methodName].FirstOrDefault()
                    ?.GetParameters()
                    .Select(param => GetApiMethodParamFullDescription(methodName, param.Name))
                    .ToArray(),
                ReturnDescription = ReturnParamFullDescription(methodName)
            }
            : default;
    }

    private ApiParamDescription ReturnParamFullDescription(string methodName)
    {
        var attributesToReturn = Lookup[methodName].FirstOrDefault()?.ReturnParameter?.GetCustomAttributes();

        if (!attributesToReturn.Any()) return null;

        var rulesToReturn = attributesToReturn?.OfType<ApiIntValidationAttribute>().FirstOrDefault();

        return new ApiParamDescription
        {
            ParamDescription = new CommonDescription
            {
                Description = attributesToReturn?
                              .OfType<ApiDescriptionAttribute>()
                              .FirstOrDefault()?
                              .Description
            },
            Required = attributesToReturn?.OfType<ApiRequiredAttribute>().FirstOrDefault()?.Required ?? false,
            MinValue = rulesToReturn?.MinValue,
            MaxValue = rulesToReturn?.MaxValue
        };
    }
}