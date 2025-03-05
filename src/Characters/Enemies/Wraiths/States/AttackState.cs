using Godot;

namespace OrcClash.Characters.Enemies.Wraiths.States;

public partial class AttackState : BaseState<BaseWraithBehavior> {
    private bool _attackFinished;

    public float DamageInterval = 1.0f;
    private float _damageCooldown = 0.0f;

    public AttackState(BaseWraithBehavior character) : base(character) { }

    private void OnHitboxAreaBodyEntered(Node2D body) {
        if (body is BaseCharacter character && character.IsInGroup("players"))
            character.TakeDamage(base.Character.AttackDamage);
    }

    private void OnAnimationFinished(StringName animName) => this._attackFinished = true;

    public override void Enter() {
        base.Character.Velocity = new Vector2(0, base.Character.Velocity.Y);

        base.Character.SetDirectionalAnimation("attack");
        this._attackFinished = false;

        base.Character.HitboxArea.BodyEntered += OnHitboxAreaBodyEntered;

        base.Character.AnimationPlayer.AnimationFinished += OnAnimationFinished;
    }

    public override void Exit() {
        base.Character.HitboxArea.BodyEntered -= OnHitboxAreaBodyEntered;

        base.Character.AnimationPlayer.AnimationFinished -= OnAnimationFinished;
    }

    public override void PhysicsProcess(double delta) {
        if (this._attackFinished) base.ChangeState("Move");
    }
}
