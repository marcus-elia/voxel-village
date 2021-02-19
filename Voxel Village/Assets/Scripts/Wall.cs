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
    private int totalHeight;
    private List<GameObject> voxels = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnableRendering()
    {
        for (int i = 0; i < voxels.Count; i++)
        {
            voxels[i].SetActive(true);
        }
    }
    public void DisableRendering()
    {
        for (int i = 0; i < voxels.Count; i++)
        {
            voxels[i].SetActive(false);
        }
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
        //totalHeight = numFloors * heightPerFloor + numFloors; // A ground between each floor
    }
    public void SetTotalHeight(int input)
    {
        totalHeight = input;
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
        // Make the base floor
        float currentCenterY = -totalHeight / 2f + 0.5f;
        CreateFloor(currentCenterY);

        // Do the first level
        currentCenterY += 0.5f + heightPerFloor / 2f;
        if(doorLoc >= 0)
        {
            CreateLevelWithDoor(currentCenterY);
        }
        else
        {
            CreateLevelFullWindows(currentCenterY);
        }
    }
    

    // Helper functions for constructing voxels
    private void CreateFloor(float yCenter)
    {
        GameObject floor = Instantiate(cubePrefab);
        floor.transform.localScale = new Vector3(length, 1, 1);
        floor.transform.parent = transform;
        floor.transform.localPosition = new Vector3(0f, yCenter, 0f);
        floor.GetComponent<Renderer>().material = buildingMat;
        voxels.Add(floor);
    }
    private void CreateLevelWithDoor(float yCenter)
    {
        int xWidth, yWidth;
        float xCenter;

        // The left voxel
        if(doorLoc > 0)
        {
            xWidth = doorLoc;
            xCenter = -this.length/2f + xWidth / 2f;
            GameObject leftVoxel = Instantiate(cubePrefab);
            leftVoxel.transform.localScale = new Vector3(xWidth, heightPerFloor, 1);
            leftVoxel.transform.parent = transform;
            leftVoxel.transform.localPosition = new Vector3(xCenter, yCenter, 0f);
            leftVoxel.GetComponent<Renderer>().material = buildingMat;
            voxels.Add(leftVoxel);
        }
        // The right voxel
        if(doorLoc <  length - 1)
        {
            xWidth = this.length - doorLoc - 1;
            xCenter = this.length/2f - xWidth / 2f;
            GameObject rightVoxel = Instantiate(cubePrefab);
            rightVoxel.transform.localScale = new Vector3(xWidth, heightPerFloor, 1);
            rightVoxel.transform.parent = transform;
            rightVoxel.transform.localPosition = new Vector3(xCenter, yCenter, 0);
            rightVoxel.GetComponent<Renderer>().material = buildingMat;
            voxels.Add(rightVoxel);
        }
        // Above the door
        if(doorHeight < heightPerFloor)
        {
            xWidth = 1;
            xCenter = -this.length/2f + doorLoc + 0.5f;
            yWidth = this.heightPerFloor - this.doorHeight;
            GameObject upVoxel = Instantiate(cubePrefab);
            upVoxel.transform.localScale = new Vector3(xWidth, yWidth, 1);
            upVoxel.transform.parent = transform;
            upVoxel.transform.localPosition = new Vector3(xCenter, yCenter + heightPerFloor/2f - yWidth/2f, 0);
            upVoxel.GetComponent<Renderer>().material = buildingMat;
            voxels.Add(upVoxel);
        }
    }
    private void CreateLevelNoWindows(float yCenter)
    {
        GameObject voxel = Instantiate(cubePrefab);
        voxel.transform.localScale = new Vector3(length, heightPerFloor, 1);
        voxel.transform.parent = transform;
        voxel.transform.localPosition = new Vector3(0f, yCenter, 0f);
        voxel.GetComponent<Renderer>().material = buildingMat;
        
        voxels.Add(voxel);
    }

    private void CreateLevelFullWindows(float yCenter)
    {
        GameObject voxel = Instantiate(cubePrefab);
        voxel.transform.localScale = new Vector3(length, heightPerFloor, 1);
        voxel.transform.parent = transform;
        voxel.transform.localPosition = new Vector3(0f, yCenter, 0f);
        voxel.GetComponent<Renderer>().material = glass;

        voxels.Add(voxel);
    }


    // Getter
    public int GetTotalHeight()
    {
        return totalHeight;
    }
}
