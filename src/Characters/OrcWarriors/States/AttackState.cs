using Godot;

namespace OrcClash.Characters.OrcWarriors.States;

public partial class AttackState : BaseState {
    private bool _attackFinished;

    public AttackState(BaseCharacter character) : base(character) { }

    private void OnAnimationFinished(StringName animName) => this._attackFinished = true;

    public override void Enter() {
        base.Character.SetDirectionalAnimation("attack");
        this._attackFinished = false;

        base.Character.AnimationPlayer.AnimationFinished += OnAnimationFinished;
    }

    public override void Exit() => base.Character.AnimationPlayer.AnimationFinished -= OnAnimationFinished;

    public override void PhysicsProcess(double delta) {
        base.Character.Velocity = new Vector2(
            0, base.Character.Velocity.Y + base.Character.GravityForce * (float)delta
        );
        base.Character.MoveAndSlide();

        base.Character.AdjustDirection();

        if (this._attackFinished) {
            float horizontalDirection = Input.GetAxis("move_left", "move_right");
            if (horizontalDirection == 0) base.ChangeState("Idle");
            else base.ChangeState("Walk");
        }
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
        }
    }
}
