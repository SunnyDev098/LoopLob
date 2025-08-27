using System.Collections.Generic;
using TMPro;
using UnityEngine;



public class FinalProceduralGenerator : MonoBehaviour
{
    [Header("Twin Gate Settings")]
    public GameObject twinGatePrefab;
    public float twinGateStartHeight = 1000f;
    public Vector2 twinGateSpawnInterval = new Vector2(200f, 300f);
    private float nextTwinGateY = 0f;

    [Header("Coin Trail Control")]
    public float coinSpacing = 1.0f;     // Constant distance between coins on all trails
    public float minCurveHeight = 1.0f;  // Prevent completely flat trails
    public float maxCurveHeight = 3.0f;  // Max curve
    public float obstacleAvoidOffset = 2.0f; // How far to curve away from nearest obstacle
    public GameObject coinPrefab;
    [Range(0f, 1f)] public float coinTrailChance = 0.5f;
    public int minCoinsPerTrail = 5;
    public int maxCoinsPerTrail = 10;
    public float curveHeight = 2f;
    public bool allowCurvedTrails = true;
    public float planetSafeOffset = 1.5f; // NEW: distance from planet
    [Range(0f, 1f)] public float freeCoinPathChance = 0.3f;     // toggle straight/curved




    public GameObject dangerZonePrefab;    // <--- for "danger_zone" objects
    public GameObject safeZonePrefab;      // <--- for "safe_zone" objects






    public float fadeDuration = 1f;
    public GameObject sprite_cariaer;
    private float startAlpha;



    [Header("References")]
    public Transform ball;
    public Camera mainCamera;



    [Header("Prefabs")]
    public GameObject planetPrefab;
    public GameObject triangle_spike_prefab;
    public GameObject laser_gun_prefabs;
    public GameObject distance_banner;

    private int last_distance_banner_height;
    public GameObject badPlanetPrefab;
    public GameObject blackHolePrefab;
    public GameObject spikePrefab;
    public GameObject magnetPrefab;
    public GameObject batteryPrefab;
    public GameObject shieldPrefab;
    public GameObject blueZonePrefab;
    public GameObject redZonePrefab;
    public GameObject beam_emitter;

    [Header("Screen Boundaries")]
    [Range(0.7f, 0.95f)] public float horizontalMargin = 0.85f;
    private float leftBound;
    private float rightBound;

    [Header("Planet Settings")]
    public float minPlanetDistance = 4f;
    public float maxPlanetDistance = 7f;
    public float initialPlanetSize = 2.5f;
    public float minPlanetSize = 1.0f;
    public float bigPlanetChance = 0.15f;
    public float bigPlanetSizeMultiplier = 2f;
    public float planetSizeVariance = 0.3f;
    public float minEdgeDistance = 1.5f;
    public AnimationCurve sizeReductionCurve;
    public AnimationCurve distanceIncreaseCurve;

    [Header("Stage Settings")]
    public float stageHeight = 100f;
    public float triggerOffset = 30f;
    private float nextStageY;

    [Header("Obstacle Settings")]
    public int safeStartHeight = 50;
    public int blackHoleStartHeight = 150;
    public int spikeStartHeight = 100;
    public int badPlanetStartHeight = 200;
    public float minSpecialObjectDistance = 15f;

    [Header("Density Controls")]
    public float maxPlanetDensity = 0.3f;
    public float maxSpikeDensity = 0.4f;
    public float minObjectSpacing = 2.5f;

    [Header("Item Distribution")]
    [Range(0f, 1f)] public float spikeProbability = 0.4f;
    [Range(0f, 1f)] public float blackHoleProbability = 0.2f;
    [Range(0f, 1f)] public float badPlanetProbability = 0.2f;
    public int powerupsPerStage = 3;

    [Header("Zone Settings")]
    public int zoneStartHeight = 200;
    [Range(0f, 1f)] public float zoneSpawnChance = 0.5f;
    public float minVerticalZoneDistance = 10f;
    public float minHorizontalZoneDistance = 5f;
    public int zonePlacementAttempts = 5;
    public int maxZonesPerStage = 2;
    public float zoneSizeVariance = 0.3f;
    public float minZoneSize = 1.5f;
    public float maxZoneSize = 3f;

