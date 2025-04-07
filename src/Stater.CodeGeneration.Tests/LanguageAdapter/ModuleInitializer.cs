using System.Runtime.CompilerServices;

namespace Stater.CodeGeneration.Tests.LanguageAdapter;

public class ModuleInitializer
{
    [ModuleInitializer]
    public static void Init()
    {
        VerifierSettings.AutoVerify();
    }
}