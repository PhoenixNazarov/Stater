namespace Stater.Domain.Models;

public record StateMachine(
    Guid Guid,
    string Name,
    List<State> States,
    List<Transition> Transitions,
    List<Variable> Variables
)
{
    public StateMachine() : this(
        Guid.NewGuid(),
        "",
        new List<State>(),
        new List<Transition>(),
        new List<Variable>()
    )
    {
    }

    public State? StartState
    {
        get { return States.Find(x => x.Type == StateType.Start); }
    }

    public List<StateTransition> StateTransitions
    {
        get
        {
            return Transitions
                .Select(y =>
                    {
                        var startState = States.Find(s => s.Guid == y.Start);
                        var endState = States.Find(s => s.Guid == y.End);
                        if (startState != null && endState != null)
                            return new StateTransition(
                                Transition: y,
                                StartState: startState,
                                EndState: endState
                            );
                        return null;
                    }
                )
                .Where(item => item != null)
                .Cast<StateTransition>()
                .ToList();
        }
    }
}