using Godot;

namespace OrcClash.Characters.Enemies.Wraiths.States;

public partial class IdleState : BaseState<BaseWraithBehavior> {
    public float IdleDuration = 2.5f;
    private Timer _idleTimer;

    public IdleState(BaseWraithBehavior character) : base(character) {
        this._idleTimer = new Timer();
        this._idleTimer.WaitTime = this.IdleDuration;
        this._idleTimer.OneShot = true;
        // this._idleTimer.Autostart = false;
        this._idleTimer.Timeout += OnIdleTimerTimeout;
    }

    private void OnIdleTimerTimeout() => base.ChangeState("Move");

    public override void Enter() {
        base.Character.Velocity = Vector2.Zero;
        base.Character.SetDirectionalAnimation("idle");

        base.Character.AddChild(this._idleTimer);
        this._idleTimer.Start();
    }

    public override void Exit() {
        this._idleTimer.Stop();
        base.Character.RemoveChild(this._idleTimer);
    }

    public override void PhysicsProcess(double delta) {
        if (base.Character.PlayerDetectorRayCast.IsColliding()) {
            GodotObject collider = base.Character.PlayerDetectorRayCast.GetCollider();
            if (
                collider is BaseCharacter character
                && character.IsInGroup("players")
            ) base.ChangeState("Move");
        }
    }
}
