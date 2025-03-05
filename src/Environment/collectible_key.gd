class_name CollectibleKey
extends Area2D

signal collected

@onready var audio_player: AudioStreamPlayer2D = $AudioPlayer


func _on_body_entered(body: Node2D) -> void:
	if body.is_in_group("players"):
		audio_player.play()
		collected.emit()

		queue_free()