    private List<GameObject> activeObjects = new List<GameObject>();
    private List<Vector2> planetPositions = new List<Vector2>();
    private List<Vector2> specialObjectPositions = new List<Vector2>();
    private List<Vector2> spikePositions = new List<Vector2>();
    private List<Vector2> zonePositions = new List<Vector2>();
    private float currentDifficulty;

    void Start()
    {
        ball = GameObject.FindGameObjectWithTag("ball").transform;
        mainCamera = Camera.main;
        CalculateScreenBounds();
        CreateFixedStartPlanets();
        nextStageY = stageHeight;
        nextTwinGateY = twinGateStartHeight;
    }

    void Update()
    {
        if (ball.position.y > nextStageY - triggerOffset)
        {
            GenerateNewStage();
            nextStageY += stageHeight;
            CleanupOldObjects();
        }
    }

    void CalculateScreenBounds()
    {
        float screenAspect = (float)Screen.width / Screen.height;
        float cameraHeight = mainCamera.orthographicSize * 2;
        float cameraWidth = cameraHeight * screenAspect;

        leftBound = mainCamera.transform.position.x - (cameraWidth * horizontalMargin / 2);
        rightBound = mainCamera.transform.position.x + (cameraWidth * horizontalMargin / 2);
    }

    void CreateFixedStartPlanets()
    {
        float[] fixedHeights = { 5f, 10f, 15f, 20f, 25f, 30f, 35f, 40f, 45f, 50f };
        float[] fixedSizes = { 4.5f, 4f, 6f, 5f, 4.3f, 2.7f, 3.6f, 5.3f, 3.8f, 3f };
        float[] fixedXPositions = {
            leftBound + 3f, rightBound - 3f,
            leftBound + 2f, rightBound - 2f,
            leftBound + 3.5f, rightBound - 3.5f,
            leftBound + 2.5f, rightBound - 2.5f,
            (leftBound + rightBound)/2, (leftBound + rightBound)/2
        };

        for (int i = 0; i < 10; i++)
        {
            Vector2 pos = new Vector2(fixedXPositions[i], fixedHeights[i]);
            CreatePlanet(pos, fixedSizes[i]);
        }

        float baseHeight = 55f;
        int planetCount = 8;
        float verticalSpread = 5f;

        for (int i = 0; i < planetCount; i++)
        {

            float size = 3f * (1 - (i * 0.02f));

            float xPos = Random.Range(leftBound, rightBound);
            float yPos = baseHeight + (i * verticalSpread) + Random.Range(-1f, 1f);


            Vector2 pos = new Vector2(xPos, yPos);
            CreatePlanet(pos, size);
        }
    }

    void GenerateNewStage()
    {
        planetPositions.Clear();
        spikePositions.Clear();
        currentDifficulty = Mathf.Clamp01((ball.position.y - safeStartHeight) / 1000f);


        if (nextStageY % 500 == 0)
        {
            if (ball.transform.position.y < 220)
            {
                StartCoroutine(FadeOut());
            }

            GameObject banner = Instantiate(distance_banner, new Vector3(0, nextStageY, -10), Quaternion.identity);
            banner.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = (nextStageY.ToString() + "m");
            last_distance_banner_height = (int)banner.transform.position.y;
            // SpawnDangerSafeZonePairsInStage(1);

        }


        PlacePlanets();
        FillGapsBetweenPlanets();
        PlacePowerups();

        if (ball.position.y > zoneStartHeight &&
            Random.value < zoneSpawnChance &&
            zonePositions.Count < maxZonesPerStage * 3)
        {
            PlaceZones();
        }

        if (specialObjectPositions.Count > 10)
        {
            specialObjectPositions.RemoveRange(0, specialObjectPositions.Count - 5);
        }
        if (ball.position.y > twinGateStartHeight && ball.position.y + stageHeight > nextTwinGateY)
        {
            TrySpawnTwinGate();
        }
    }
    bool IsLaserGunNear(float y, float range)
    {
        
        foreach (GameObject obj in activeObjects)
        {
            if (obj == null) continue;
            if (obj.CompareTag("laser_gun")) // Make sure your laser gun prefab has this tag
            {
                if (Mathf.Abs(obj.transform.position.y - y) < range)
                    return true;
                Debug.Log("twin_removed");

            }
        }
        
        return false;
    }
    void TrySpawnTwinGate()
    {
        float spawnY = nextTwinGateY + Random.Range(100f, 130f);
        Debug.Log("twin_made");
        // Check laser guns nearby (edges of the screen)
        if (IsLaserGunNear(spawnY, 6f)) return;

        // Decide gate side
        bool leftFirst = Random.value < 0.5f;

        // Create parent
        GameObject twinGate = Instantiate(twinGatePrefab, new Vector3(0,nextStageY,0), Quaternion.identity);
        activeObjects.Add(twinGate);

        // Position children
        Transform gateA = twinGate.transform.GetChild(0);
        Transform gateB = twinGate.transform.GetChild(1);

        float leftX = game_manager_scr.left_bar_x-0.8f;
        float rightX = game_manager_scr.right_bar_x+0.8f;

        if (leftFirst)
        {
            gateA.position = new Vector3(leftX, spawnY, 0f);
            gateB.position = new Vector3(rightX, spawnY + 6f, 0f);
        }
        else
        {
            gateA.position = new Vector3(rightX, spawnY, 0f);
            gateB.position = new Vector3(leftX, spawnY + 6f, 0f);
        }

        // Schedule next spawn in 200–300 range
        nextTwinGateY += Random.Range(twinGateSpawnInterval.x, twinGateSpawnInterval.y);
    }

