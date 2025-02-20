using Godot;

using OrcClash.Characters;

namespace OrcClash.Core;

public partial class Deadzone : Area2D {
    public override void _Ready() => base.BodyEntered += OnBodyEntered;

    private void OnBodyEntered(Node2D body) {
        if (body is BaseCharacter character) {
            character.TakeDamage(character.MaxHealth);
            character.QueueFree();
        }
    }
}
