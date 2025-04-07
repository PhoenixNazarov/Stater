using Stater.CodeGeneration.Entity;
using Stater.CodeGeneration.LanguageAdapter.Base;
using Stater.CodeGeneration.LanguageAdapter.TypeScript;

namespace Stater.CodeGeneration.Tests.LanguageAdapter;

public class TypeScriptAdapterGenerateDoorTests : BaseTestAdapterGenerateDoorTests
{
    protected override BaseLanguageAdapter Adapter => new TypeScriptAdapter();
    protected override Language Language => Language.TypeScript;
}

public class TypeScriptAdapterGenerateTestsDoorTests : BaseTestAdapterGenerateTestsDoorTests
{
    protected override BaseLanguageAdapter Adapter => new TypeScriptAdapter();
    protected override Language Language => Language.TypeScript;
}