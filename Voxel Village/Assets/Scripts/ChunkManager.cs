using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct Point2D
{
    public int x;
    public int z;

    public Point2D(int x, int z)
    {
        this.x = x;
        this.z = z;
    }
}


public class ChunkManager : MonoBehaviour
{
    // Chunk-related variables
    public static int chunkSize = 20;
    public static float chunkHeightStep = 1f;
    public static int waterLevel = 3;
    public Transform playerTransform;
    public float playerHeight;
    private int currentPlayerChunkID;
    public int renderRadius = 2;
    private Dictionary<int, GameObject> allSeenChunks;
    private List<GameObject> currentChunks;

    public GameObject ChunkPrefab;

    // Perlin noise variables
    public static int seed = 2;
    private static int noiseScale = 2;
    private static int numOctaves = 3;

    public Slider renderRadiusSlider;


    // Start is called before the first frame update
    void Start()
    {
        //seed = (int)Time.time;
        int modulus = 65536;
        PerlinNoiseGenerator.SetA(modulus/2 + Mathf.FloorToInt(Random.Range(0, modulus/2)));
        PerlinNoiseGenerator.SetB(modulus/2 + Mathf.FloorToInt(Random.Range(0, modulus/2)));
        PerlinNoiseGenerator.SetC(Mathf.FloorToInt(Random.Range(0, modulus)));
        PerlinNoiseGenerator.SetModulus(modulus);

        currentPlayerChunkID = getChunkIDContainingPoint(playerTransform.position, chunkSize);
        allSeenChunks = new Dictionary<int, GameObject>();
        currentChunks = new List<GameObject>();
        updateChunks();

        // Move the player to be on the ground
        float playerY = allSeenChunks[0].GetComponent<Chunk>().GetGroundHeight() + playerHeight/2f;
        playerTransform.position = new Vector3(playerTransform.position.x, playerY, playerTransform.position.z);

    }

    // Update is called once per frame
    void Update()
    {
        // Update chunks if the player moved
        if (updateCurrentPlayerChunkID())
        {
            updateChunks();
        }
        // Update chunks if render radius changed
        int newRad = (int)renderRadiusSlider.value;
        if (newRad != renderRadius)
        {
            this.UpdateRenderRadius(newRad);
        }

    }

    // If the player enters a new chunk, return true and update the chunk id
    private bool updateCurrentPlayerChunkID()
    {
        int newChunkID = getChunkIDContainingPoint(playerTransform.position, chunkSize);
        if (newChunkID != currentPlayerChunkID)
        {
            currentPlayerChunkID = newChunkID;
            return true;
        }
        return false;
    }

    private void updateChunks()
    {
        // Disable previous chunks from drawing, and remove them from the arraylist
        for (int i = 0; i < currentChunks.Count; i++)
        {
            currentChunks[i].GetComponent<Chunk>().DisableChunk();
        }
        currentChunks = new List<GameObject>();

        // Go through the new chunks and add/create them
        List<int> chunkIDs = getChunkIDsAroundID(currentPlayerChunkID, renderRadius);
        for (int i = 0; i < chunkIDs.Count; i++)
        {
            int id = (chunkIDs[i]);
            if (allSeenChunks.ContainsKey(id))
            {
                currentChunks.Add(allSeenChunks[id]);
                allSeenChunks[id].GetComponent<Chunk>().EnableChunk();
            }
            else
            {
                // Make the new Chunk
                GameObject c = Instantiate(ChunkPrefab);
                c.GetComponent<Chunk>().SetChunkID(id);
                c.GetComponent<Chunk>().SetSideLength(chunkSize);

                // Get the Perlin value for the new chunk
                float perlin = PerlinNoiseGenerator.GetRandomValue(chunkIDtoPoint2D(id));
                c.GetComponent<Chunk>().SetPerlinValue(perlin);
                c.GetComponent<Chunk>().SetChunkType();
                c.GetComponent<Chunk>().InitializeGround();
                c.GetComponent<Chunk>().SetPlayerTransform(playerTransform);
                c.GetComponent<Chunk>().AttemptToGenerateBuilding(5);
                c.GetComponent<Chunk>().GenerateWater();
                c.GetComponent<Chunk>().EnableChunk();
                allSeenChunks.Add(id, c);
                currentChunks.Add(c);
            }
        }
    }

