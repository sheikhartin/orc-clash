using System;

using Godot;

using OrcClash.Core;

namespace OrcClash.Characters;

public partial class BaseCharacter : CharacterBody2D {
    #region Exported properties
    [ExportGroup("Movement Settings")]
    [Export]
    public float BaseSpeed = 220f;
    [Export]
    public float Acceleration = 1.3f;

    [Export]
    public float JumpVelocity = -400f;
    [Export]
    public bool AllowDoubleJump = true;
    [Export]
    public float GravityForce = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

    [ExportGroup("Combat Settings")]
    [Export]
    public int MaxHealth = 100;
    [Export]
    public int AttackDamage = 40;

    [ExportGroup("Animation Settings")]
    [Export]
    public string AnimationPrefix = "default/";

    [ExportGroup("Debug Settings")]
    [Export]
    public bool DebugMode = false;
    #endregion

    #region Nodes
    public Sprite2D Sprite { get; private set; }

    public CollisionShape2D HurtboxCollision { get; private set; }
    public Area2D HitboxArea { get; private set; }

    public ProgressBar HealthBar { get; private set; }

    public AnimationPlayer AnimationPlayer { get; private set; }

    public StateMachine StateMachine { get; private set; }

    public Label DebugLabel { get; private set; }
    #endregion

    #region Character status properties
    public enum LookDirection { Left, Right }
    public LookDirection CurrentLookDirection = LookDirection.Right;

    protected string _activeMotionType;

    private int _currentHealth;
    public int CurrentHealth {
        get => this._currentHealth;
        private set {
            this._currentHealth = Mathf.Clamp(value, 0, this.MaxHealth);
            this.HealthBar.Value = this._currentHealth;
        }
    }
    #endregion

    public void TakeDamage(int damage) {
        if (damage <= 0) return;
        this.CurrentHealth -= damage;

        if (this.CurrentHealth <= 0) this.StateMachine.ChangeState("Death");
        else this.StateMachine.ChangeState("Hurt");
    }

    public void Heal(int amount) {
        if (amount <= 0) return;
        this.CurrentHealth = Mathf.Min(this.CurrentHealth + amount, this.MaxHealth);
    }

    protected virtual void InitializeStates() { }

    public void SetDirectionalAnimation(string motionType, bool forceReply = false) {
        if (string.IsNullOrEmpty(motionType)) {
            GD.PushWarning("The motion type can't be blank!");
            return;
        }

        string fullAnimationName = string.Concat(
            this.AnimationPrefix, motionType, "_", this.CurrentLookDirection.ToString().ToLower()
        );
        if (this.AnimationPlayer.HasAnimation(fullAnimationName)) {
            if (forceReply) this.AnimationPlayer.Stop();
            this.AnimationPlayer.Play(fullAnimationName);
            this._activeMotionType = motionType;
        } else {
            GD.PrintErr($"Animation '{fullAnimationName}' not found.");
        }
    }

    public virtual void AdjustDirection() {
        LookDirection newLookDirection = base.Velocity.X switch {
            < 0 => LookDirection.Left,
            > 0 => LookDirection.Right,
            _ => this.CurrentLookDirection
        };
        if (newLookDirection != this.CurrentLookDirection) {
            this.CurrentLookDirection = newLookDirection;
            SetDirectionalAnimation(this._activeMotionType);
        }
    }

    public override void _Ready() {
        this.Sprite = (
            base.GetNode<Sprite2D>("Sprite")
            ?? throw new NullReferenceException("'Sprite' node is missing in the scene.")
        );

        this.HurtboxCollision = (
            base.GetNode<CollisionShape2D>("HurtboxCollision")
            ?? throw new NullReferenceException("'HurtboxCollision' node is missing in the scene.")
        );
        this.HitboxArea = (
            base.GetNode<Area2D>("HitboxArea")
            ?? throw new NullReferenceException("'HitboxArea' node is missing in the scene.")
        );

        this.HealthBar = (
            base.GetNode<ProgressBar>("HealthBar")
            ?? throw new NullReferenceException("'HealthBar' node is missing in the scene.")
        );

        this.AnimationPlayer = (
            base.GetNode<AnimationPlayer>("AnimationPlayer")
            ?? throw new NullReferenceException("'AnimationPlayer' node is missing in the scene.")
        );

        this.StateMachine = (
            base.GetNode<StateMachine>("StateMachine")
            ?? throw new NullReferenceException("'StateMachine' node is missing in the scene.")
        );

        this.DebugLabel = (
            base.GetNode<Label>("DebugLabel")
            ?? throw new NullReferenceException("'DebugLabel' node is missing in the scene.")
        );
        this.DebugLabel.Visible = this.DebugMode;

        this.CurrentHealth = this.MaxHealth;

        InitializeStates();
        this.StateMachine.EnterInitialState();
    }

    public override void _Process(double _delta) {
        if (DebugMode) {
            this.DebugLabel.Text = string.Join(
                '\n',
                $"State: {this.StateMachine.CurrentState}",
                $"Animation: {this.AnimationPlayer.CurrentAnimation}"
            );
        }
    }
}

public abstract class BaseState<T> : IState where T : BaseCharacter {
    protected readonly T Character;

    protected BaseState(T character) => this.Character = character;

    public virtual void Enter() { }
    public virtual void Exit() { }
    public virtual void Process(double delta) { }
    public virtual void PhysicsProcess(double delta) { }
    public virtual void HandleInput(InputEvent @event) { }

    protected void ChangeState(string newState) => this.Character.StateMachine.ChangeState(newState);
}
