using Godot;

namespace OrcClash.Characters.Enemies.Wraiths.States;

public partial class MoveState : BaseState<BaseWraithBehavior> {
    private bool _isChasingPlayer = false;

    public MoveState(BaseWraithBehavior character) : base(character) { }

    private void OnHitboxAreaBodyEntered(Node2D body) {
        if (body is BaseCharacter character && character.IsInGroup("players")) {
            base.ChangeState("Attack");
        } else if (body is TileMapLayer) {
            base.Character.CurrentLookDirection = (
                base.Character.CurrentLookDirection == BaseCharacter.LookDirection.Left
                ? BaseCharacter.LookDirection.Right
                : BaseCharacter.LookDirection.Left
            );
            base.Character.SetDirectionalAnimation("move");
        }
    }

    public override void Enter() {
        base.Character.SetDirectionalAnimation("move");

        base.Character.HitboxArea.BodyEntered += OnHitboxAreaBodyEntered;
    }

    public override void Exit() => base.Character.HitboxArea.BodyEntered -= OnHitboxAreaBodyEntered;

    public override void PhysicsProcess(double delta) {
        GodotObject collider = base.Character.PlayerDetectorRayCast.GetCollider();
        if (
            collider is BaseCharacter character
            && character.IsInGroup("players")
        ) this._isChasingPlayer = true;
        else this._isChasingPlayer = false;

        if (!base.Character.GroundDetectorRayCast.IsColliding()) {
            base.Character.CurrentLookDirection = (
                base.Character.CurrentLookDirection == BaseCharacter.LookDirection.Left
                ? BaseCharacter.LookDirection.Right
                : BaseCharacter.LookDirection.Left
            );
            base.Character.SetDirectionalAnimation("move");
        }

        KinematicCollision2D lastSlideCollision = base.Character.GetLastSlideCollision();
        if (lastSlideCollision is not null) {
            GodotObject collisionObject = lastSlideCollision.GetCollider();
            if (
                collisionObject is BaseCharacter targetCharacter
                && targetCharacter.IsInGroup("players")
            ) {
                Vector2 directionToPlayer = (
                    targetCharacter.GlobalPosition - base.Character.GlobalPosition
                ).Normalized();
                base.Character.CurrentLookDirection = (
                    directionToPlayer.X < 0
                    ? BaseCharacter.LookDirection.Left
                    : BaseCharacter.LookDirection.Right
                );
                base.Character.SetDirectionalAnimation("move");

                this._isChasingPlayer = true;
            }
        }

        float direction = base.Character.CurrentLookDirection == BaseCharacter.LookDirection.Left ? -1 : 1;
        float speed = (
            this._isChasingPlayer
            ? base.Character.BaseSpeed * base.Character.Acceleration
            : base.Character.BaseSpeed
        );
        base.Character.Velocity = new Vector2(direction * speed, base.Character.Velocity.Y);
        base.Character.SetDirectionalAnimation("move");
        base.Character.MoveAndSlide();
    }
}
