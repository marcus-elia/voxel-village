using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WindowPlan  {Alternating, Centered, Full, NoWindows};

public class Wall : MonoBehaviour
{
    public GameObject cubePrefab;

    public Material buildingMat;
    public Material glass;

    private int doorLoc;
    private int length;
    private float leftX, rightX; // The ends could be ints or int + 0.5 due to even/odd
    private int numFloors;
    private int heightPerFloor;
    private int doorHeight;
    private List<GameObject> voxels = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Setters
    public void SetLength(int inputLength)
    {
        length = inputLength;
        leftX = 0f - length / 2f;
        rightX = length / 2f;
    }
    public void SetDoorLoc(int inputDoorLoc)
    {
        doorLoc = inputDoorLoc;
    }
    public void SetNumFloors(int inputNumFloors)
    {
        numFloors = inputNumFloors;
    }
    public void SetHeightPerFloor(int input)
    {
        heightPerFloor = input;
    }
    public void SetDoorHeight(int input)
    {
        if(input > heightPerFloor)
        {
            doorHeight = heightPerFloor;
        }
        else
        {
            doorHeight = input;
        }
    }
    public void CreateVoxels()
    {
        CreateFloor(0);
        if(doorLoc >= 0)
        {
            CreateLevelWithDoor(1);
        }
    }
    

    // Helper functions for constructing voxels
    private void CreateFloor(int height)
    {
        GameObject floor = Instantiate(cubePrefab);
        floor.transform.localScale = new Vector3(length, 1, 1);
        floor.GetComponent<Renderer>().material = buildingMat;
        voxels.Add(floor);
    }
    private void CreateLevelWithDoor(int height)
    {
        int xWidth, yWidth;
        float xCenter, yCenter;

        // The left voxel
        if(doorLoc > 0)
        {
            xWidth = doorLoc;
            xCenter = this.leftX + xWidth / 2f;
            yCenter = height + heightPerFloor / 2f;
            GameObject leftVoxel = Instantiate(cubePrefab);
            leftVoxel.transform.localScale = new Vector3(xWidth, heightPerFloor, 1);
            leftVoxel.transform.position = new Vector3(xCenter, yCenter, 0);
            leftVoxel.GetComponent<Renderer>().material = buildingMat;
            voxels.Add(leftVoxel);
        }
        // The right voxel
        if(doorLoc <  length - 1)
        {
            xWidth = this.length - doorLoc - 1;
            xCenter = this.rightX - xWidth / 2f;
            yCenter = height + heightPerFloor / 2f;
            GameObject rightVoxel = Instantiate(cubePrefab);
            rightVoxel.transform.localScale = new Vector3(xWidth, heightPerFloor, 1);
            rightVoxel.transform.position = new Vector3(xCenter, yCenter, 0);
            rightVoxel.GetComponent<Renderer>().material = buildingMat;
            voxels.Add(rightVoxel);
        }
        // Above the door
        if(doorHeight < heightPerFloor)
        {
            xWidth = 1;
            xCenter = doorLoc + 0.5f;
            yWidth = this.heightPerFloor - this.doorHeight;
            yCenter = this.doorHeight + yWidth / 2f;
            GameObject upVoxel = Instantiate(cubePrefab);
            upVoxel.transform.localScale = new Vector3(xWidth, yWidth, 1);
            upVoxel.transform.position = new Vector3(xCenter, yCenter, 0);
            upVoxel.GetComponent<Renderer>().material = buildingMat;
            voxels.Add(upVoxel);
        }
    }
    private void CreateLevelNoWindows(int height)
    {
        GameObject voxel = Instantiate(cubePrefab);
        voxel.transform.localScale = new Vector3(length, heightPerFloor, 1);
        voxel.transform.position = new Vector3(length / 2f, height + heightPerFloor / 2f, 0);
        voxel.GetComponent<Renderer>().material = buildingMat;
        voxels.Add(voxel);
    }
}
