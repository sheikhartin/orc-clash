using Godot;
using System.Collections.Generic;

namespace OrcClash.Core;

public interface IState {
    void Enter();
    void Exit();
    void Process(double delta);
    void PhysicsProcess(double delta);
    void HandleInput(InputEvent @event);
}

public partial class StateMachine : Node {
    [Export]
    public string InitialState = "Idle";

    public IState CurrentState;
    public readonly Dictionary<string, IState> States = new();

    public void AddState(string name, IState state) => States[name] = state;

    public void ChangeState(string newState) {
        if (string.IsNullOrEmpty(newState)) {
            GD.PushWarning("State name cannot be null or empty.");
            return;
        }

        if (States.TryGetValue(newState, out IState nextState)) {
            CurrentState?.Exit();
            CurrentState = nextState;
            CurrentState.Enter();
        } else {
            GD.PushError($"State '{newState}' not found!");
        }
    }

    public void EnterInitialState() {
        if (!string.IsNullOrEmpty(InitialState)) {
            ChangeState(InitialState);
        } else {
            GD.PushWarning("Initial state is not set in the state machine.");
        }
    }

    // public override void _Ready() => EnterInitialState();

    public override void _Process(double delta) => CurrentState?.Process(delta);

    public override void _PhysicsProcess(double delta) => CurrentState?.PhysicsProcess(delta);

    public override void _Input(InputEvent @event) => CurrentState?.HandleInput(@event);
}