    void PlaceZones()
    {
        int zonesPlacedThisStage = 0;
        int attempts = 0;

        while (zonesPlacedThisStage < maxZonesPerStage && attempts < zonePlacementAttempts)
        {
            attempts++;

            float yPos = nextStageY + Random.Range(20f, stageHeight - 20f);

            bool validYPosition = true;
            foreach (Vector2 existingZone in zonePositions)
            {
                if (Mathf.Abs(existingZone.y - yPos) < minVerticalZoneDistance)
                {
                    validYPosition = false;
                    break;
                }
            }

            if (!validYPosition) continue;

            float blueX = Random.Range(leftBound + 2f, rightBound - 2f);
            Vector2 bluePos = new Vector2(blueX, yPos + Random.RandomRange(5, 10));

            float redYOffset = Random.Range(minVerticalZoneDistance * 0.3f, minVerticalZoneDistance * 0.7f);
            float redX = (blueX < (leftBound + rightBound) / 2) ?
                Random.Range((leftBound + rightBound) / 2 + minHorizontalZoneDistance, rightBound - 2f) :
                Random.Range(leftBound + 2f, (leftBound + rightBound) / 2 - minHorizontalZoneDistance);
            Vector2 redPos = new Vector2(redX, yPos + redYOffset - Random.RandomRange(5, 10));

            if (IsZonePositionValid(bluePos) && IsZonePositionValid(redPos))
            {
                float blueSize = Random.Range(minZoneSize, maxZoneSize) *
                               (1 + Random.Range(-zoneSizeVariance, zoneSizeVariance));
                float redSize = Random.Range(minZoneSize, maxZoneSize) *
                              (1 + Random.Range(-zoneSizeVariance, zoneSizeVariance));

                CreateBlueZone(bluePos, blueSize);
                CreateRedZone(redPos, redSize);

                zonePositions.Add(bluePos);
                zonePositions.Add(redPos);
                zonesPlacedThisStage++;
            }
        }
    }

    bool IsZonePositionValid(Vector2 pos)
    {
        foreach (Vector2 zonePos in zonePositions)
        {
            if (Vector2.Distance(pos, zonePos) < minVerticalZoneDistance)
            {
                return false;
            }
        }

        foreach (Vector2 specialPos in specialObjectPositions)
        {
            if (Vector2.Distance(pos, specialPos) < minSpecialObjectDistance * 0.7f)
            {
                return false;
            }
        }

        return true;
    }

    void CreateBlueZone(Vector2 pos, float size)
    {
        CreateEdgeLaserGun(pos.y);
        /*
        GameObject zone = Instantiate(blueZonePrefab, pos, Quaternion.identity);
        zone.transform.localScale = zone.transform.localScale * Random.RandomRange(0.7f, 1.5f);
        activeObjects.Add(zone);
        */
    }

    void CreateRedZone(Vector2 pos, float size)
    {
        CreateEdgeLaserGun(pos.y);

        GameObject zone = Instantiate(redZonePrefab, pos, Quaternion.identity);
        zone.transform.localScale = zone.transform.localScale * Random.RandomRange(0.7f, 1.5f);
        activeObjects.Add(zone);
    }

