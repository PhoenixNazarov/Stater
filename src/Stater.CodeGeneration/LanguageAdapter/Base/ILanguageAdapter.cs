using Stater.CodeGeneration.Entity;
using Stater.Domain.Models;

namespace Stater.CodeGeneration.LanguageAdapter.Base;

public interface ILanguageAdapter
{
    string Generate(StateMachine stateMachine, GenerationSettings settings);
}