    public void UpdateRenderRadius(int newRadius)
    {
        this.renderRadius = newRadius;
        this.updateChunks();
    }

    // =======================================
    //
    //               Math Things
    //
    // =======================================
    public static int nearestPerfectSquare(int n)
    {
        int squareJumpAmount = 3;
        int curSquare = 1;
        int prevSquare = 0;
        while (curSquare < n)
        {
            prevSquare = curSquare;
            curSquare += squareJumpAmount;
            squareJumpAmount += 2;  // the difference between consecutive squares is odd integer
        }
        if (n - prevSquare > curSquare - n)
        {
            return curSquare;
        }
        else
        {
            return prevSquare;
        }
    }
    // Assuming n is a perfect square, return the square root of n as an int
    public static int isqrt(int n)
    {
        return (int)Mathf.Round(Mathf.Sqrt(n));
    }
    // Convert a ChunkID to the coordinates of the chunk
    public static Point2D chunkIDtoPoint2D(int n)
    {
        int s = nearestPerfectSquare(n);
        int sq = isqrt(s);
        if (s % 2 == 0)
        {
            if (n >= s)
            {
                return new Point2D(sq / 2, -sq / 2 + n - s);
            }
            else
            {
                return new Point2D(sq / 2 - s + n, -sq / 2);
            }
        }
        else
        {
            if (n >= s)
            {
                return new Point2D(-(sq + 1) / 2, (sq + 1) / 2 - 1 - n + s);
            }
            else
            {
                return new Point2D(-(sq + 1) / 2 + s - n, (sq + 1) / 2 - 1);
            }
        }
    }
    // Wrapper
    public static Vector2 chunkIDtoVector2(int n)
    {
        Point2D converted = chunkIDtoPoint2D(n);
        return new Vector2(converted.x, converted.z);
    }
    // Convert the coordinates of the chunk to the ChunkID
    public static int chunkCoordsToChunkID(int a, int b)
    {
        // Bottom Zone
        if (b > 0 && a >= -b && a < b)
        {
            return 4 * b * b + 3 * b - a;
        }
        // Left Zone
        else if (a < 0 && b < -a && b >= a)
        {
            return 4 * a * a + 3 * a - b;
        }
        // Top Zone
        else if (b < 0 && a <= -b && a > b)
        {
            return 4 * b * b + b + a;
        }
        // Right Zone
        else if (a > 0 && b <= a && b > -a)
        {
            return 4 * a * a + a + b;
        }
        // Only a=0, b=0 is not in a zone
        else
        {
            return 0;
        }
    }
    // Wrapper function
    public static int point2DtoChunkID(Point2D p)
    {
        return chunkCoordsToChunkID(p.x, p.z);
    }
    public static float distanceFormula(float x1, float y1, float x2, float y2)
    {
        return Mathf.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));
    }
    public static float distance2d(Vector3 v1, Vector3 v2)
    {
        return distanceFormula(v1.x, v1.z, v2.x, v2.z);
    }
    // Get a diamond shape of chunk ids centered around the given point
    public static List<int> getChunkIDsAroundPoint(Point2D p, int radius)
    {
        List<int> result = new List<int>();

        // Start at the bottom of the diamond and work up from there
        for (int b = p.z + radius; b >= p.z - radius; b--)
        {
            int distanceFromZ = Mathf.Abs(b - p.z);
            for (int a = p.x - (radius - distanceFromZ); a <= p.x + (radius - distanceFromZ); a++)
            {
                result.Add(chunkCoordsToChunkID(a, b));
            }
        }
        return result;
    }
    // Wrapper
    public static List<int> getChunkIDsAroundID(int id, int radius)
    {
        return getChunkIDsAroundPoint(chunkIDtoPoint2D(id), radius);
    }
    // Get the chunkID of the chunk containing a given point
    public int getChunkIDContainingPoint(Vector3 p, int chunkSize)
    {
        int x = (int)Mathf.Floor(p.x / chunkSize);
        int z = (int)Mathf.Floor(p.z / chunkSize);
        return chunkCoordsToChunkID(x, z);
    }
}

