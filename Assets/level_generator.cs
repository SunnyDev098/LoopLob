using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class LevelGenerator : MonoBehaviour
{
    [Header("Config & References")]
    public StageConfig config;
    private Transform _ball;
    private Camera _cam;

    // pools
    private SimplePool _planetPool;
    private Dictionary<GameObject, SimplePool> _enemyPools;

    // active instances
    private List<GameObject> _activePlanets = new List<GameObject>();
    private List<GameObject> _activeEnemies = new List<GameObject>();

    private float _nextStageY;


    void Start()
    {
        // grab refs
        _ball = GameObject.FindWithTag("ball").transform;
        _cam = Camera.main;

        // init planet pool & tutorial planets
        _planetPool = new SimplePool(config.planetPrefab,
                                     config.tutorialPlanets.Length,
                                     this.transform);

        foreach (var tp in config.tutorialPlanets)
        {
            var go = _planetPool.Get();
            go.transform.position = tp.position;
            go.transform.localScale = Vector3.one * tp.size;
            _activePlanets.Add(go);
        }

        // init enemy pools
        _enemyPools = new Dictionary<GameObject, SimplePool>();
        foreach (var prof in config.enemyProfiles)
        {
            if (prof.prefab == null) continue;
            if (!_enemyPools.ContainsKey(prof.prefab))
                _enemyPools[prof.prefab] =
                  new SimplePool(prof.prefab, 5, this.transform);
        }

        // set up first spawn threshold
        _nextStageY = config.tutorialEndY;
    }

    void Update()
    {
        // in case the ball climbs multiple stages in one frame:
        while (_ball.position.y > _nextStageY - config.triggerOffset)
        {
            GenerateStage();
            _nextStageY += config.stageHeight;
            CleanupBelow(_ball.position.y - config.stageHeight);
        }
    }

    private void GenerateStage()
    {
        // normalized difficulty from 0→1
        float diff = Mathf.Clamp01(_nextStageY / config.maxDifficultyHeight);

        // filter the enemy list by diff
        var valid = config.enemyProfiles
                      .Where(p => diff >= p.minDifficulty && diff <= p.maxDifficulty)
                      .ToList();
        if (valid.Count == 0) return;

        // build total weight
        float totalW = valid.Sum(p => p.spawnWeight);
        int count = Mathf.RoundToInt(config.spawnsPerMeter * config.stageHeight);

        // camera bounds
        float aspect = (float)Screen.width / Screen.height;
        float camH = _cam.orthographicSize;
        float camW = camH * aspect;
        float left = _cam.transform.position.x - camW * config.horizontalMargin;
        float right = _cam.transform.position.x + camW * config.horizontalMargin;

        for (int i = 0; i < count; i++)
        {
            // random position in this stage
            float y = Random.Range(_nextStageY, _nextStageY + config.stageHeight);
            float x = Random.Range(left, right);
            Vector2 pos = new Vector2(x, y);

            // choose a profile by weight
            float pick = Random.value * totalW;
            EnemyProfile chosen = valid.Last();
            foreach (var p in valid)
            {
                pick -= p.spawnWeight;
                if (pick <= 0f) { chosen = p; break; }
            }

            // safety check against planets & other enemies
            if (!IsSafeToSpawn(pos, chosen.minSpacing))
                continue;

            // spawn it
            var pool = _enemyPools[chosen.prefab];
            var go = pool.Get();
            go.transform.position = pos;

            // call Initialize if applicable
            var ie = go.GetComponent<IEnemy>();
            ie?.Initialize(diff);

            _activeEnemies.Add(go);
        }
    }

    private bool IsSafeToSpawn(Vector2 pos, float minSpacing)
    {
        // against planets (use their scale as radius)
        foreach (var pl in _activePlanets)
        {
            float radius = pl.transform.localScale.x * 0.5f;
            if (Vector2.Distance(pos, pl.transform.position) < radius + minSpacing)
                return false;
        }

        // against other enemies
        foreach (var en in _activeEnemies)
        {
            if (Vector2.Distance(pos, en.transform.position) < minSpacing)
                return false;
        }

        return true;
    }

    private void CleanupBelow(float yThreshold)
    {
        // planets
        for (int i = _activePlanets.Count - 1; i >= 0; i--)
        {
            var pl = _activePlanets[i];
            if (pl.transform.position.y < yThreshold)
            {
                var pm = pl.GetComponent<PoolMember>();
                pm.Pool.Release(pl);
                _activePlanets.RemoveAt(i);
            }
        }

        // enemies
        for (int i = _activeEnemies.Count - 1; i >= 0; i--)
        {
            var en = _activeEnemies[i];
            if (en.transform.position.y < yThreshold)
            {
                var pm = en.GetComponent<PoolMember>();
                pm.Pool.Release(en);
                _activeEnemies.RemoveAt(i);
            }
        }
    }
}
