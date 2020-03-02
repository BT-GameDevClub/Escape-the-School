using UnityEngine;

public static class GroundCheck 
{
    // Checks if the player is on the ground. Allows for multiple rays since the player might be slightly off the edge.
    public static bool OnGround(Vector3 position, float jumpChecks, float width, float distanceToGround)
    {
        // Checks if the player is on the ground. Allows for multiple rays since the player might be slightly off the edge.
        for (int i = 0; i < jumpChecks; i++)
        {
            if (Physics2D.Raycast(new Vector2((position.x + width / jumpChecks * i) - width / 2 - width / jumpChecks / 2, position.y), Vector2.down, distanceToGround, 1 << LayerMask.NameToLayer("Ground"))) return true;
        }
        return false;
    }

}
