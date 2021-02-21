using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Chunk : MonoBehaviour
{
    // Basic chunk properties
    private int chunkID;
    private Point2D chunkCoords; // top left
    private int sideLength;
    private int perlinValue;
    private float groundHeight;
    private static int perlinResolution = 15;
    private static float groundHeightStep = 1f;
    private static int minBuildingSize = 5;
    private static int maxBuildingSize = 9;

    // Probability of a building having a window plan type
    private static float noWindowsProb = 0.15f;
    private static float fullWindowsProb = 0.3f;
    private static float alternatingWindowsProb = 0.65f;
    private static float centeredWindowsProb = 1.0f;

    public GameObject groundPrefab;
    public GameObject cubePrefab;
    public GameObject buildingPrefab;

    public Material lowMaterial;
    public Material highMaterial;

    public Texture redRC;
    public Texture blueRC;
    public Texture greenRC;
    public Texture whiteRC;
    public Texture yellowRC;
    public Texture orangeRC;
    public Texture cyanRC;
    public Texture magentaRC;
    public static int numTextures = 8;

    private GameObject ground;

    private Transform playerTransform;

    // Keep track of buildings
    private List<BuildingInfo> footprints = new List<BuildingInfo>();
    private List<GameObject> buildings = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void EnableChunk()
    {
        ground.SetActive(true);
        for(int i = 0; i < buildings.Count; i++)
        {
            buildings[i].GetComponent<Building>().EnableRendering();
        }
    }

    public void DisableChunk()
    {
        ground.SetActive(false);
        for (int i = 0; i < buildings.Count; i++)
        {
            buildings[i].GetComponent<Building>().DisableRendering();
        }
    }

    // Setters
    public void InitializeGround()
    {
        ground = Instantiate(groundPrefab);
        ground.transform.position = transform.position + Vector3.up*groundHeight/2;
        ground.transform.localScale = new Vector3(sideLength, groundHeight, sideLength);
        if(groundHeight < perlinResolution*groundHeightStep / 2)
        {
            ground.GetComponent<Renderer>().material = lowMaterial;
        }
        else
        {
            ground.GetComponent<Renderer>().material = highMaterial;
        }
    }
    public void SetChunkID(int inputID)
    {
        chunkID = inputID;
        chunkCoords = ChunkManager.chunkIDtoPoint2D(chunkID);
    }
    public void SetSideLength(int inputSideLength)
    {
        sideLength = inputSideLength;
        transform.position = new Vector3(sideLength * chunkCoords.x + sideLength / 2.0f, 0f, sideLength * chunkCoords.z + sideLength / 2.0f);
    }
    public void SetPlayerTransform(Transform input)
    {
        playerTransform = input;
    }
    public void SetPerlinValue(float inputPerlinValue)
    {
        perlinValue = (int)Mathf.Floor(perlinResolution*inputPerlinValue + 1);
        perlinValue = Mathf.Max(perlinValue, 1);
        this.ComputeGroundHeight();
    }
    public void ComputeGroundHeight()
    {
        groundHeight = perlinValue * groundHeightStep;
    }

    // Returns a random point on the plane of this chunk, that is not within buffer of the border
    private Vector3 getRandomPoint(float buffer)
    {
        float randomX = Random.Range(-sideLength / 2 + buffer, sideLength / 2 - buffer);
        float randomZ = Random.Range(-sideLength / 2 + buffer, sideLength / 2 - buffer);
        return transform.position + new Vector3(randomX, 0f, randomZ);
    }

    public void AttemptToGenerateBuilding(int numAttempts)
    {
        int i = 0;
        while (i < numAttempts)
        {
            i++;
            int randXDim = Mathf.FloorToInt(Random.Range(minBuildingSize, maxBuildingSize + 1));
            int randZDim = Mathf.FloorToInt(Random.Range(minBuildingSize, maxBuildingSize + 1));

            int bottomLeftX = Mathf.FloorToInt(Random.Range(0, sideLength - randXDim));
            int bottomLeftZ = Mathf.FloorToInt(Random.Range(0, sideLength - randZDim));
            Point2D potentialCorner = new Point2D(bottomLeftX, bottomLeftZ);

            if(!OverlapsExistingFootprint(potentialCorner, randXDim, randZDim))
            {
                BuildingInfo info = new BuildingInfo(potentialCorner, randXDim, randZDim);
                //Vector3 bottomLeft = GetWorldCoordinates(potentialCorner);
                Vector3 centerBase = CalculateBuildingCenterBase(potentialCorner, randXDim, randZDim);
                CreateBuilding(info, centerBase);
            }
        }
    }

    private bool FootprintsOverlap(Point2D corner1, int w1, int h1, Point2D corner2, int w2, int h2)
    {
        bool disjointX = corner2.x > corner1.x + w1 || corner1.x > corner2.x + w2;
        bool disjointZ = corner2.z > corner1.z + h1 || corner1.z > corner2.z + h2;
        return !(disjointX || disjointZ);
    }
    // Wrapper
    private bool OverlapsBuildingInfo(BuildingInfo existing, Point2D corner, int w, int h)
    {
        return FootprintsOverlap(existing.bottomLeft, existing.xWidth, existing.zWidth, corner, w, h);
    }

    private bool OverlapsExistingFootprint(Point2D corner, int w, int h)
    {
        for(int i = 0; i < footprints.Count; i++)
        {
            if(OverlapsBuildingInfo(footprints[i], corner, w, h))
            {
                return true;
            }
        }
        return false;
    }

    // Given that p is an index of a square in this chunk, find the actual world
    // coordinates of the bottom left of the square
    public Vector3 GetWorldCoordinates(Point2D p)
    {
        float x = chunkCoords.x * sideLength + p.x;
        float z = chunkCoords.z * sideLength + p.z;
        return new Vector3(x, groundHeight, z);
    }
    public Vector3 CalculateBuildingCenterBase(Point2D potentialCorner, int randXDim, int randZDim)
    {
        float x = chunkCoords.x * sideLength + potentialCorner.x + randXDim / 2f;
        float z = chunkCoords.z * sideLength + potentialCorner.z + randZDim / 2f;
        return new Vector3(x, groundHeight, z);
    }

    private void CreateBuilding(BuildingInfo info, Vector3 centerBase)
    {
        GameObject building = Instantiate(buildingPrefab);
        building.GetComponent<Building>().SetFootprint(info);
        building.GetComponent<Building>().SetTrueCenterBase(centerBase);
        building.GetComponent<Building>().SetHeightPerFloor(ChooseRandomHeightPerFloor());
        building.GetComponent<Building>().SetNumFloors(ChooseRandomNumFloors());
        building.GetComponent<Building>().SetDoorHeight(3);
        building.GetComponent<Building>().CalculateTotalHeight();
        building.GetComponent<Building>().SetWindowPlan(ChooseRandomWindowPlan());
        building.GetComponent<Building>().SetTexture(ChooseRandomTexture());
        building.transform.position = centerBase;
        building.GetComponent<Building>().CreateVoxels();
        footprints.Add(info);
        buildings.Add(building);
    }

    private WindowPlan ChooseRandomWindowPlan()
    {
        float rand = Random.Range(0, 1000) / 1000f;
        if(rand < noWindowsProb)
        {
            return WindowPlan.NoWindows;
        }
        else if(rand < fullWindowsProb)
        {
            return WindowPlan.Full;
        }
        else if(rand < alternatingWindowsProb)
        {
            return WindowPlan.Alternating;
        }
        else
        {
            return WindowPlan.Centered;
        }
    }

    private int ChooseRandomHeightPerFloor()
    {
        return Random.Range(4, 7);
    }

    private int ChooseRandomNumFloors()
    {
        return Random.Range(1, 6);
    }

    private Texture ChooseRandomTexture()
    {
        float rand = Random.Range(0, 1000) / 1000f;
        if(rand < 1f / numTextures)
        {
            return redRC;
        }
        else if(rand < 2f / numTextures)
        {
            return blueRC;
        }
        else if(rand < 3f / numTextures)
        {
            return greenRC;
        }
        else if(rand < 4f / numTextures)
        {
            return yellowRC;
        }
        else if(rand < 5f / numTextures)
        {
            return whiteRC;
        }
        else if(rand < 6f / numTextures)
        {
            return orangeRC;
        }
        else if(rand < 7f / numTextures)
        {
            return cyanRC;
        }
        else
        {
            return magentaRC;
        }
    }

}

