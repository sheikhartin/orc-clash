using Godot;

namespace OrcClash.Characters.Enemies.Wraiths.States;

public partial class DeathState : BaseState<BaseWraithBehavior> {
    public DeathState(BaseWraithBehavior character) : base(character) { }

    public override void Enter() => base.Character.SetDirectionalAnimation("death");

    public override void PhysicsProcess(double delta) {
        base.Character.Velocity = new Vector2(
            0, base.Character.Velocity.Y + base.Character.GravityForce * (float)delta
        );
        base.Character.MoveAndSlide();
    }
}
