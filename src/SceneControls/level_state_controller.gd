class_name LevelStateController
extends Node

@onready var collectible_keys: Node2D = $CollectibleKeys
@onready var key_counter_label: Label = $CollectibleKeys/KeyCounterLabel

var total_keys: int = 0
var collected_keys: int = 0


func _ready() -> void:
	for item in collectible_keys.get_children():
		if item is CollectibleKey:
			total_keys += 1
			item.collected.connect(_on_key_collected)

	update_key_counter()


func _on_key_collected() -> void:
	collected_keys += 1
	update_key_counter()

	if collected_keys == total_keys:
		print("All keys collected!")


func update_key_counter() -> void:
	key_counter_label.text = "🔑 %d / %d" % [collected_keys, total_keys]


func _on_victory_gate_body_entered(body: Node2D) -> void:
	if body.is_in_group("players") and collected_keys == total_keys:
		get_tree().change_scene_to_file(
			"res://scenes/user_interfaces/start_menu.tscn"
		)
