
using System.Data;
using System.Reflection;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("TauBetaDelta")]

namespace ForgeWorks.GlowFork.Automata;

/// <summary>
/// <para>Core GameEngine Component</para>
/// 
/// Use the <see cref="StateMachine"/> to map and manage application states and transitional configurations.
/// </summary>
public sealed class StateMachine
{
    /* **
        Discover the entry assembly's exported types derived from the `State` base class
    ** */
    private static readonly Type[] exportedTypes = Assembly
        .GetEntryAssembly()
        .GetExportedTypes()
        .Where(t => !t.IsAbstract)
        .Where(t => typeof(State).IsAssignableFrom(t))
        .ToArray();
    //  default binding flags for reflection
    private static readonly BindingFlags ctorBinding = BindingFlags.Instance | BindingFlags.Public;

    /* **
        Bot ctorParamTypes & ctorParams make the assumption that all State-derived classes will use these constructor
        parameters.
    ** */
    private readonly Type[] ctorParamTypes;
    private readonly object[] ctorParams;
    /* **
        Dictionaries map exported, State-derived types and constructor functions.
    ** */
    private readonly Dictionary<string, Type> stateLookup = new();
    private readonly Dictionary<Type, Func<State>> states = new();

    /// <summary>
    /// Event when the <see cref="Current"/> state has changed
    /// </summary>
    public event Action<State> StateChanged;
    /// <summary>
    /// Get the <see cref="Current"/> <see cref="State"/>
    /// </summary>
    public State Current { get; private set; }

    /// <summary>
    /// Constructor; provide optional set of <see cref="State"/> parameters
    /// </summary>
    /// <param name="stateParameters"></param>
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
    /// <summary>
    /// Constructor; provide starting <see cref="State"/> and optional set of <see cref="State"/> parameters
    /// </summary>
    public StateMachine(State startState, params object[] stateParameters) : this(stateParameters)
    {
        Current = startState;
    }
    /// <summary>
    /// Configure all viable state names. Omitted state names will not transition.
    /// </summary>
    /// <returns>The current <see cref="StateMachine"/></returns>
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
    /// <summary>
    /// Get the <see cref="Current"/> as <see cref="TState"/>
    /// </summary>
    /// <typeparam name="TState">where TState : State</typeparam>
    /// <returns><see cref="TState"/></returns>
    public TState GetCurrent<TState>() where TState : State
    {
        return (TState)Current;
    }

    // currently change states from the Entry Assembly (TauBetaDelta)
    internal void ChangeState<TState>() where TState : State
    {
        ChangeState(typeof(TState));
    }
    internal void ChangeState(string nextState)
    {
        //  look up the state name in state type lookup
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
            //  constructs the state objects with input parameters if supplied
            var next = getState();
            //  don't change if states are the same
            if (next != Current)
            {
                //  cache old state
                var oldState = Current;

                //  set current state and raise state change event
                Current = next;
                StateChanged?.Invoke(oldState);
            }
        }
    }
}
