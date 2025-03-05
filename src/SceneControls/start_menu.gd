extends CanvasLayer

@onready var start_screen_layout: VBoxContainer = %StartScreenLayout
@onready var game_level_selection_layout: VBoxContainer = %GameLevelSelectionLayout
@onready var game_settings_layout: VBoxContainer = %GameSettingsLayout

@onready var music_volume_slider: HSlider = %MusicVolumeSlider
@onready var sfx_volume_slider: HSlider = %SFXVolumeSlider

@onready var main_menu_return_button: Button = %MainMenuReturnButton


func _ready() -> void:
	var music_volume_percent: float = SettingsManager.get_setting(
		"audio", "music_volume_percent"
	)
	var sfx_volume_percent: float = SettingsManager.get_setting(
		"audio", "sfx_volume_percent"
	)

	music_volume_slider.value = music_volume_percent
	sfx_volume_slider.value = sfx_volume_percent


func _on_game_level_selection_button_pressed() -> void:
	start_screen_layout.visible = false
	main_menu_return_button.show()

	game_level_selection_layout.visible = true


func _on_level_button_01_pressed() -> void:
	var packed_scene: PackedScene = preload("res://scenes/levels/game_level_01.tscn")
	get_tree().change_scene_to_packed(packed_scene)


func _on_game_settings_button_pressed() -> void:
	start_screen_layout.visible = false
	main_menu_return_button.show()

	game_settings_layout.visible = true


func _on_music_volume_slider_value_changed(value: float) -> void:
	AudioManager.music_volume_changed.emit(value)


func _on_sfx_volume_slider_value_changed(value: float) -> void:
	AudioManager.sfx_volume_changed.emit(value)


func _on_exit_button_pressed() -> void:
	get_tree().quit()


func _on_main_menu_return_button_pressed() -> void:
	game_level_selection_layout.visible = false
	game_settings_layout.visible = false
	main_menu_return_button.hide()

	start_screen_layout.visible = true
