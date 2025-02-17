using System.Collections.Generic;

using Godot;

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

    public void AddState(string name, IState state) => this.States[name] = state;

    public void ChangeState(string newState) {
        if (string.IsNullOrEmpty(newState)) {
            GD.PushWarning("State name cannot be null or empty.");
            return;
        }

        if (this.States.TryGetValue(newState, out IState nextState)) {
            this.CurrentState?.Exit();
            this.CurrentState = nextState;
            this.CurrentState.Enter();
        } else {
            GD.PushError($"State '{newState}' not found!");
        }
    }

    public void EnterInitialState() {
        if (!string.IsNullOrEmpty(this.InitialState)) {
            ChangeState(this.InitialState);
        } else {
            GD.PushWarning("Initial state is not set in the state machine.");
        }
    }

    // public override void _Ready() => EnterInitialState();

    // public override void _ExitTree() {
    //     this.CurrentState?.Exit();
    //     this.CurrentState = null;
    // }

    public override void _Process(double delta) => this.CurrentState?.Process(delta);

    public override void _PhysicsProcess(double delta) => this.CurrentState?.PhysicsProcess(delta);

    public override void _Input(InputEvent @event) => this.CurrentState?.HandleInput(@event);
}
