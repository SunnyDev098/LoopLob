using UnityEngine;

[CreateAssetMenu(menuName = "Procedural/Enemy Profile")]
public class EnemyProfile : ScriptableObject
{
    [Tooltip("The prefab to be pooled & spawned.")]
    public GameObject prefab;

    [Tooltip("Normalized difficulty [0..1] at which this begins to appear.")]
    [Range(0f, 1f)] public float minDifficulty = 0f;

    [Tooltip("Normalized difficulty [0..1] beyond which it no longer spawns.")]
    [Range(0f, 1f)] public float maxDifficulty = 1f;

    [Tooltip("Relative weight for random selection.")]
    public float spawnWeight = 1f;

    [Tooltip("Minimum safe spacing (in world units) to _any_ other object.")]
    public float minSpacing = 2f;
}
public interface IEnemy
{
    /// <summary>
    /// Called immediately after spawn.  difficulty ∈ [0..1].
    /// </summary>
    void Initialize(float normalizedDifficulty);
}
