extends Node

const SETTINGS_PATH: String = "user://settings.cfg"

var settings: Dictionary = {
	"audio":
	{
		"music_volume_percent": 67.0,
		"sfx_volume_percent": 67.0,
	},
}


func _init() -> void:
	load_settings()


func save_settings() -> void:
	var config := ConfigFile.new()

	config.set_value(
		"audio", "music_volume_percent", settings["audio"]["music_volume_percent"]
	)
	config.set_value(
		"audio", "sfx_volume_percent", settings["audio"]["sfx_volume_percent"]
	)

	var err := config.save(SETTINGS_PATH)
	if err != OK:
		push_error("Failed to save settings to '%s'!" % SETTINGS_PATH)
	else:
		print("Settings saved successfully.")


func load_settings() -> void:
	var config := ConfigFile.new()
	if config.load(SETTINGS_PATH) != OK:
		print("No saved settings found, using defaults...")
		return

	settings["audio"]["music_volume_percent"] = config.get_value(
		"audio", "music_volume_percent", settings["audio"]["music_volume_percent"]
	)
	settings["audio"]["sfx_volume_percent"] = config.get_value(
		"audio", "sfx_volume_percent", settings["audio"]["sfx_volume_percent"]
	)

	print("Settings loaded: ", settings)


func get_setting(section: String, key: String, default: Variant = null) -> Variant:
	if settings.has(section) and settings[section].has(key):
		return settings[section][key]
	return default


func set_setting(section: String, key: String, value: Variant) -> void:
	settings[section][key] = value
	save_settings()
