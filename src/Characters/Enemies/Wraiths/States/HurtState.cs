using Godot;

namespace OrcClash.Characters.Enemies.Wraiths.States;

public partial class HurtState : BaseState<BaseWraithBehavior> {
    public HurtState(BaseWraithBehavior character) : base(character) { }

    private void OnAnimationFinished(StringName animName) => base.ChangeState("Move");

    public override void Enter() {
        base.Character.SetDirectionalAnimation("hurt");

        base.Character.AnimationPlayer.AnimationFinished += OnAnimationFinished;
    }

    public override void Exit() => base.Character.AnimationPlayer.AnimationFinished -= OnAnimationFinished;
}
