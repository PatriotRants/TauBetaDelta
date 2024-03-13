
using System.Data;
using System.Reflection;

namespace ForgeWorks.GlowFork.Automata;

public sealed class StateMachine
{
    private static readonly Type[] exportedTypes = Assembly
        .GetExecutingAssembly()
        .GetExportedTypes()
        .Where(t => !t.IsAbstract)
        .Where(t => typeof(State).IsAssignableFrom(t))
        .ToArray();
    private static readonly BindingFlags ctorBinding = BindingFlags.Instance | BindingFlags.Public;

    private readonly Type[] ctorParamTypes;
    private readonly object[] ctorParams;
    private readonly Dictionary<string, Type> stateLookup = new();
    private readonly Dictionary<Type, Func<State>> states = new();

    public event Action<State> StateChanged;

    public State Current { get; private set; }

    public StateMachine(params object[] stateParameters)
    {
        //  transform to array of types
        ctorParamTypes = stateParameters
            .Select(o => o.GetType())
            .ToArray();
        //  copy the array of objects
        ctorParams = stateParameters
            .ToArray();
    }
    public StateMachine(State startState, params object[] stateParameters) : this(stateParameters)
    {
        Current = startState;
    }
    public StateMachine WithStates(params string[] stateNames)
    {
        //  this is likely going to need some tweaking when moved to GlowFork lib
        foreach (var state in stateNames)
        {
            if (exportedTypes.TryGet(state, out Type stateType))
            {
                var ctor = stateType.GetConstructor(ctorBinding, ctorParamTypes.ToArray());
                if (ctor != null)
                {
                    //  add valid state to lookup
                    stateLookup.Add(state, stateType);
                    //  add valid state type to func lookup
                    states.Add(stateType, () => ctor.Invoke(ctorParams.ToArray()) as State);
                }
            }
        }

        return this;
    }

    public TState GetCurrent<TState>() where TState : State
    {
        return (TState)Current;
    }

    internal void ChangeState<TState>() where TState : State
    {
        ChangeState(typeof(TState));
    }
    internal void ChangeState(string nextState)
    {
        if (stateLookup.TryGetValue(nextState, out Type stateType))
        {
            ChangeState(stateType);
        }
    }

    private void ChangeState(Type stateType)
    {
        // transition to TState
        if (states.TryGetValue(stateType, out Func<State> getState))
        {
            var next = getState();

            if (next != Current)
            {
                var oldState = Current;

                Current = next;
                StateChanged?.Invoke(oldState);
            }
        }
    }
}
