using Godot;

namespace OrcClash.Characters.OrcWarriors.States;

public partial class JumpState : BaseState<BaseOrcBehavior> {
    private bool _hasDoubleJumped;

    public JumpState(BaseOrcBehavior character) : base(character) { }

    private void PerformJump() {
        base.Character.Velocity = new Vector2(base.Character.Velocity.X, base.Character.JumpVelocity);
        base.Character.SetDirectionalAnimation("jump", forceReply: true);

        if (Character.IsOnFloor()) _hasDoubleJumped = false;
    }

    public override void Enter() => PerformJump();

    public override void PhysicsProcess(double delta) {
        base.Character.Velocity += new Vector2(0, base.Character.GravityForce * (float)delta);
        base.Character.MoveAndSlide();

        base.Character.AdjustDirection();

        if (base.Character.IsOnFloor()) {
            float horizontalDirection = Input.GetAxis("move_left", "move_right");
            if (horizontalDirection == 0) base.ChangeState("Idle");
            else base.ChangeState("Walk");
        }
    }

    public override void HandleInput(InputEvent @event) {
        if (@event.IsActionPressed("jump")) {
            if (base.Character.AllowDoubleJump && !_hasDoubleJumped) {
                PerformJump();
                _hasDoubleJumped = true;
            }
        }

        if (Input.IsActionPressed("move_left")) {
            base.Character.Velocity = new Vector2(
                -base.Character.BaseSpeed, base.Character.Velocity.Y
            );
        } else if (Input.IsActionPressed("move_right")) {
            base.Character.Velocity = new Vector2(
                base.Character.BaseSpeed, base.Character.Velocity.Y
            );
        }
    }
}
