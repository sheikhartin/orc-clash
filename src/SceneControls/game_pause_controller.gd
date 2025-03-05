extends Control

@onready var pause_menu_layout: VBoxContainer = $PauseMenuLayout

var game_ended: bool = false


func _ready() -> void:
	pause_menu_layout.visible = false


func _unhandled_input(event: InputEvent) -> void:
	if game_ended or not event.is_action_pressed("toggle_pause"):
		return

	var paused_state: bool = not get_tree().is_paused()
	get_tree().paused = paused_state
	pause_menu_layout.visible = paused_state


func set_game_ended(ended: bool) -> void:
	game_ended = ended
	if game_ended:
		pause_menu_layout.visible = false


func _on_continue_button_pressed() -> void:
	get_tree().paused = false
	pause_menu_layout.visible = false


func _on_back_to_menu_button_pressed() -> void:
	get_tree().paused = false
	get_tree().change_scene_to_file("res://scenes/user_interfaces/start_menu.tscn")
