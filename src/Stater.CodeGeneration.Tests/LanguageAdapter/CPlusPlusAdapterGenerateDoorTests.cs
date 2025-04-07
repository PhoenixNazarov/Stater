using Stater.CodeGeneration.Entity;
using Stater.CodeGeneration.LanguageAdapter.Base;
using Stater.CodeGeneration.LanguageAdapter.CPlusPlus;
using Stater.CodeGeneration.LanguageAdapter.CSharp;
using Stater.CodeGeneration.LanguageAdapter.Java;
using Stater.CodeGeneration.LanguageAdapter.Kotlin;

namespace Stater.CodeGeneration.Tests.LanguageAdapter;

public class CPlusPlusAdapterGenerateDoorTests : BaseTestAdapterGenerateDoorTests
{
    protected override BaseLanguageAdapter Adapter => new CPlusPlusAdapter();
    protected override Language Language => Language.CPlusPlus;
}

public class CPlusPlusAdapterGenerateTestsDoorTests : BaseTestAdapterGenerateTestsDoorTests
{
    protected override BaseLanguageAdapter Adapter => new CPlusPlusAdapter();
    protected override Language Language => Language.CPlusPlus;
}