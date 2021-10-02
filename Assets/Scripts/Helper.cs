using UnityEngine;

public static class Helper
{
    public static Vector3 GetRandomDir()
    {
        return new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)).normalized;
    }

    public static bool Reached(Vector3 currentPosition, Vector3 targetPosition) =>
        Vector2.Distance(currentPosition, targetPosition) < 1e-2;
    
    public static bool Reached(Quaternion currentRotation, Quaternion targetRotation) =>
        Quaternion.Angle(currentRotation, targetRotation) < 1e-2;
    
}