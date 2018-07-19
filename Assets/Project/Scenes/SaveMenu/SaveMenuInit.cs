using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SS.Scene;
using TMPro;
public class SaveMenuInit : MonoBehaviour
{
    [SerializeField] GameObject[] buttons;
    enum MenuType {save,load }
    [SerializeField] MenuType menuType;
    // Use this for initialization
    void Start()
    {

        try
        {
            SaveLoadGame.Load();
            foreach (KeyValuePair<int, GameData> saveSlot in SaveLoadGame.savedGames)
            {
                Debug.Log("Save Key: " + saveSlot.Key);
                Debug.Log("data: " + saveSlot.Value._Progress.getStage());
                Debug.Log("data checkpoint: " + saveSlot.Value._Progress.checkPoint);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
        }
        for (int bti = 0; bti < buttons.Length; bti++)
        {
            //buttons[i].GetComponent<Button>().onClick = SaveMenuController.OnSaveButton(i);//Calls the TaskOnClick/TaskWithParameters method when you click the Button
            
            if (SaveLoadGame.savedGames.ContainsKey(bti))
            {
                buttons[bti].transform.Find("Text").GetComponent<TextMeshProUGUI>().text = getSaveDataText(SaveLoadGame.savedGames[bti]);
            }
            int localBti = bti;
            switch (menuType)
            {
                case MenuType.save:
                    Debug.Log("init save btn");
                    buttons[bti].GetComponent<Button>().onClick.AddListener(() => OnSaveButton(localBti));
                    break;
                case MenuType.load:
                    Debug.Log("init load btn func");
                    buttons[bti].GetComponent<Button>().onClick.AddListener(() => OnLoadButton(localBti));
                    break;
            }
            //btn2.onClick.AddListener(delegate { TaskWithParameters("Hello"); });
        }
    }

    public string getSaveDataText(GameData gameData)
    {
        string saveText = gameData._Progress.getStage() + "\n" + gameData.modifiedTime;
        return saveText;
    }

    public void OnSaveButton(int i)
    {
        //Output this to console when the Button is clicked
        Debug.Log("You have clicked the button!" + i);
        SaveLoadGame.Save(i);
        buttons[i].transform.Find("Text").GetComponent<TextMeshProUGUI>().text = getSaveDataText(GameData.current);
    }

    public void OnLoadButton(int i)
    {
        //Output this to console when the Button is clicked
        Debug.Log("You have clicked the button!" + i);
        if (SaveLoadGame.savedGames.ContainsKey(i))
        {
            GameData.current = SaveLoadGame.savedGames[i];
            string savedStage = GameData.current._Progress.getStage();
            SceneManager.LoadScene(savedStage);
        }
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
