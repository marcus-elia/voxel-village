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

    public GameObject groundPrefab;
    public Material lowMaterial;

    private GameObject ground;

    private Transform playerTransform;

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
    }

    public void DisableChunk()
    {
        ground.SetActive(false);
    }

    // Setters
    public void InitializeGround()
    {
        ground = Instantiate(groundPrefab);
        ground.transform.position = transform.position + Vector3.up*groundHeight/2;
        ground.transform.localScale = new Vector3(sideLength, groundHeight, sideLength);
        ground.GetComponent<Renderer>().material = lowMaterial;
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


}

