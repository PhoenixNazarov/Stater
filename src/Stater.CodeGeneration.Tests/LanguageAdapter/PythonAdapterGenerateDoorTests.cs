using Stater.CodeGeneration.Entity;
using Stater.CodeGeneration.LanguageAdapter.Base;
using Stater.CodeGeneration.LanguageAdapter.Python;

namespace Stater.CodeGeneration.Tests.LanguageAdapter;

public class PythonAdapterGenerateDoorTests : BaseTestAdapterGenerateDoorTests
{
    protected override BaseLanguageAdapter Adapter => new PythonAdapter();
    protected override Language Language => Language.Python3;
}

public class PythonAdapterGenerateTestsDoorTests : BaseTestAdapterGenerateTestsDoorTests
{
    protected override BaseLanguageAdapter Adapter => new PythonAdapter();
    protected override Language Language => Language.Python3;
}