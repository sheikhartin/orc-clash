extends Node

signal music_volume_changed(new_volume_percent: float)
signal sfx_volume_changed(new_volume_percent: float)

const AUDIO_BUS_MUSIC: String = "Music"
const AUDIO_BUS_SFX: String = "SFX"

const AUDIO_VOLUME_MIN_DB: float = -20.0
const AUDIO_VOLUME_MAX_DB: float = 15.0


func _ready() -> void:
	var music_volume_percent: float = SettingsManager.get_setting(
		"audio", "music_volume_percent"
	)
	var sfx_volume_percent: float = SettingsManager.get_setting(
		"audio", "sfx_volume_percent"
	)

	set_music_volume_by_percent(music_volume_percent)
	set_sfx_volume_by_percent(sfx_volume_percent)

	music_volume_changed.connect(_on_music_volume_changed)
	sfx_volume_changed.connect(_on_sfx_volume_changed)


func percent_to_db(percent: float) -> float:
	return lerp(AUDIO_VOLUME_MIN_DB, AUDIO_VOLUME_MAX_DB, percent / 100.0)


func db_to_percent(db: float) -> float:
	return clamp(
		inverse_lerp(AUDIO_VOLUME_MIN_DB, AUDIO_VOLUME_MAX_DB, db) * 100.0, 0, 100
	)


func set_music_volume_by_percent(volume_percent: float) -> void:
	var bus_index: int = AudioServer.get_bus_index(AUDIO_BUS_MUSIC)
	if bus_index < 0:
		push_error("Music bus '%s' not found!" % AUDIO_BUS_MUSIC)
		return

	var volume_db: float = clamp(
		percent_to_db(volume_percent), AUDIO_VOLUME_MIN_DB, AUDIO_VOLUME_MAX_DB
	)
	AudioServer.set_bus_volume_db(bus_index, volume_db)
	SettingsManager.set_setting("audio", "music_volume_percent", volume_percent)
	print("Music volume adjusted: %.1f%% (%.2f dB)" % [volume_percent, volume_db])


func set_sfx_volume_by_percent(volume_percent: float) -> void:
	var bus_index: int = AudioServer.get_bus_index(AUDIO_BUS_SFX)
	if bus_index < 0:
		push_error("SFX bus '%s' not found!" % AUDIO_BUS_MUSIC)
		return

	var volume_db: float = clamp(
		percent_to_db(volume_percent), AUDIO_VOLUME_MIN_DB, AUDIO_VOLUME_MAX_DB
	)
	AudioServer.set_bus_volume_db(bus_index, volume_db)
	SettingsManager.set_setting("audio", "sfx_volume_percent", volume_percent)
	print("SFX volume adjusted: %.1f%% (%.2f dB)" % [volume_percent, volume_db])


func _on_music_volume_changed(new_volume_percent: float) -> void:
	set_music_volume_by_percent(new_volume_percent)


func _on_sfx_volume_changed(new_volume_percent: float) -> void:
	set_sfx_volume_by_percent(new_volume_percent)
