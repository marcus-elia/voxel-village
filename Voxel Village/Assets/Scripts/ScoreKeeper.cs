using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreKeeper : MonoBehaviour
{
    public static int chunksGenerated;
    public static int chunksVisited;
    public static float maxPlayerHeight;

    public GameObject scoreUI;

    private static Dictionary<int, bool> visitedChunks = new Dictionary<int, bool>();

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateChunk(int id)
    {
        chunksGenerated++;
        visitedChunks[id] = false;
        scoreUI.GetComponent<ScoreUI>().chunksGeneratedText.text = chunksGenerated.ToString();
    }
    public void VisitChunk(int id)
    {
        if(!visitedChunks[id])
        {
            visitedChunks[id] = true;
            chunksVisited++;
            scoreUI.GetComponent<ScoreUI>().chunksVisitedText.text = chunksVisited.ToString();
        }
    }
    public void CheckPlayerHeight(float height)
    {
        if(height > maxPlayerHeight)
        {
            maxPlayerHeight = height;
            scoreUI.GetComponent<ScoreUI>().maxHeightText.text = ((int)maxPlayerHeight).ToString();
        }
    }
}
