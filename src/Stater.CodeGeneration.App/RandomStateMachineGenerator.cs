using Stater.Domain.Models;

namespace Stater.CodeGeneration.App;

public class RandomStateMachineGenerator(int seed)
{
    private readonly Random random = new(seed);

    private string RandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    private State GenerateState() => new(
        Guid.NewGuid(),
        RandomString(10),
        RandomString(20),
        StateType.Common,
        0,
        0,
        new List<Event>(),
        new List<Event>()
    );

    private Transition GenerateTransition(IReadOnlyList<State> states) => new(
        Guid.NewGuid(),
        RandomString(15),
        states[random.Next(states.Count)].Guid,
        states[random.Next(states.Count)].Guid,
        null,
        null
    );

    private VariableValue.StringVariable GenerateStringVariableValue() => new(RandomString(30));
    private VariableValue.IntVariable GenerateIntVariableValue() => new(random.Next(99999));
    private VariableValue.FloatVariable GenerateFloatVariableValue() => new((float)random.NextDouble());
    private VariableValue.BoolVariable GenerateBoolVariableValue() => new(random.Next(2) == 0);

    private VariableValue GenerateVariableValue() => random.Next(4) switch
    {
        0 => GenerateStringVariableValue(),
        1 => GenerateIntVariableValue(),
        2 => GenerateFloatVariableValue(),
        3 => GenerateBoolVariableValue(),
        _ => throw new ArgumentOutOfRangeException()
    };

    private VariableValue GenerateVariableValue(VariableValue variableValue) => variableValue switch
    {
        VariableValue.StringVariable => GenerateStringVariableValue(),
        VariableValue.IntVariable => GenerateIntVariableValue(),
        VariableValue.FloatVariable => GenerateFloatVariableValue(),
        VariableValue.BoolVariable => GenerateBoolVariableValue(),
        _ => throw new ArgumentOutOfRangeException()
    };

    private Variable GenerateVariable() => new(Guid.NewGuid(), RandomString(15), GenerateVariableValue());

    private Event GenerateEvent(IReadOnlyList<Variable> variables)
    {
        switch (random.Next(2))
        {
            case 0:
                var variable = variables[random.Next(variables.Count)];
                return new Event.VariableSet(
                    variable.Guid,
                    GenerateVariableValue(variable.StartValue)
                );
            case 1:
                var variable2 = variables[random.Next(variables.Count)];
                return new Event.VariableMath(
                    variable2.Guid,
                    random.Next(4) switch
                    {
                        0 => Event.VariableMath.MathTypeEnum.Sum,
                        1 => Event.VariableMath.MathTypeEnum.Div,
                        2 => Event.VariableMath.MathTypeEnum.Sub,
                        3 => Event.VariableMath.MathTypeEnum.Mul,
                        _ => throw new ArgumentOutOfRangeException()
                    },
                    GenerateVariableValue(variable2.StartValue)
                );
        }

        throw new ArgumentOutOfRangeException();
    }

    private Condition GenerateCondition(IReadOnlyList<Variable> variables)
    {
        switch (random.Next(1))
        {
            case 0:
                var variable1 = variables[random.Next(variables.Count)];
                return new Condition.VariableCondition(
                    variable1.Guid,
                    random.Next(6) switch
                    {
                        0 => Condition.VariableCondition.ConditionTypeEnum.Lt,
                        1 => Condition.VariableCondition.ConditionTypeEnum.Le,
                        2 => Condition.VariableCondition.ConditionTypeEnum.Eq,
                        3 => Condition.VariableCondition.ConditionTypeEnum.Ne,
                        4 => Condition.VariableCondition.ConditionTypeEnum.Gt,
                        5 => Condition.VariableCondition.ConditionTypeEnum.Ge,
                        _ => throw new ArgumentOutOfRangeException()
                    },
                    GenerateVariableValue(variable1.StartValue)
                );
        }

        throw new ArgumentOutOfRangeException();
    }

    public StateMachine GenerateStateMachine(
        int countState,
        int countTransitions,
        int countVariables,
        int countEvents,
        int countConditions
    )
    {
        var states = Enumerable.Range(0, countState).Select(_ => GenerateState()).ToList();
        var startStateIndex = random.Next(countState);
        states[startStateIndex] = states[startStateIndex] with { Type = StateType.Start };
        var transitions = Enumerable.Range(0, countTransitions).Select(_ => GenerateTransition(states)).ToList();
        var variables = Enumerable.Range(0, countVariables).Select(_ => GenerateVariable()).ToList();

        for (var i = 0; i < countEvents; i++)
        {
            var transitionIndex = random.Next(countTransitions);
            transitions[transitionIndex] = transitions[transitionIndex] with { Event = GenerateEvent(variables) };
        }

        for (var i = 0; i < countConditions; i++)
        {
            var transitionIndex = random.Next(countTransitions);
            transitions[transitionIndex] =
                transitions[transitionIndex] with { Condition = GenerateCondition(variables) };
        }
        
        return new StateMachine(
            Guid.NewGuid(),
            RandomString(20),
            states,
            transitions,
            variables
        );
    }
}