    private void PlaceRandomObjectAtHeight(float yPos)
    {
        if (GetObjectDensity(yPos, 10f) > maxPlanetDensity * 10f) return;

        List<float> xPositions = new List<float>();
        for (float x = leftBound + 2f; x <= rightBound - 2f; x += 2f)
        {
            xPositions.Add(x);
        }

        for (int i = 0; i < xPositions.Count; i++)
        {
            int randomIndex = Random.Range(i, xPositions.Count);
            float temp = xPositions[i];
            xPositions[i] = xPositions[randomIndex];
            xPositions[randomIndex] = temp;
        }

        int placed = 0;
        foreach (float x in xPositions)
        {
            if (placed >= 1) break;

            Vector2 position = new Vector2(x, yPos);

            if (!IsPositionSafe(position, minObjectSpacing)) continue;

            float rand = Random.value;
            bool canPlaceSpecial = ball.position.y > blackHoleStartHeight &&
                                 IsSpecialObjectPositionSafe(position) &&
                                 Random.value < 0.3f;

            if (canPlaceSpecial)
            {
                if (ball.position.y > badPlanetStartHeight && rand < badPlanetProbability)
                {
                    CreateBadPlanet(position, Random.Range(1.2f, 1.8f));
                    specialObjectPositions.Add(position);
                    placed++;
                }
                else if (rand < blackHoleProbability)
                {
                    CreateBlackHole(position);
                    specialObjectPositions.Add(position);
                    placed++;
                }
            }

            else if (ball.position.y < spikeStartHeight)
            {
                if (rand < spikeProbability && GetSpikeDensity(yPos, 5f) < maxSpikeDensity * 5f)
                {
                    CreateSpike(position);
                    spikePositions.Add(position);
                    placed++;
                }
            }

        }
    }

    float GetObjectDensity(float yPos, float range)
    {
        float count = 0;
        foreach (var obj in activeObjects)
        {
            if (obj != null && Mathf.Abs(obj.transform.position.y - yPos) < range)
            {
                count++;
            }
        }
        return count / range;
    }

    float GetSpikeDensity(float yPos, float range)
    {
        float count = 0;
        foreach (var pos in spikePositions)
        {
            if (Mathf.Abs(pos.y - yPos) < range)
            {
                count++;
            }
        }
        return count / range;
    }

    void PlacePlanets()
    {
        float currentY = nextStageY;
        float stageTop = nextStageY + stageHeight;
        int maxPlanets = Mathf.FloorToInt(stageHeight * maxPlanetDensity);

        for (int i = 0; i < maxPlanets && currentY < stageTop; i++)
        {
            bool isBigPlanet = Random.value < bigPlanetChance;
            float sizeBase = isBigPlanet ?
                initialPlanetSize * bigPlanetSizeMultiplier :
                Mathf.Lerp(initialPlanetSize, minPlanetSize, sizeReductionCurve.Evaluate(currentDifficulty));

            float size = sizeBase * (1 + Random.Range(-planetSizeVariance, planetSizeVariance * 0.8f));
            float distance = Random.Range(minPlanetDistance, maxPlanetDistance);

            Vector2 position = GetSafePlanetPosition(currentY, size, distance);

            if (position != Vector2.zero)
            {
                if (position.x < leftBound + size + minEdgeDistance ||
                       position.x > rightBound - size - minEdgeDistance)
                {
                    position.x = Mathf.Clamp(
                        position.x,
                        leftBound + size * 0.55f + minEdgeDistance,
                        rightBound - size * 0.55f - minEdgeDistance
                    );
                }

                if (isBigPlanet)
                {

                }

                /*
                if (ball.position.y > badPlanetStartHeight &&
                    Random.value < badPlanetProbability * 0.5f &&
                    IsSpecialObjectPositionSafe(position))
                {
                    CreateBadPlanet(position, size * 1.3f);
                    specialObjectPositions.Add(position);
                }
                */
                //else
                {
                    CreatePlanet(position, size);
                }
                planetPositions.Add(position);
                currentY = position.y + distance;
            }
            else
            {
                currentY += distance * 0.5f;
            }
        }
    }

