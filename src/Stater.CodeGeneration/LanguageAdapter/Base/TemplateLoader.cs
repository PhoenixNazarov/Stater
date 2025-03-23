using System.Reflection;

namespace Stater.CodeGeneration.LanguageAdapter.Base;

public static class TemplateLoader
{
    public static string LoadTemplate(string templateName)
    {
        var resourceName = $"Stater.CodeGeneration.Templates.{templateName}.scriban";

        using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);
        if (stream == null)
            throw new FileNotFoundException($"Template {templateName} not found");

        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
}