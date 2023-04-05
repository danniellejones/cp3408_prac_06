using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node 
{
    public List<Edge> edgeList = new List<Edge>();
    public Node path = null;
    // Game object at the node location
    GameObject id;
    // Help calculate distances between and where tank has to go
    //public float xPos;
    //public float yPos;
    //public float zPos;

    public float f, g, h;
    // Store another node, where they came from
    public Node cameFrom;

    public Node(GameObject i)
    {
        id = i;
        //xPos = i.transform.position.x;
        //yPos = i.transform.position.y;
        //zPos = i.transform.position.z;
        path = null;
    }

    // Used for comparing against another 
    public GameObject getId()
    {
        return id;
    }

}
