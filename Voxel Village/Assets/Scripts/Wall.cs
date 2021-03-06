﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WindowPlan  {Centered, Alternating, Full, NoWindows};

public class Wall : MonoBehaviour
{
    public GameObject cubePrefab;

    public Material buildingMat;
    public Material glass;
    public Texture redRC;
    private Texture texture;

    private int doorLoc;
    private int length;
    private float leftX, rightX; // The ends could be ints or int + 0.5 due to even/odd
    private int numFloors;
    private int heightPerFloor;
    private int doorHeight;
    private int totalHeight;
    private WindowPlan windowPlan;
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
    public void SetTexture(Texture input)
    {
        texture = input;
    }
    public void SetWindowPlan(WindowPlan input)
    {
        windowPlan = input;
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
            CreateLevel(currentCenterY);
        }
        currentCenterY += 0.5f + heightPerFloor / 2f;
        CreateFloor(currentCenterY);

        // Do the rest of the levels
        for (int i = 1; i < numFloors; i++)
        {
            currentCenterY += 0.5f + heightPerFloor / 2f;
            CreateLevel(currentCenterY);
            currentCenterY += 0.5f + heightPerFloor / 2f;
            CreateFloor(currentCenterY);
        }
    }
    

    // Helper functions for constructing voxels
    private void CreateFloor(float yCenter)
    {
        GameObject floor = Instantiate(cubePrefab);
        floor.transform.localScale = new Vector3(length, 1, 1);
        floor.transform.parent = transform;
        floor.transform.localPosition = new Vector3(0f, yCenter, 0f);
        floor.GetComponent<Renderer>().material.mainTexture = this.texture;
        floor.GetComponent<Renderer>().material.mainTextureScale = new Vector2(length, 1);
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
            leftVoxel.GetComponent<Renderer>().material.mainTexture = this.texture;
            leftVoxel.GetComponent<Renderer>().material.mainTextureScale = new Vector2(xWidth, heightPerFloor);
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
            rightVoxel.GetComponent<Renderer>().material.mainTexture = this.texture;
            rightVoxel.GetComponent<Renderer>().material.mainTextureScale = new Vector2(xWidth, heightPerFloor);
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
            upVoxel.GetComponent<Renderer>().material.mainTexture = this.texture;
            upVoxel.GetComponent<Renderer>().material.mainTextureScale = new Vector2(xWidth, yWidth);
            voxels.Add(upVoxel);
        }
    }
    private void CreateLevel(float yCenter)
    {
        if(this.windowPlan == WindowPlan.NoWindows)
        {
            CreateLevelNoWindows(yCenter);
        }
        else if(this.windowPlan == WindowPlan.Alternating)
        {
            CreateLevelAlternatingWindows(yCenter);
        }
        else if(this.windowPlan == WindowPlan.Centered)
        {
            CreateLevelCenteredWindows(yCenter);
        }
        else if(this.windowPlan == WindowPlan.Full)
        {
            CreateLevelFullWindows(yCenter);
        }
    }
    private void CreateLevelNoWindows(float yCenter)
    {
        GameObject voxel = Instantiate(cubePrefab);
        voxel.transform.localScale = new Vector3(length, heightPerFloor, 1);
        voxel.transform.parent = transform;
        voxel.transform.localPosition = new Vector3(0f, yCenter, 0f);
        voxel.GetComponent<Renderer>().material.mainTexture = this.texture;
        voxel.GetComponent<Renderer>().material.mainTextureScale = new Vector2(length, heightPerFloor);

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


    private void CreateLevelCenteredWindows(float yCenter)
    {
        int windowHeight;
        int windowWidth;

        // If the wall is wide enough, make left/right borders
        if (length > 2)
        {
            int borderWidth;
            if(length % 4 < 2)
            {
                borderWidth = length / 4;
            }
            else 
            {
                borderWidth = length / 4 + 1;
            }

            // Left border 
            GameObject leftVoxel = Instantiate(cubePrefab);
            leftVoxel.transform.localScale = new Vector3(borderWidth, heightPerFloor, 1);
            leftVoxel.transform.parent = transform;
            leftVoxel.transform.localPosition = new Vector3(-length / 2f + borderWidth / 2f, yCenter, 0f);
            leftVoxel.GetComponent<Renderer>().material.mainTexture = this.texture;
            leftVoxel.GetComponent<Renderer>().material.mainTextureScale = new Vector2(borderWidth, heightPerFloor);
            voxels.Add(leftVoxel);

            // Right border
            GameObject rightVoxel = Instantiate(cubePrefab);
            rightVoxel.transform.localScale = new Vector3(borderWidth, heightPerFloor, 1);
            rightVoxel.transform.parent = transform;
            rightVoxel.transform.localPosition = new Vector3(length / 2f - borderWidth / 2f, yCenter, 0f);
            rightVoxel.GetComponent<Renderer>().material.mainTexture = this.texture;
            rightVoxel.GetComponent<Renderer>().material.mainTextureScale = new Vector2(borderWidth, heightPerFloor);
            voxels.Add(rightVoxel);

            windowWidth = length - 2 * borderWidth;
        }
        else
        {
            windowWidth = length;
        }

        // If the wall is tall enough, make bottom/top borders
        if (heightPerFloor > 2)
        {
            int borderHeight;
            if (heightPerFloor % 4 < 2)
            {
                borderHeight = heightPerFloor / 4;
            }
            else
            {
                borderHeight = heightPerFloor / 4 + 1;
            }

            // Top border
            GameObject topVoxel = Instantiate(cubePrefab);
            topVoxel.transform.localScale = new Vector3(windowWidth, borderHeight, 1);
            topVoxel.transform.parent = transform;
            topVoxel.transform.localPosition = new Vector3(0f, yCenter + heightPerFloor / 2f - borderHeight / 2f, 0f);
            topVoxel.GetComponent<Renderer>().material.mainTexture = this.texture;
            topVoxel.GetComponent<Renderer>().material.mainTextureScale = new Vector2(windowWidth, borderHeight);
            voxels.Add(topVoxel);

            // Bottom border
            GameObject bottomVoxel = Instantiate(cubePrefab);
            bottomVoxel.transform.localScale = new Vector3(windowWidth, borderHeight, 1);
            bottomVoxel.transform.parent = transform;
            bottomVoxel.transform.localPosition = new Vector3(0f, yCenter - heightPerFloor / 2f + borderHeight / 2f, 0f);
            bottomVoxel.GetComponent<Renderer>().material.mainTexture = this.texture;
            bottomVoxel.GetComponent<Renderer>().material.mainTextureScale = new Vector2(windowWidth, borderHeight);
            voxels.Add(bottomVoxel);

            windowHeight = heightPerFloor - borderHeight * 2;
        }
        else
        {
            windowHeight = heightPerFloor;
        }

        // Make the window
        GameObject voxel = Instantiate(cubePrefab);
        voxel.transform.localScale = new Vector3(windowWidth, windowHeight, 1);
        voxel.transform.parent = transform;
        voxel.transform.localPosition = new Vector3(0f, yCenter, 0f);
        voxel.GetComponent<Renderer>().material = glass;
        voxels.Add(voxel);
    }

    private void CreateLevelAlternatingWindows(float yCenter)
    {
        // If it's even length, don't do alternating.
        if(length % 2 == 0)
        {
            CreateLevelNoWindows(yCenter);
        }
        // If length is odd, then we are good.
        else
        {
            bool isWindow = true;
            for(int i = -(length/2); i < length/2 + 1; i++)
            {
                GameObject voxel = Instantiate(cubePrefab);
                voxel.transform.localScale = new Vector3(1, heightPerFloor, 1);
                voxel.transform.parent = transform;
                voxel.transform.localPosition = new Vector3(i, yCenter, 0f);
                if(isWindow)
                {
                    voxel.GetComponent<Renderer>().material = glass;
                }
                else
                {
                    voxel.GetComponent<Renderer>().material.mainTexture = this.texture;
                    voxel.GetComponent<Renderer>().material.mainTextureScale = new Vector2(1, heightPerFloor);
                }

                voxels.Add(voxel);

                // keep track of the alternatingness
                isWindow = !isWindow;
            }
        }
    }


    // Getter
    public int GetTotalHeight()
    {
        return totalHeight;
    }
}
