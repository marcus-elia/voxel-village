using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A corner is a vertical prism at the corner of a building
public class Corner : MonoBehaviour
{
    public GameObject cubePrefab;

    public Material cornerMat;

    private int height;
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

    public void SetHeight(int inputHeight)
    {
        height = inputHeight;
    }
    public void CreateVoxel()
    {
        voxel = Instantiate(cubePrefab);
        voxel.transform.parent = transform;
        voxel.GetComponent<Renderer>().material = cornerMat;
        voxel.transform.localScale = new Vector3(1f, height, 1f);
    }
}
