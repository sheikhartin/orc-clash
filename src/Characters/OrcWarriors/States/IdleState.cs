using Godot;

namespace OrcClash.Characters.OrcWarriors.States;

public partial class IdleState : BaseState<BaseOrcBehavior> {
    public IdleState(BaseOrcBehavior character) : base(character) { }

    public override void Enter() {
        base.Character.Velocity = new Vector2(0, base.Character.Velocity.Y);
        base.Character.SetDirectionalAnimation("idle");
    }

    public override void PhysicsProcess(double delta) {
        base.Character.Velocity = new Vector2(
            base.Character.Velocity.X,
            base.Character.Velocity.Y + base.Character.GravityForce * (float)delta
        );
        base.Character.MoveAndSlide();

        base.Character.AdjustDirection();
    }

    public override void HandleInput(InputEvent @event) {
        if (@event.IsActionPressed("jump") && base.Character.IsOnFloor()) {
            base.ChangeState("Jump");
        } else if (
            @event.IsActionPressed("fast_move_left")
            || @event.IsActionPressed("fast_move_right")
        ) {
            base.ChangeState("Run");
        } else if (
            @event.IsActionPressed("move_left")
            || @event.IsActionPressed("move_right")
        ) {
            base.ChangeState("Walk");
        } else if (@event.IsActionPressed("attack")) {
            base.ChangeState("Attack");
        }
    }
}
