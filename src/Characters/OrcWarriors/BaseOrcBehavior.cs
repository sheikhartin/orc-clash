namespace OrcClash.Characters.OrcWarriors;

public partial class BaseOrcBehavior : BaseCharacter {
    protected override void InitializeStates() {
        base.StateMachine.AddState("Idle", new States.IdleState(this));
        base.StateMachine.AddState("Walk", new States.WalkState(this));
        base.StateMachine.AddState("Jump", new States.JumpState(this));
    }
}
