using Godot;

namespace OrcClash.Characters.OrcWarriors.States;

public partial class WalkState : BaseState<BaseOrcBehavior> {
    public WalkState(BaseOrcBehavior character) : base(character) { }

    public override void Enter() => base.Character.SetDirectionalAnimation("walk");

    public override void PhysicsProcess(double delta) {
        float horizontalDirection = Input.GetAxis("move_left", "move_right");
        if (horizontalDirection == 0) base.ChangeState("Idle");

        base.Character.Velocity = new Vector2(
            horizontalDirection * base.Character.BaseSpeed,
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
        } else if (@event.IsActionPressed("attack")) {
            base.ChangeState("Attack");
        }
    }
}