    Vector2 GetSafePlanetPosition(float yPos, float size, float minDistance)
    {
        int attempts = 0;
        Vector2 position = Vector2.zero;

        while (attempts < 100)
        {
            float xPos;
            if (Random.value < 0.7f)
            {
                float centerBias = Random.Range(-1f, 1f) * (rightBound - leftBound) * 0.3f;
                xPos = (leftBound + rightBound) / 2 + centerBias;
            }
            else
            {
                xPos = Random.Range(
                    leftBound + size + minEdgeDistance,
                    rightBound - size - minEdgeDistance
                );
            }

            position = new Vector2(xPos, yPos);

            if (IsPositionSafe(position, size + minDistance * 0.7f))
            {
                return position;
            }
            attempts++;
        }
        return Vector2.zero;
    }

    private void FillGapsBetweenPlanets()
    {
        // 1) Planet-related coin trails
        if (planetPositions.Count >= 2)
        {
            planetPositions.Sort((a, b) => a.y.CompareTo(b.y));

            for (int i = 0; i < planetPositions.Count - 1; i++)
            {
                Vector2 bottomPlanet = planetPositions[i];
                Vector2 topPlanet = planetPositions[i + 1];
                float gapHeight = topPlanet.y - bottomPlanet.y;

                if (gapHeight > minPlanetDistance * 1f)
                {
                    int objectsToPlace = Mathf.Clamp(Mathf.FloorToInt(gapHeight / 8f), 1, 3);
                    for (int j = 1; j <= objectsToPlace; j++)
                    {
                        float yPos = bottomPlanet.y + (gapHeight * j / (objectsToPlace + 1));
                        PlaceRandomObjectAtHeight(yPos);
                    }

                    if (coinPrefab != null && Random.value < coinTrailChance)
                    if (coinPrefab != null )
                    {
                        bool curved = allowCurvedTrails && Random.value > 0.5f;
                        CreateCoinTrail(
                            bottomPlanet + Vector2.up * planetSafeOffset,
                            topPlanet - Vector2.up * planetSafeOffset,
                            curved
                        );
                        Debug.Log("planet_coin");
                    }
                }
            }
        }

        // 2) Free-floating coin trails (not related to planets)
        if (coinPrefab != null && Random.value < freeCoinPathChance)
        {
            float yStart = nextStageY + Random.Range(10f, stageHeight - 10f);
            Vector2 startPos = new Vector2(
                Random.Range(leftBound + 1f, rightBound - 1f),
                yStart
            );
            Vector2 endPos = startPos + new Vector2(
                Random.Range(-3f, 3f),
                Random.Range(4f, 8f)
            );

           // bool curved = allowCurvedTrails && Random.value > 0.5f;
            bool curved = allowCurvedTrails && Random.value > 0.5f;
            CreateCoinTrail(startPos, endPos, curved);
            Debug.Log("free_coin");

        }
    }


    void PlacePowerups()
    {
        for (int i = 0; i < powerupsPerStage; i++)
        {

            if (Random.RandomRange(0f, 1f) > 0.1f)
            {
                float yPos = nextStageY + Random.Range(10f, stageHeight - 10f);
                float xPos = Random.Range(leftBound + 1f, rightBound - 1f);
                Vector2 pos = new Vector2(xPos, yPos);

                if (IsPositionSafe(pos, 2f))
                {
                    int type = Random.Range(1, 1);
                    switch (type)
                    {
                        case 0: CreateBattery(pos); break;
                        // case 1: CreateMagnet(pos); break;
                        case 1: CreateShield(pos); break;
                    }
                }
            }

        }
    }

    bool IsSpecialObjectPositionSafe(Vector2 pos)
    {
        foreach (Vector2 specialPos in specialObjectPositions)
        {
            if (Vector2.Distance(pos, specialPos) < minSpecialObjectDistance)
            {
                return false;
            }
        }
        return true;
    }

    void CreatePlanet(Vector2 pos, float size)
    {


        GameObject planet = Instantiate(planetPrefab, pos, Quaternion.identity);
        planet.transform.localScale = Vector3.one * size;
        activeObjects.Add(planet);

        // if (ball.transform.position.y > 400)
        if (ball.transform.position.y > 500)
        {
            if (Random.RandomRange(0f, 1f) > 0.7f)
            {
                planet.GetComponent<planet_attribute_scr>().moving_planet = true;
            }
            else
            {
                planet.GetComponent<planet_attribute_scr>().moving_planet = false;

            }








        }

        if (ball.transform.position.y > 1000)
        {

            if (Random.RandomRange(0f, 1f) > 0.7f)


            {

                planet.GetComponent<planet_attribute_scr>().is_bomber = true;

                planet.transform.GetChild(0).GetChild(2).gameObject.SetActive(false);
                planet.transform.GetChild(0).GetChild(4).gameObject.SetActive(true);


            }
            else
            {

                planet.GetComponent<planet_attribute_scr>().is_bomber = false;



            }
        }




    }

