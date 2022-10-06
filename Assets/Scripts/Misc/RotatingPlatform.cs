using UnityEngine;
using NaughtyAttributes;

public class RotatingPlatform : MonoBehaviour
{
    //public float Speed {
    //    get {
    //        return rpm * 2f * Mathf.PI * radius / 60f / ppu;
    //    }
    //}
    [SerializeField][Label("radius(px)")] float radius;
    [SerializeField][Label("rpm(rotation/minute)")] float rpm;
    [SerializeField][NonReorderable] Transform[] platforms;
    float ppu = 16f;
    float startTime;

    private void Awake()
    {
        startTime = Time.fixedTime;
    }

    Vector2 Rotate(Vector2 point, float rad)
    {
        return new Vector2(
            Mathf.Cos(rad) * point.x - Mathf.Sin(rad) * point.y,
            Mathf.Sin(rad) * point.x + Mathf.Cos(rad) * point.y);
    }

    void UpdatePlatformLocation(float time)
    {
        for (int i = 0; i < platforms.Length; i++)
        {
            Vector2 startPosition = Rotate(new Vector2(radius, 0f), 2f * Mathf.PI * ((float)i / (float)platforms.Length));
            platforms[i].position = Rotate(startPosition, -time * rpm * 2f * Mathf.PI / 60f) / ppu;
            platforms[i].position = new Vector2(platforms[i].position.x, (48f / 120f) * platforms[i].position.y);
        }
    }

    private void FixedUpdate()
    {
        float currentTime = Time.fixedTime - startTime;
        UpdatePlatformLocation(currentTime);
    }
}
