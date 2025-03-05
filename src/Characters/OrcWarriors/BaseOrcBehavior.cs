using System;

using Godot;

using OrcClash.Effects;

namespace OrcClash.Characters.OrcWarriors;

public partial class BaseOrcBehavior : BaseCharacter {
    public CameraShake Camera;

    public override void _Ready() {
        base._Ready();

        this.Camera = (
            base.GetNode<Camera2D>("Camera") as CameraShake
            ?? throw new NullReferenceException("'Camera' node is missing in the scene.")
        );
    }

    protected override void InitializeStates() {
        base.StateMachine.AddState("Idle", new States.IdleState(this));
        base.StateMachine.AddState("Walk", new States.WalkState(this));
        base.StateMachine.AddState("Jump", new States.JumpState(this));
        base.StateMachine.AddState("Attack", new States.AttackState(this));
        base.StateMachine.AddState("Hurt", new States.HurtState(this));
        base.StateMachine.AddState("Death", new States.DeathState(this));
    }
}
