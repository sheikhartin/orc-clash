using Godot;

using OrcClash.Characters;

namespace OrcClash.Environment;

public partial class EnergyBooster : Area2D {
    [Export]
    public float HealInterval = 1f;
    [Export]
    public float HealPercent = 0.2f;

    private Timer _healTimer;
    private BaseCharacter _player;

    private void OnBodyEntered(Node2D body) {
        if (body is BaseCharacter character && character.IsInGroup("players")) {
            this._player = character;
            this._healTimer.Start();
        }
    }

    private void OnBodyExited(Node2D body) {
        if (body == this._player) {
            this._healTimer.Stop();
            this._player = null;
        }
    }

    private void OnHeal() {
        if (this._player is not null) {
            GD.Print("Healing player...");
            int healAmount = (int)(this._player.MaxHealth * this.HealPercent);
            this._player.Heal(healAmount);
        }
    }

    public override void _Ready() {
        this._healTimer = new Timer {
            WaitTime = this.HealInterval,
            OneShot = false
        };
        base.AddChild(this._healTimer);
        this._healTimer.Timeout += OnHeal;

        base.BodyEntered += OnBodyEntered;
        base.BodyExited += OnBodyExited;
    }
}
