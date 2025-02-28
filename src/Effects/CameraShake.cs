using Godot;

namespace OrcClash.Effects;

public partial class CameraShake : Camera2D {
    [Export]
    private float _shakePower = 0.0f;
    [Export]
    private float _shakeReduction = 0.9f;
    [Export]
    private float _shakeDuration = 0.0f;

    private RandomNumberGenerator _randomNumberGenerator = new();

    public override void _Process(double delta) {
        if (this._shakeDuration <= 0) {
            base.Offset = Vector2.Zero;
            return;
        }

        base.Offset = new Vector2(
            this._randomNumberGenerator.RandfRange(-this._shakePower, this._shakePower),
            this._randomNumberGenerator.RandfRange(-this._shakePower, this._shakePower)
        );

        // Reduce shake over time
        this._shakeDuration -= (float)delta;
        this._shakePower *= this._shakeReduction;
    }

    public void Shake(float power, float duration) {
        this._shakePower = power;
        this._shakeDuration = duration;
    }
}
