using Stater.CodeGeneration.Entity;
using Stater.CodeGeneration.LanguageAdapter.Base;
using Stater.CodeGeneration.LanguageAdapter.Kotlin;

namespace Stater.CodeGeneration.Tests.LanguageAdapter;

public class KotlinAdapterGenerateDoorTests : BaseTestAdapterGenerateDoorTests
{
    protected override BaseLanguageAdapter Adapter => new KotlinAdapter();
    protected override Language Language => Language.Kotlin;
}

public class KotlinAdapterGenerateTestsDoorTests : BaseTestAdapterGenerateTestsDoorTests
{
    protected override BaseLanguageAdapter Adapter => new KotlinAdapter();
    protected override Language Language => Language.Kotlin;
}