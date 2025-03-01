using Godot;

namespace OrcClash.Characters.OrcWarriors.States;

public partial class HurtState : BaseState<BaseOrcBehavior> {
    private float _cameraShakePower = 2.5f;
    private float _cameraShakeDuration = 0.12f;

    public HurtState(BaseOrcBehavior character) : base(character) { }

    private void OnAnimationFinished(StringName animName) => base.ChangeState("Walk");

    public override void Enter() {
        base.Character.SetDirectionalAnimation("hurt");

        // TODO: Apply knockback

        base.Character.Camera.Shake(
            power: this._cameraShakePower, duration: this._cameraShakeDuration
        );

        base.Character.AnimationPlayer.AnimationFinished += OnAnimationFinished;
    }

    public override void Exit() => base.Character.AnimationPlayer.AnimationFinished -= OnAnimationFinished;
}
