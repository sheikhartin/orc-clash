extends Path2D

@export var speed: float = 0.3
@export var ping_pong: bool = true  # If true, the platform moves back and forth along the path.
@export var wait_time: float = 0.9

@onready var path_follow: PathFollow2D = $PathFollow
@onready var wait_timer: Timer = $Timer

enum Direction { FORWARD = 1, BACKWARD = -1 }
var direction: Direction = Direction.FORWARD


func _ready() -> void:
	if not path_follow:
		printerr("Error: `PathFollow` node not found!")
		set_process(false)
		return

	path_follow.rotates = false

	wait_timer.one_shot = true
	wait_timer.wait_time = wait_time


func _process(delta: float) -> void:
	if not wait_timer.is_stopped():
		return

	var displacement: float = speed * delta * direction
	path_follow.progress_ratio += displacement

	if path_follow.progress_ratio <= 0.0 or path_follow.progress_ratio >= 1.0:
		path_follow.progress_ratio = clamp(path_follow.progress_ratio, 0.0, 1.0)

		wait_timer.start()

		if ping_pong:
			direction = (
				Direction.BACKWARD
				if direction == Direction.FORWARD
				else Direction.FORWARD
			)
