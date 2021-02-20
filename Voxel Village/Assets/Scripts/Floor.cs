using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{
    public GameObject cubePrefab;

    public Material floorMat;
    public Material roofMat;

    private int xWidth, zWidth;
    private GameObject voxel;

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
        voxel.SetActive(true);
    }
    public void DisableRendering()
    {
        voxel.SetActive(false);
    }

    public void SetXWidth(int input)
    {
        xWidth = input;
    }
    public void SetZWidth(int input)
    {
        zWidth = input;
    }
    public void CreateVoxel(bool isRoof=false)
    {
        voxel = Instantiate(cubePrefab);
        voxel.transform.parent = transform;
        if(isRoof)
        {
            voxel.GetComponent<Renderer>().material = roofMat;
        }
        else
        {
            voxel.GetComponent<Renderer>().material = floorMat;
        }
        voxel.transform.localScale = new Vector3(xWidth, 1f, zWidth);
    }
}
