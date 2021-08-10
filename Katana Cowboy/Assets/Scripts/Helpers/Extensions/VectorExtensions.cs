using UnityEngine;

public static class VectorExtensions
{
    public static Vector2 ConvertVectorToBasis(Vector2 convertVector, Vector2 baseA, Vector2 baseB)
    {
        float denom = baseA.x * baseB.y - baseB.x * baseA.y;
        if (denom == 0.0f)
        {
            return Vector2.zero;
        }

        
        float a = (convertVector.x * baseB.y - baseB.x * convertVector.y) / denom;
        float b = (baseA.x * convertVector.y - convertVector.x * baseA.y) / denom;

        return a * baseA + b * baseB;
    }
}
