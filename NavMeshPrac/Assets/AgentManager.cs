using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentManager : MonoBehaviour
{
    // Collect agent objects and keep in an array
    GameObject[] agents;

    // Start is called before the first frame update
    void Start()
    {
        agents = GameObject.FindGameObjectsWithTag("AI");
    }

    // Update is called once per frame
    void Update()
    {
        // Click on environment, get the mouse position and then go there
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100)) // Distance of 100
            {
                foreach (GameObject a in agents)
                {
                    a.GetComponent<AIController>().agent.SetDestination(hit.point);
                }
            }
        }
        
    }
}