    void CreateBadPlanet(Vector2 pos, float size)
    {







        if (Random.RandomRange(0f, 1f) > 0.75f)
        {



            ////////////////////////  beam_part


            Vector2 black_beam_pos = new Vector3(Random.RandomRange(game_manager_scr.left_bar_x + 2, game_manager_scr.right_bar_x - 2), pos.y + Random.RandomRange(-4, 4));

            Debug.Log("beam_made");
            GameObject maded_beam_emitter = Instantiate(beam_emitter, black_beam_pos, Quaternion.identity);
            maded_beam_emitter.transform.Rotate(0, 0, Random.Range(-30, 30));
            activeObjects.Add(maded_beam_emitter);


            ////////////////////////  alien_part
            Vector3 the_pos = new Vector3(0, pos.y, -15);
            GameObject planet = Instantiate(badPlanetPrefab, the_pos, Quaternion.identity);
            // planet.transform.localScale = Vector3.one * size;
            activeObjects.Add(planet);


            planet.GetComponent<alien_wandering_scr>().yDuration = Random.RandomRange(2, 5);
            planet.GetComponent<alien_wandering_scr>().yRange = Random.RandomRange(0.5f, 4);
            planet.GetComponent<alien_wandering_scr>().xDuration = Random.RandomRange(4, 8);

        }
        else
        {
            Vector3 the_pos = new Vector3(0, pos.y, -15);
            GameObject planet = Instantiate(badPlanetPrefab, the_pos, Quaternion.identity);
            // planet.transform.localScale = Vector3.one * size;
            activeObjects.Add(planet);


            planet.GetComponent<alien_wandering_scr>().yDuration = Random.RandomRange(2, 5);
            planet.GetComponent<alien_wandering_scr>().yRange = Random.RandomRange(0.5f, 4);
            planet.GetComponent<alien_wandering_scr>().xDuration = Random.RandomRange(4, 8);
        }

    }

    void CreateSpike(Vector2 pos)
    {


        if (Random.RandomRange(0f, 1f) > 0.75f)
        {

            Vector2 spike_beam_pos = new Vector2(Random.RandomRange(game_manager_scr.left_bar_x + 2, game_manager_scr.right_bar_x - 2), pos.y);


            //  Debug.Log("beam_made");
            GameObject maded_beam_emitter = Instantiate(beam_emitter, spike_beam_pos, Quaternion.identity);
            maded_beam_emitter.transform.Rotate(0, 0, Random.Range(-30, 30));
            activeObjects.Add(maded_beam_emitter);
        }
        else
        {
            //  Debug.Log("spike_made");
            GameObject spike = Instantiate(spikePrefab, pos, Quaternion.identity);
            spike.transform.Rotate(0, 0, Random.Range(0, 360));
            activeObjects.Add(spike);
        }




    }

    void CreateBlackHole(Vector2 pos)
    {

        if (Random.RandomRange(0f, 1f) > 0.6f)
        {



            ////////////////////////  beam_part

            Vector2 black_beam_pos = new Vector3(Random.RandomRange(game_manager_scr.left_bar_x + 2, game_manager_scr.right_bar_x - 2), pos.y + Random.RandomRange(1, 3));


            Debug.Log("beam_made  some problem_detected");
            GameObject maded_beam_emitter = Instantiate(beam_emitter, black_beam_pos, Quaternion.identity);
            maded_beam_emitter.transform.Rotate(0, 0, Random.Range(-30, 30));
            activeObjects.Add(maded_beam_emitter);


            ////////////////////////  black_hole_part


            GameObject bh = Instantiate(blackHolePrefab, pos, Quaternion.identity);

            bh.transform.localScale *= (currentDifficulty + 2) * Random.RandomRange(0.4f, 0.8f);
            activeObjects.Add(bh);

        }
        else
        {
            GameObject bh = Instantiate(blackHolePrefab, pos, Quaternion.identity);

            bh.transform.localScale *= (currentDifficulty + 2) * Random.RandomRange(0.4f, 0.8f);
            activeObjects.Add(bh);
        }


    }

    void CreateMagnet(Vector2 pos)
    {
        GameObject magnet = Instantiate(magnetPrefab, pos, Quaternion.identity);
        activeObjects.Add(magnet);
    }

