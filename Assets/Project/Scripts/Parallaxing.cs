using UnityEngine;
using System.Collections;

public class Parallaxing : MonoBehaviour
{

    public Transform[] backgrounds;         // Array (list) of all the back- and foregrounds to be parallaxed
    private float[] parallaxScales;         // The proportion of the camera's movement to move the backgrounds by
    public float smoothing = 1f;            // How smooth the parallax is going to be. Make sure to set this above 0

    private Transform cam;                  // reference to the main cameras transform
    private Vector3 previousCamPos;         // the position of the camera in the previous frame
                                            // GameObject go;

    // Use this for initialization
    void Start()
    {
        // asigning coresponding parallaxScales
        parallaxScales = new float[backgrounds.Length];
        for (int i = 0; i < backgrounds.Length; i++)
        {
            parallaxScales[i] = backgrounds[i].position.z * -1;
        }
        // The previous frame had the current frame's camera position
        if (GameData.current != null)
        {
            cam = StageData.current.playerCamera.transform;
            previousCamPos = cam.position;
        }
        
        // go = GameObject.Find("redsquare");
        // Debug.Log(go);
    }

    // Update is called once per frame
    void Update()
    {
        if (cam != null)
        {
            // for each background
            for (int i = 0; i < backgrounds.Length; i++)
            {
                // the parallax is the opposite of the camera movement because the previous frame multiplied by the scale
                float parallax = (previousCamPos.x - cam.position.x) * parallaxScales[i];
                float parallaxY = (previousCamPos.y - cam.position.y) * parallaxScales[i];

                // set a target x position which is the current position plus the parallax
                float backgroundTargetPosX = backgrounds[i].position.x + parallax;
                float backgroundTargetPosY = backgrounds[i].position.y + parallaxY;

                // create a target position which is the background's current position with it's target x position
                Vector3 backgroundTargetPos = new Vector3(backgroundTargetPosX, backgroundTargetPosY, backgrounds[i].position.z);

                // fade between current position and the target position using lerp
                backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, backgroundTargetPos, smoothing * Time.deltaTime);


            }

        }
        else
        {
            cam = StageData.current.playerCamera.transform;
        }
        // set the previousCamPos to the camera's position at the end of the frame

        previousCamPos = cam.position;
    }
}
