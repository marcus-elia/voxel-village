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
    public Material buildingMat1;


    private BuildingInfo footprint;
    private Vector3 trueBottomLeft;
    private Side doorSide;
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
    public void SetFootprint(BuildingInfo input)
    {
        footprint = input;
    }
    public void SetTrueBottomLeft(Vector3 input)
    {
        trueBottomLeft = input;
    }
    public void EnableRendering()
    {
        for(int i = 0; i < voxels.Count; i++)
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
        int length = footprint.xWidth;
        float y = trueBottomLeft.y + 0.5f;
        if(doorSide == Side.Top)
        {
            // Randomly put the door along the side
            int doorLoc = Mathf.FloorToInt(Random.Range(1, length - 1));

            // Build the voxel left of the door
            GameObject leftVoxel = Instantiate(cube);
            int xWidth = doorLoc;
            float xCenter = trueBottomLeft.x + xWidth / 2f;
            float zCenter = trueBottomLeft.z + footprint.zWidth - 0.5f;
            leftVoxel.transform.localScale = new Vector3(xWidth, 1, 1);
            leftVoxel.transform.position = new Vector3(xCenter, y, zCenter);
            leftVoxel.GetComponent<Renderer>().material = buildingMat1;
            voxels.Add(leftVoxel);

            // Build the voxel right of the door
            GameObject rightVoxel = Instantiate(cube);
            xWidth = footprint.xWidth - doorLoc - 1;
            xCenter = trueBottomLeft.x + doorLoc + 1f + xWidth / 2f;
            zCenter = trueBottomLeft.z + footprint.zWidth - 0.5f;
            rightVoxel.transform.localScale = new Vector3(xWidth, 1, 1);
            rightVoxel.transform.position = new Vector3(xCenter, y, zCenter);
            rightVoxel.GetComponent<Renderer>().material = buildingMat1;
            voxels.Add(rightVoxel);
        }
        else
        {
            GameObject wall = Instantiate(cube);
            int xWidth = footprint.xWidth;
            float xCenter = trueBottomLeft.x + xWidth / 2f;
            float zCenter = trueBottomLeft.z + footprint.zWidth - 0.5f;
            wall.transform.localScale = new Vector3(xWidth, 1, 1);
            wall.transform.position = new Vector3(xCenter, y, zCenter);
            wall.GetComponent<Renderer>().material = buildingMat1;
            voxels.Add(wall);
        }
    }

    private void CreateLeft()
    {
        int length = footprint.zWidth - 2;
        float y = trueBottomLeft.y + 0.5f;
        if (doorSide == Side.Left)
        {
            // Randomly put the door along the side
            int doorLoc = Mathf.FloorToInt(Random.Range(0, length));

            int zWidth; float xCenter, zCenter;
            // Build the voxel below the door
            if(doorLoc > 0)
            {
                GameObject lowerVoxel = Instantiate(cube);
                zWidth = doorLoc;
                xCenter = trueBottomLeft.x + 0.5f;
                zCenter = trueBottomLeft.z + 1f + zWidth/2f;
                lowerVoxel.transform.localScale = new Vector3(1, 1, zWidth);
                lowerVoxel.transform.position = new Vector3(xCenter, y, zCenter);
                lowerVoxel.GetComponent<Renderer>().material = buildingMat1;
                voxels.Add(lowerVoxel);
            }
            
            // Build the voxel above the door
            if(doorLoc < length - 1)
            {
                GameObject upperVoxel = Instantiate(cube);
                zWidth = length - doorLoc - 1;
                xCenter = trueBottomLeft.x + 0.5f;
                zCenter = trueBottomLeft.z + 1f + 1f + doorLoc + zWidth / 2f;
                upperVoxel.transform.localScale = new Vector3(1, 1, zWidth);
                upperVoxel.transform.position = new Vector3(xCenter, y, zCenter);
                upperVoxel.GetComponent<Renderer>().material = buildingMat1;
                voxels.Add(upperVoxel);
            }
            
        }
        // If this side does not have a door, it is one continuous wall
        else
        {
            GameObject wall = Instantiate(cube);
            int zWidth = length;
            float xCenter = trueBottomLeft.x + 0.5f;
            float zCenter = trueBottomLeft.z + 1f + zWidth / 2f;
            wall.transform.localScale = new Vector3(1, 1, zWidth);
            wall.transform.position = new Vector3(xCenter, y, zCenter);
            wall.GetComponent<Renderer>().material = buildingMat1;
            voxels.Add(wall);
        }
    }

    private void CreateBottom()
    {
        int length = footprint.xWidth;
        float y = trueBottomLeft.y + 0.5f;
        if (doorSide == Side.Bottom)
        {
            // Randomly put the door along the side
            int doorLoc = Mathf.FloorToInt(Random.Range(1, length - 1));

            // Build the voxel left of the door
            GameObject leftVoxel = Instantiate(cube);
            int xWidth = doorLoc;
            float xCenter = trueBottomLeft.x + xWidth / 2f;
            float zCenter = trueBottomLeft.z + 0.5f;
            leftVoxel.transform.localScale = new Vector3(xWidth, 1, 1);
            leftVoxel.transform.position = new Vector3(xCenter, y, zCenter);
            leftVoxel.GetComponent<Renderer>().material = buildingMat1;
            voxels.Add(leftVoxel);

            // Build the voxel right of the door
            GameObject rightVoxel = Instantiate(cube);
            xWidth = footprint.xWidth - doorLoc - 1;
            xCenter = trueBottomLeft.x + doorLoc + 1f + xWidth / 2f;
            zCenter = trueBottomLeft.z + 0.5f;
            rightVoxel.transform.localScale = new Vector3(xWidth, 1, 1);
            rightVoxel.transform.position = new Vector3(xCenter, y, zCenter);
            rightVoxel.GetComponent<Renderer>().material = buildingMat1;
            voxels.Add(rightVoxel);
        }
        else
        {
            GameObject wall = Instantiate(cube);
            int xWidth = footprint.xWidth;
            float xCenter = trueBottomLeft.x + xWidth / 2f;
            float zCenter = trueBottomLeft.z + 0.5f;
            wall.transform.localScale = new Vector3(xWidth, 1, 1);
            wall.transform.position = new Vector3(xCenter, y, zCenter);
            wall.GetComponent<Renderer>().material = buildingMat1;
            voxels.Add(wall);
        }
    }

    private void CreateRight()
    {
        int length = footprint.zWidth - 2;
        float y = trueBottomLeft.y + 0.5f;
        if (doorSide == Side.Right)
        {
            // Randomly put the door along the side
            int doorLoc = Mathf.FloorToInt(Random.Range(0, length));

            int zWidth; float xCenter, zCenter;
            // Build the voxel below the door
            if (doorLoc > 0)
            {
                GameObject lowerVoxel = Instantiate(cube);
                zWidth = doorLoc;
                xCenter = trueBottomLeft.x + footprint.xWidth - 0.5f;
                zCenter = trueBottomLeft.z + 1f + zWidth / 2f;
                lowerVoxel.transform.localScale = new Vector3(1, 1, zWidth);
                lowerVoxel.transform.position = new Vector3(xCenter, y, zCenter);
                lowerVoxel.GetComponent<Renderer>().material = buildingMat1;
                voxels.Add(lowerVoxel);
            }

            // Build the voxel above the door
            if(doorLoc < length - 1)
            {
                GameObject upperVoxel = Instantiate(cube);
                zWidth = length - doorLoc - 1;
                xCenter = trueBottomLeft.x + footprint.xWidth - 0.5f;
                zCenter = trueBottomLeft.z + 1f + 1f + doorLoc + zWidth / 2f;
                upperVoxel.transform.localScale = new Vector3(1, 1, zWidth);
                upperVoxel.transform.position = new Vector3(xCenter, y, zCenter);
                upperVoxel.GetComponent<Renderer>().material = buildingMat1;
                voxels.Add(upperVoxel);
            }
            
        }
        // If this side does not have a door, it is one continuous wall
        else
        {
            GameObject wall = Instantiate(cube);
            int zWidth = length;
            float xCenter = trueBottomLeft.x + footprint.xWidth - 0.5f;
            float zCenter = trueBottomLeft.z + 1f + zWidth / 2f;
            wall.transform.localScale = new Vector3(1, 1, zWidth);
            wall.transform.position = new Vector3(xCenter, y, zCenter);
            wall.GetComponent<Renderer>().material = buildingMat1;
            voxels.Add(wall);
        }
    }
}