    void CreateBattery(Vector2 pos)
    {
        GameObject battery = Instantiate(batteryPrefab, pos, Quaternion.identity);
        activeObjects.Add(battery);
    }

    void CreateShield(Vector2 pos)
    {
        GameObject shield = Instantiate(shieldPrefab, pos, Quaternion.identity);
        activeObjects.Add(shield);
    }

    bool IsPositionSafe(Vector2 pos, float radius)
    {
        foreach (var obj in activeObjects)
        {
            if (obj != null && Vector2.Distance(pos, obj.transform.position) < radius)
            {
                return false;
            }
        }
        return true;
    }

    void CleanupOldObjects()
    {
        for (int i = activeObjects.Count - 1; i >= 0; i--)
        {
            if (activeObjects[i] != null &&
                activeObjects[i].transform.position.y < ball.position.y - stageHeight)
            {
                Destroy(activeObjects[i]);
                activeObjects.RemoveAt(i);
            }
        }

        for (int i = specialObjectPositions.Count - 1; i >= 0; i--)
        {
            if (specialObjectPositions[i].y < ball.position.y - stageHeight)
            {
                specialObjectPositions.RemoveAt(i);
            }
        }

        for (int i = spikePositions.Count - 1; i >= 0; i--)
        {
            if (spikePositions[i].y < ball.position.y - stageHeight)
            {
                spikePositions.RemoveAt(i);
            }
        }

        for (int i = zonePositions.Count - 1; i >= 0; i--)
        {
            if (zonePositions[i].y < ball.position.y - stageHeight * 1.5f)
            {
                zonePositions.RemoveAt(i);
            }
        }
    }
    void CreateEdgeLaserGun(float yHeight)
    {
        bool leftSide = Random.value < 0.5f;

        float the_y;


        if (leftSide)
        {

            Vector3 gunPos = new Vector3(-8, yHeight, 0);


            GameObject gun = Instantiate(laser_gun_prefabs, gunPos, Quaternion.Euler(0, 0, 0));
            gun.GetComponent<laser_gun_scr>().is_left = true;
            activeObjects.Add(gun);
        }

        if (!leftSide)
        {


            Vector3 gunPos = new Vector3(8, yHeight, 0);


            GameObject gun = Instantiate(laser_gun_prefabs, gunPos, Quaternion.Euler(0, 180, 0));

            gun.GetComponent<laser_gun_scr>().is_left = false;


            activeObjects.Add(gun);

        }


    }
    void CreateCoinTrail(Vector2 start, Vector2 end, bool curved)
    {
        if (coinPrefab == null) return;

        List<Vector2> points = new List<Vector2>();
        float distAcc = 0f;
        int resolution = 50;

        // Direction vectors
        Vector2 dir = (end - start).normalized;
        Vector2 perp = new Vector2(-dir.y, dir.x);
        float side = (Random.value < 0.5f) ? 1f : -1f;

        // Curve height clamp
        float verticalDiff = Mathf.Abs(end.y - start.y);
        float curveY = Mathf.Clamp(verticalDiff, minCurveHeight, maxCurveHeight);

        // Main control point
        Vector2 control = (start + end) / 2;
        control.y += curveY;

        // Avoid nearest obstacle
        Vector2 nearestObstacle = FindNearestObstacle(control);
        if (nearestObstacle != Vector2.zero)
        {
            Vector2 dirFromObstacle = (control - nearestObstacle).normalized;
            control += dirFromObstacle * obstacleAvoidOffset;
        }

        // Twist point for nice trail shape
        Vector2 twistPoint = start + dir * 4f + perp * side * 2f;

        // Path samples
        List<Vector2> pathSamples = new List<Vector2>();
        if (curved)
        {
            for (int i = 0; i <= resolution; i++)
            {
                float t = i / (float)resolution;

                // Bezier curve points
                Vector2 p0p1 = Vector2.Lerp(start, twistPoint, t);
                Vector2 p1p2 = Vector2.Lerp(twistPoint, control, t);
                Vector2 p2p3 = Vector2.Lerp(control, end, t);

                Vector2 mid1 = Vector2.Lerp(p0p1, p1p2, t);
                Vector2 mid2 = Vector2.Lerp(p1p2, p2p3, t);

                Vector2 pos = Vector2.Lerp(mid1, mid2, t);
                pathSamples.Add(pos);
            }
        }
        else
        {
            for (int i = 0; i <= resolution; i++)
            {
                float t = i / (float)resolution;
                pathSamples.Add(Vector2.Lerp(start, end, t));
            }
        }

        // Constant spacing selection
        points.Add(pathSamples[0]);
        for (int i = 1; i < pathSamples.Count; i++)
        {
            float segDist = Vector2.Distance(pathSamples[i - 1], pathSamples[i]);
            distAcc += segDist;
            if (distAcc >= coinSpacing)
            {
                points.Add(pathSamples[i]);
                distAcc = 0f;
            }
        }

        // Force at least one spawn point in the middle if all else fails
        if (points.Count == 0)
            points.Add((start + end) * 0.5f);

        // --- New Obstacle Handling ---
        int spawned = 0;
        foreach (Vector2 p in points)
        {
            Vector2 spawnPos = p;

            // If blocked, find nearest free position within small search radius
            if (IsPositionBlocked(p))
            {
                Vector2? freeSpot = FindNearestFreePosition(p, 1.5f, 6); // radius = 1.5, 6 tries
                if (freeSpot.HasValue)
                    spawnPos = freeSpot.Value;
                else
                    continue; // skip if no free spot found
            }

           GameObject coin =  Instantiate(coinPrefab, spawnPos, Quaternion.identity);
            activeObjects.Add(coin);
            spawned++;
        }
    }

