using Stater.CodeGeneration.Entity;
using Stater.CodeGeneration.LanguageAdapter.Base;
using Stater.CodeGeneration.LanguageAdapter.Java;
using Stater.CodeGeneration.LanguageAdapter.Kotlin;

namespace Stater.CodeGeneration.Tests.LanguageAdapter;

public class JavaAdapterGenerateDoorTests : BaseTestAdapterGenerateDoorTests
{
    protected override BaseLanguageAdapter Adapter => new JavaAdapter();
    protected override Language Language => Language.Java;
}

public class JavaTestsAdapterGenerateDoorTests : BaseTestAdapterGenerateTestsDoorTests
{
    protected override BaseLanguageAdapter Adapter => new JavaAdapter();
    protected override Language Language => Language.Java;
}