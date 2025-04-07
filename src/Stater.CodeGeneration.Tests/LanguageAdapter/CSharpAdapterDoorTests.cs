using Stater.CodeGeneration.Entity;
using Stater.CodeGeneration.LanguageAdapter.Base;
using Stater.CodeGeneration.LanguageAdapter.CSharp;
using Stater.CodeGeneration.LanguageAdapter.Java;
using Stater.CodeGeneration.LanguageAdapter.Kotlin;

namespace Stater.CodeGeneration.Tests.LanguageAdapter;

public class CShaprAdapterGenerateDoorTests : BaseTestAdapterGenerateDoorTests
{
    protected override BaseLanguageAdapter Adapter => new CSharpAdapter();
    protected override Language Language => Language.CSharp;
}

public class CShaprAdapterGenerateTestsDoorTests : BaseTestAdapterGenerateTestsDoorTests
{
    protected override BaseLanguageAdapter Adapter => new CSharpAdapter();
    protected override Language Language => Language.CSharp;
}