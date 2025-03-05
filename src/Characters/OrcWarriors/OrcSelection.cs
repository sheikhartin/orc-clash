using System.Collections.Generic;

using Godot;

namespace OrcClash.Characters.OrcWarriors;

public partial class OrcSelection : Node2D {
    public enum OrcType {
        SwampSlasher,
        JungleAxeman,
        MeadowCrusher,
    }

    private static readonly Dictionary<OrcType, string> OrcScenePaths = new() {
        { OrcType.SwampSlasher, "res://scenes/characters/orc_warriors/swamp_slasher.tscn" },
		// { OrcType.JungleAxeman, "res://scenes/characters/orc_warriors/jungle_axeman.tscn" },
		// { OrcType.MeadowCrusher, "res://scenes/characters/orc_warriors/meadow_crusher.tscn" },
	};

    [Export]
    public OrcType Type { get; protected set; } = OrcType.SwampSlasher;

    public void SpawnSelectedOrc() {
        if (!OrcScenePaths.TryGetValue(Type, out string scenePath)) {
            GD.PushError($"Invalid orc type: {Type}");
            return;
        }

        PackedScene orcScene = GD.Load<PackedScene>(scenePath);
        CharacterBody2D orcInstance = orcScene.Instantiate<CharacterBody2D>();
        orcInstance.AddToGroup("players");

        base.GetParent().CallDeferred("add_child", orcInstance);
        orcInstance.GlobalPosition = base.GlobalPosition;

        GD.Print($"Spawned {Type} at position {orcInstance.GlobalPosition}");
    }

    public override void _Ready() => SpawnSelectedOrc();
}
