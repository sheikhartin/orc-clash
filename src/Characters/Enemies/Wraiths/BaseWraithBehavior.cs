using Godot;

namespace OrcClash.Characters.Enemies.Wraiths;

public partial class BaseWraithBehavior : BaseCharacter {
    #region Nodes
    public RayCast2D PlayerDetectorRayCast;
    public RayCast2D GroundDetectorRayCast;
    #endregion

    protected override void InitializeStates() {
        base.StateMachine.AddState("Idle", new States.IdleState(this));
        base.StateMachine.AddState("Move", new States.MoveState(this));
        base.StateMachine.AddState("Attack", new States.AttackState(this));
        base.StateMachine.AddState("Hurt", new States.HurtState(this));
        base.StateMachine.AddState("Death", new States.DeathState(this));
    }

    public override void _Ready() {
        base._Ready();
        base.AddToGroup("enemies");

        this.PlayerDetectorRayCast = base.GetNode<RayCast2D>("%PlayerDetectorRayCast");
        this.GroundDetectorRayCast = base.GetNode<RayCast2D>("%GroundDetectorRayCast");

        this.CurrentLookDirection = LookDirection.Left;
    }
}
