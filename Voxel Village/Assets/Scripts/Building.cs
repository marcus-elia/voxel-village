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
    public GameObject cornerPrefab;
    public GameObject floorPrefab;


    private BuildingInfo footprint;
    private Vector3 trueBottomLeft;
    private Vector3 trueCenterBase;
    private Side doorSide;
    private List<GameObject> voxels = new List<GameObject>();
    private List<GameObject> gameObjects = new List<GameObject>();

    private int heightPerFloor;
    private int doorHeight;
    private int numFloors;
    private int totalHeight; // This is calculated based on height per floor and num floors
    private WindowPlan windowPlan;

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
    public void SetHeightPerFloor(int input)
    {
        heightPerFloor = input;
    }
    public void SetDoorHeight(int input)
    {
        doorHeight = input;
    }
    public void SetNumFloors(int input)
    {
        numFloors = input;
    }
    public void CalculateTotalHeight()
    {
        totalHeight = heightPerFloor*numFloors + (numFloors + 1);
    }
    public void SetWindowPlan(WindowPlan input)
    {
        windowPlan = input;
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
        doorSide = GetRandomSide();

        // Create the 4 sides.
        this.CreateTop();
        this.CreateLeft();
        this.CreateBottom();
        this.CreateRight();

        // Create the 4 corners
        this.CreateCorners();

        // Make a floor between each level
        this.CreateFloors();
    }

    public static Side GetRandomSide()
    {
        int rand = Mathf.FloorToInt(Random.Range(0, 4));
        switch(rand)
        {
            case 0:
                return Side.Top; break;
            case 1:
                return Side.Left; break;
            case 2:
                return Side.Bottom; break;
            default:
                return Side.Right; break;
        }
    }

    private void CreateTop()
    {
        int topDoorLoc = (doorSide == Side.Top) ? Mathf.FloorToInt(Random.Range(0, footprint.xWidth - 2)) : -1;
        GameObject wall = Instantiate(wallPrefab);
        wall.GetComponent<Wall>().SetLength(footprint.xWidth - 2);
        wall.GetComponent<Wall>().SetDoorLoc(topDoorLoc);
        wall.GetComponent<Wall>().SetNumFloors(this.numFloors);
        wall.GetComponent<Wall>().SetHeightPerFloor(this.heightPerFloor);
        wall.GetComponent<Wall>().SetDoorHeight(this.doorHeight);
        wall.GetComponent<Wall>().SetTotalHeight(this.totalHeight);
        wall.GetComponent<Wall>().SetWindowPlan(this.windowPlan);
        wall.GetComponent<Wall>().CreateVoxels();

        // Put the wall in the correct position
        wall.transform.parent = transform;
        float x = 0f;
        float y = totalHeight / 2f;
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
        wall.GetComponent<Wall>().SetNumFloors(this.numFloors);
        wall.GetComponent<Wall>().SetHeightPerFloor(this.heightPerFloor);
        wall.GetComponent<Wall>().SetDoorHeight(this.doorHeight);
        wall.GetComponent<Wall>().SetTotalHeight(this.totalHeight);
        wall.GetComponent<Wall>().SetWindowPlan(this.windowPlan);
        wall.GetComponent<Wall>().CreateVoxels();

        // Put the wall in the correct position
        wall.transform.parent = transform;
        float x = 0f;
        float y = totalHeight / 2f;
        float z = -(footprint.zWidth - 1) / 2f;
        wall.transform.localPosition = new Vector3(x, y, z);
        wall.transform.Rotate(Vector3.up, 0f);

        gameObjects.Add(wall);
    }

    private void CreateLeft()
    {
        int topDoorLoc = (doorSide == Side.Left) ? Mathf.FloorToInt(Random.Range(0, footprint.zWidth - 2)) : -1;
        GameObject wall = Instantiate(wallPrefab);
        wall.GetComponent<Wall>().SetLength(footprint.zWidth - 2);
        wall.GetComponent<Wall>().SetDoorLoc(topDoorLoc);
        wall.GetComponent<Wall>().SetNumFloors(this.numFloors);
        wall.GetComponent<Wall>().SetHeightPerFloor(this.heightPerFloor);
        wall.GetComponent<Wall>().SetDoorHeight(this.doorHeight);
        wall.GetComponent<Wall>().SetTotalHeight(this.totalHeight);
        wall.GetComponent<Wall>().SetWindowPlan(this.windowPlan);
        wall.GetComponent<Wall>().CreateVoxels();

        // Put the wall in the correct position
        wall.transform.parent = transform;
        float x = -(footprint.xWidth - 1) / 2f;
        float y = totalHeight / 2f;
        float z = 0f;
        wall.transform.localPosition = new Vector3(x, y, z);
        wall.transform.Rotate(Vector3.up, 270f);

        gameObjects.Add(wall);
    }

    private void CreateRight()
    {
        int topDoorLoc = (doorSide == Side.Right) ? Mathf.FloorToInt(Random.Range(0, footprint.zWidth - 2)) : -1;
        GameObject wall = Instantiate(wallPrefab);
        wall.GetComponent<Wall>().SetLength(footprint.zWidth - 2);
        wall.GetComponent<Wall>().SetDoorLoc(topDoorLoc);
        wall.GetComponent<Wall>().SetNumFloors(this.numFloors);
        wall.GetComponent<Wall>().SetHeightPerFloor(this.heightPerFloor);
        wall.GetComponent<Wall>().SetDoorHeight(this.doorHeight);
        wall.GetComponent<Wall>().SetTotalHeight(this.totalHeight);
        wall.GetComponent<Wall>().SetWindowPlan(this.windowPlan);
        wall.GetComponent<Wall>().CreateVoxels();

        // Put the wall in the correct position
        wall.transform.parent = transform;
        float x = (footprint.xWidth - 1) / 2f;
        float y = totalHeight / 2f;
        float z = 0f;
        wall.transform.localPosition = new Vector3(x, y, z);
        wall.transform.Rotate(Vector3.up, 90f);

        gameObjects.Add(wall);
    }

    private void CreateCorners()
    {
        GameObject corner;
        float x, y, z;

        // Top left
        corner = Instantiate(cornerPrefab);
        corner.GetComponent<Corner>().SetHeight(totalHeight);
        corner.GetComponent<Corner>().CreateVoxel();

        corner.transform.parent = transform;
        x = -(footprint.xWidth - 1) / 2f;
        y = totalHeight / 2f;
        z = (footprint.zWidth - 1) / 2f;
        corner.transform.localPosition = new Vector3(x, y, z);

        gameObjects.Add(corner);

        // Top right
        corner = Instantiate(cornerPrefab);
        corner.GetComponent<Corner>().SetHeight(totalHeight);
        corner.GetComponent<Corner>().CreateVoxel();

        corner.transform.parent = transform;
        x = (footprint.xWidth - 1) / 2f;
        y = totalHeight / 2f;
        z = (footprint.zWidth - 1) / 2f;
        corner.transform.localPosition = new Vector3(x, y, z);

        gameObjects.Add(corner);

        // Bottom right
        corner = Instantiate(cornerPrefab);
        corner.GetComponent<Corner>().SetHeight(totalHeight);
        corner.GetComponent<Corner>().CreateVoxel();

        corner.transform.parent = transform;
        x = (footprint.xWidth - 1) / 2f;
        y = totalHeight / 2f;
        z = -(footprint.zWidth - 1) / 2f;
        corner.transform.localPosition = new Vector3(x, y, z);

        gameObjects.Add(corner);

        // Bottom left
        corner = Instantiate(cornerPrefab);
        corner.GetComponent<Corner>().SetHeight(totalHeight);
        corner.GetComponent<Corner>().CreateVoxel();

        corner.transform.parent = transform;
        x = -(footprint.xWidth - 1) / 2f;
        y = totalHeight / 2f;
        z = -(footprint.zWidth - 1) / 2f;
        corner.transform.localPosition = new Vector3(x, y, z);

        gameObjects.Add(corner);
    }

    private void CreateFloors()
    {
        float yCenter = 0.5f;
        GameObject floor;

        floor = Instantiate(floorPrefab);
        floor.GetComponent<Floor>().SetXWidth(footprint.xWidth - 2);
        floor.GetComponent<Floor>().SetZWidth(footprint.zWidth - 2);
        floor.GetComponent<Floor>().CreateVoxel();
        floor.transform.parent = transform;
        floor.transform.localPosition = new Vector3(0, yCenter, 0);

        gameObjects.Add(floor);

        for(int i = 0; i < numFloors; i++)
        {
            yCenter += heightPerFloor + 1;

            floor = Instantiate(floorPrefab);
            floor.GetComponent<Floor>().SetXWidth(footprint.xWidth - 2);
            floor.GetComponent<Floor>().SetZWidth(footprint.zWidth - 2);
            floor.GetComponent<Floor>().CreateVoxel();
            floor.transform.parent = transform;
            floor.transform.localPosition = new Vector3(0, yCenter, 0);

            gameObjects.Add(floor);
        }

        // Add the roof
        GameObject roof = Instantiate(floorPrefab);
        roof.GetComponent<Floor>().SetXWidth(footprint.xWidth - 2);
        roof.GetComponent<Floor>().SetZWidth(footprint.zWidth - 2);
        roof.GetComponent<Floor>().CreateVoxel(true);
        roof.transform.parent = transform;
        roof.transform.localPosition = new Vector3(0, yCenter + 1f, 0);

        gameObjects.Add(roof);
    }
}
