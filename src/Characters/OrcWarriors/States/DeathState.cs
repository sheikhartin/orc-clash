using Godot;

namespace OrcClash.Characters.OrcWarriors.States;

public partial class DeathState : BaseState<BaseOrcBehavior> {
    public DeathState(BaseOrcBehavior character) : base(character) { }

    public override void Enter() => base.Character.SetDirectionalAnimation("death");

    public override void PhysicsProcess(double delta) {
        base.Character.Velocity = new Vector2(
            0, base.Character.Velocity.Y + base.Character.GravityForce * (float)delta
        );
        base.Character.MoveAndSlide();
    }
}
