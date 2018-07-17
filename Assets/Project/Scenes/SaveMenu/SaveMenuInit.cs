using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SS.Scene;

public class SaveMenuInit : MonoBehaviour
{
    [SerializeField] GameObject[] buttons;
    // Use this for initialization
    void Start()
    {
        SaveLoadGame.Load();
        for (int i = 0; i < buttons.Length; i++)
        {
            //buttons[i].GetComponent<Button>().onClick = SaveMenuController.OnSaveButton(i);//Calls the TaskOnClick/TaskWithParameters method when you click the Button
            buttons[i].GetComponent<Button>().onClick.AddListener(delegate { SaveLoadGame.Save(i); });
            //btn2.onClick.AddListener(delegate { TaskWithParameters("Hello"); });
        }
    }

    void OnSaveButton(int i)
    {
        //Output this to console when the Button is clicked
        Debug.Log("You have clicked the button!" + i);
        SaveLoadGame.Save();
    }

    void TaskWithParameters(string message)
    {
        //Output this to console when the Button is clicked
        Debug.Log(message);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
