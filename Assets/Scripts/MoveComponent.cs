using UnityEngine;

public class MoveComponent : MonoBehaviour
{
    // A component for object movement.

    // Allows for the movement of GameObjects with inertia & max speed parameters.
    
    public float accel;
    public float decel;
	public Vector2 maxSpeed; // The maximum speed for each axis of movement.

    [HideInInspector] public Vector2 Speed; // The current movement speed of the object.

    // A version of Mathf.Sign() that can return 0.
    int Sign(float num)
    {
        //                  |                  |
        return (num == 0) ? ((num < 0) ? -1 : 1) : 0;
    }
    
    float Accelerate(float speedVar, float axis, float delta)
    {
        return speedVar + (accel * axis * delta);
    }

    float Decelerate(float speedVar, float delta)
    {
        return (Mathf.Abs(speedVar) <= decel) ? 0f : speedVar + (decel * (float)Sign(-speedVar) * delta);
    }

    float Cap(float speedVar, float speedCap, float magni)
    {
        return (Mathf.Abs(speedVar) > (speedCap * Mathf.Abs(magni))) ? (speedCap * magni) : speedVar;
    }

    public Vector2 Move(Vector2 moveDir, float delta)
    {
        // acceleration & deceleration
		Speed.x = (Mathf.Abs(moveDir.x) != 0f) ? Accelerate(Speed.x, moveDir.x, delta) : Decelerate(Speed.x, delta);
        Speed.y = (Mathf.Abs(moveDir.y) != 0f) ? Accelerate(Speed.y, moveDir.y, delta) : Decelerate(Speed.y, delta);

		// prevents the object from going too fast
		Speed.x = Cap(Speed.x, maxSpeed.x, moveDir.x);
        Speed.y = Cap(Speed.y, maxSpeed.y, moveDir.y);

		return new Vector2(Speed.x, Speed.y) * delta;
    }
}
