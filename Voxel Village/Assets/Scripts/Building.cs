using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct BuildingInfo
{
    public Point2D bottomLeft;
    public int xWidth;
    public int zWidth;

    public BuildingInfo(Point2D inputBottomLeft, int inputXWidth, int inputZWidth)
    {
        bottomLeft = inputBottomLeft;
        xWidth = inputXWidth;
        zWidth = inputZWidth;
    }
}

public enum Side { Top, Left, Bottom, Right };



public class Building : MonoBehaviour
{
    public GameObject cube;
    public GameObject wallPrefab;
    public Material buildingMat1;


    private BuildingInfo footprint;
    private Vector3 trueBottomLeft;
    private Vector3 trueCenterBase;
    private Side doorSide;
    private List<GameObject> voxels = new List<GameObject>();
    private List<GameObject> gameObjects = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Setters
    public void SetFootprint(BuildingInfo input)
    {
        footprint = input;
    }
    public void SetTrueBottomLeft(Vector3 input)
    {
        trueBottomLeft = input;
    }
    public void SetTrueCenterBase(Vector3 input)
    {
        trueCenterBase = input;
        transform.position = trueCenterBase;
    }
    public void EnableRendering()
    {
        for(int i = 0; i < gameObjects.Count; i++)
        {
            gameObjects[i].SetActive(true);
        }
    }
    public void DisableRendering()
    {
        for (int i = 0; i < gameObjects.Count; i++)
        {
            gameObjects[i].SetActive(false);
        }
    }

    // Functions to create the building
    public void CreateVoxels()
    {
        Debug.Log("calling create voxels");
        doorSide = GetRandomSide();

        // Create the 4 sides. I have chosen the top and bottom to contain the corners
        this.CreateTop();
        this.CreateLeft();
        this.CreateBottom();
        this.CreateRight();
    }

    public static Side GetRandomSide()
    {
        int rand = Mathf.FloorToInt(Random.Range(0, 4));
        switch(rand)
        {
            case 0:
                Debug.Log("top");
                return Side.Top; break;
            case 1:
                Debug.Log("left");
                return Side.Left; break;
            case 2:
                Debug.Log("bottom");
                return Side.Bottom; break;
            default:
                Debug.Log("right");
                return Side.Right; break;
        }
    }

    private void CreateTop()
    {
        int topDoorLoc = (doorSide == Side.Top) ? Mathf.FloorToInt(Random.Range(0, footprint.xWidth - 2)) : -1;
        GameObject wall = Instantiate(wallPrefab);
        wall.GetComponent<Wall>().SetLength(footprint.xWidth - 2);
        wall.GetComponent<Wall>().SetDoorLoc(topDoorLoc);
        wall.GetComponent<Wall>().SetNumFloors(1);
        wall.GetComponent<Wall>().SetHeightPerFloor(4);
        wall.GetComponent<Wall>().SetDoorHeight(2);
        wall.GetComponent<Wall>().CreateVoxels();

        // Put the wall in the correct position
        wall.transform.parent = transform;
        float x = 0f;
        float y = wall.GetComponent<Wall>().GetTotalHeight() / 2f;
        float z = (footprint.zWidth - 1)/2f;
        wall.transform.localPosition = new Vector3(x, y, z);
        wall.transform.Rotate(Vector3.up, 180f);

        gameObjects.Add(wall);
    }

    private void CreateBottom()
    {
        int topDoorLoc = (doorSide == Side.Bottom) ? Mathf.FloorToInt(Random.Range(0, footprint.xWidth - 2)) : -1;
        GameObject wall = Instantiate(wallPrefab);
        wall.GetComponent<Wall>().SetLength(footprint.xWidth - 2);
        wall.GetComponent<Wall>().SetDoorLoc(topDoorLoc);
        wall.GetComponent<Wall>().SetNumFloors(1);
        wall.GetComponent<Wall>().SetHeightPerFloor(4);
        wall.GetComponent<Wall>().SetDoorHeight(2);
        wall.GetComponent<Wall>().CreateVoxels();

        // Put the wall in the correct position
        wall.transform.parent = transform;
        float x = 0f;
        float y = wall.GetComponent<Wall>().GetTotalHeight() / 2f;
        float z = -(footprint.zWidth - 1) / 2f;
        wall.transform.localPosition = new Vector3(x, y, z);
        wall.transform.Rotate(Vector3.up, 0f);

        gameObjects.Add(wall);
    }

    private void CreateLeft()
    {
        int topDoorLoc = (doorSide == Side.Left) ? Mathf.FloorToInt(Random.Range(0, footprint.xWidth - 2)) : -1;
        GameObject wall = Instantiate(wallPrefab);
        wall.GetComponent<Wall>().SetLength(footprint.zWidth - 2);
        wall.GetComponent<Wall>().SetDoorLoc(topDoorLoc);
        wall.GetComponent<Wall>().SetNumFloors(1);
        wall.GetComponent<Wall>().SetHeightPerFloor(4);
        wall.GetComponent<Wall>().SetDoorHeight(2);
        wall.GetComponent<Wall>().CreateVoxels();

        // Put the wall in the correct position
        wall.transform.parent = transform;
        float x = -(footprint.xWidth - 1) / 2f;
        float y = wall.GetComponent<Wall>().GetTotalHeight() / 2f;
        float z = 0f;
        wall.transform.localPosition = new Vector3(x, y, z);
        wall.transform.Rotate(Vector3.up, 270f);

        gameObjects.Add(wall);
    }

    private void CreateRight()
    {
        int topDoorLoc = (doorSide == Side.Right) ? Mathf.FloorToInt(Random.Range(0, footprint.xWidth - 2)) : -1;
        GameObject wall = Instantiate(wallPrefab);
        wall.GetComponent<Wall>().SetLength(footprint.zWidth - 2);
        wall.GetComponent<Wall>().SetDoorLoc(topDoorLoc);
        wall.GetComponent<Wall>().SetNumFloors(1);
        wall.GetComponent<Wall>().SetHeightPerFloor(4);
        wall.GetComponent<Wall>().SetDoorHeight(2);
        wall.GetComponent<Wall>().CreateVoxels();

        // Put the wall in the correct position
        wall.transform.parent = transform;
        float x = (footprint.xWidth - 1) / 2f;
        float y = wall.GetComponent<Wall>().GetTotalHeight() / 2f;
        float z = 0f;
        wall.transform.localPosition = new Vector3(x, y, z);
        wall.transform.Rotate(Vector3.up, 90f);

        gameObjects.Add(wall);
    }
}
