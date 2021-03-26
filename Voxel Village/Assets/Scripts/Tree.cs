using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    public GameObject treeLeavesPrefab;
    public GameObject treeTrunkPrefab;

    private float trunkHeight;
    private float leavesRadius;
    private float trunkRadius;

    private GameObject trunk;
    private GameObject leaves;

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
        trunk.SetActive(true);
        leaves.SetActive(true);
    }
    public void DisableRendering()
    {
        trunk.SetActive(false);
        leaves.SetActive(false);
    }


    public void SetTrunkHeight(float input)
    {
        trunkHeight = input;
    }
    public void SetLeavesRadius(float input)
    {
        leavesRadius = input;
    }
    public void SetTrunkRadius(float input)
    {
        trunkRadius = input;
    }
    public void CreateTree()
    {
        trunk = Instantiate(treeTrunkPrefab);
        trunk.transform.localScale = new Vector3(2*trunkRadius, trunkHeight, 2*trunkRadius);
        trunk.transform.parent = transform;
        trunk.transform.localPosition = Vector3.up * trunkHeight / 2f; //Vector3.zero;// + new Vector3(-trunkRadius / 2f, 0, -trunkRadius / 2f); // 

        leaves = Instantiate(treeLeavesPrefab);
        leaves.transform.localScale = new Vector3(2*leavesRadius, 2*leavesRadius, 2*leavesRadius);
        leaves.transform.parent = transform;
        leaves.transform.localPosition = Vector3.up * (7f*trunkHeight/8) + new Vector3(-leavesRadius, 0f, 0f);//  + leavesRadius/2f);
    }


    // Getters
    public float GetTrunkRadius()
    {
        return trunkRadius;
    }
    public float GetLeavesRadius()
    {
        return leavesRadius;
    }
    public Vector2 GetXZCoords()
    {
        return new Vector2(transform.position.x, transform.position.z);
    }
}