    // Checks if point is too close to planets/obstacles
    bool IsPositionBlocked(Vector2 pos)
    {
        foreach (GameObject obj in activeObjects)
        {
            if (obj == null || obj.CompareTag("coin")) continue;
            if (Vector2.Distance(pos, obj.transform.position) < planetSafeOffset)
                return true;
        }
        return false;
    }

    // Finds free spot nearby where a coin can be placed
    Vector2? FindNearestFreePosition(Vector2 from, float searchRadius, int attempts)
    {
        for (int i = 0; i < attempts; i++)
        {
            Vector2 candidate = from + Random.insideUnitCircle * searchRadius;

            if (!IsPositionBlocked(candidate))
                return candidate;
        }
        return null;
    }


    Vector2 FindNearestObstacle(Vector2 fromPos)
    {
        Vector2 nearest = Vector2.zero;
        float closestDist = Mathf.Infinity;

        foreach (GameObject obj in activeObjects)
        {
            if (obj == null) continue;

            Vector2 objPos = obj.transform.position;
            float dist = Vector2.Distance(fromPos, objPos);

            // Skip if it's actually a coin prefab (so we don't avoid our own coins)
            if (obj.CompareTag("coin")) continue;

            if (dist < closestDist)
            {
                closestDist = dist;
                nearest = objPos;
            }
        }

        // Return only if something is within avoidance radius
        return (closestDist < 10f) ? nearest : Vector2.zero;
    }


    private System.Collections.IEnumerator FadeOut()
    {
        SpriteRenderer sr = sprite_cariaer.GetComponent<SpriteRenderer>();
        if (sr == null)
            yield break;

        Color originalColor = sr.color;
        float startAlpha = originalColor.a;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / fadeDuration);
            Color c = originalColor;
            c.a = Mathf.Lerp(startAlpha, 0f, t);
            sr.color = c;
            yield return null;
        }

        originalColor.a = 0f;
        sr.color = originalColor;
    }


    void SpawnDangerSafeZonePairsInStage(int pairsPerStage = 1)
    {
        for (int i = 0; i < 1; i++)
        {

            int start = Random.RandomRange(100, 200) + last_distance_banner_height;
            int end = start + Random.RandomRange(50, 200);
            // Place danger zone
            int dangerY = start; // leave space for safe zone above

            game_manager_scr.next_danger_zone_height = dangerY;
            game_manager_scr.check_for_danger_zone = true;
            float x_danger = 0;
            Vector2 posDanger = new Vector2(x_danger, dangerY);


            GameObject dz = Instantiate(dangerZonePrefab, posDanger, Quaternion.identity);
            activeObjects.Add(dz);

            // Place its safe zone immediately after (higher Y, min +10 separation)
            float safeY = end;
            float x_safe = 0;
            Vector2 posSafe = new Vector2(x_safe, safeY);


            GameObject sz = Instantiate(safeZonePrefab, posSafe, Quaternion.identity);
            activeObjects.Add(sz);


        }
    }
}
