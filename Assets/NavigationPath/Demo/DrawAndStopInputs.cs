using UnityEngine;

public class DrawAndStopInputs : MonoBehaviour
{
    public NavmeshPathDraw navmeshDraw;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.S)){
            navmeshDraw.Stop();
        }

        if(Input.GetKeyDown(KeyCode.A)){
            navmeshDraw.Draw();
        }
    }
}
