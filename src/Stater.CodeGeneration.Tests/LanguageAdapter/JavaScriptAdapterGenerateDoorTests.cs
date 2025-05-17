using Stater.CodeGeneration.Entity;
using Stater.CodeGeneration.LanguageAdapter.Base;
using Stater.CodeGeneration.LanguageAdapter.Java;
using Stater.CodeGeneration.LanguageAdapter.JavaScript;
using Stater.CodeGeneration.LanguageAdapter.Kotlin;

namespace Stater.CodeGeneration.Tests.LanguageAdapter;

public class JavaScriptAdapterGenerateDoorTests : BaseTestAdapterGenerateDoorTests
{
    protected override BaseLanguageAdapter Adapter => new JavaScriptAdapter();
    protected override Language Language => Language.JavaScript;
}

public class JavaScriptAdapterGenerateTestsDoorTests : BaseTestAdapterGenerateTestsDoorTests
{
    protected override BaseLanguageAdapter Adapter => new JavaScriptAdapter();
    protected override Language Language => Language.JavaScript;
}