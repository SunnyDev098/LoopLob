using UnityEngine;
using System;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Procedural/Stage Config")]
public class StageConfig : ScriptableObject
{
    [Header("Planet & Tutorial Setup")]
    [Tooltip("Planet prefab used for both tutorial & normal stages.")]
    public GameObject planetPrefab;

    [Tooltip("When ball.Y passes this, we leave the tutorial.")]
    public float tutorialEndY = 50;

    [Tooltip("Radius around tutorial planets for safe‐zone checks.")]
    public float tutorialMinSpacing = 3f;

    [Serializable]
    public struct TutorialPlanet
    {
        public Vector2 position;
        public float size;
    }

    [Tooltip("Fixed planets at the very start (tutorial).")]
    public TutorialPlanet[] tutorialPlanets;


    [Header("Stage & Spawn Settings")]
    [Tooltip("How tall each stage chunk is.")]
    public float stageHeight = 100f;

    [Tooltip("How close to the top of the next stage before spawning.")]
    public float triggerOffset = 30f;

    [Tooltip("Number of total spawns per World‐unit (meter).")]
    public float spawnsPerMeter = 0.1f;

    [Tooltip("Horizontal margin as fraction of camera width (0..1).")]
    [Range(0f, 1f)] public float horizontalMargin = 0.85f;

    [Header("Difficulty Scaling")]
    [Tooltip("Meters at which difficulty = 1.  Below that it’s linear from 0→1.")]
    public float maxDifficultyHeight = 1000f;

    [Header("Enemy Types")]
    [Tooltip("All enemy/item types that can spawn after the tutorial.")]
    public List<EnemyProfile> enemyProfiles = new List<EnemyProfile>();
}
