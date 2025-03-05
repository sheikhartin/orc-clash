using Godot;

using OrcClash.Characters;

namespace OrcClash.Environment;

public partial class Deadzone : Area2D {
    public override void _Ready() => base.BodyEntered += OnBodyEntered;

    private void OnBodyEntered(Node2D body) {
        if (body is BaseCharacter character) {
            character.TakeDamage(character.MaxHealth);
            if (body.IsInGroup("enemies")) {
                character.QueueFree();
            } else {
                GD.Print("Game over! You lost. The level will be reloaded...");
                string currentScenePath = GetTree().CurrentScene.SceneFilePath;
                GetTree().ChangeSceneToFile(currentScenePath);
            }
        }
    }
}
