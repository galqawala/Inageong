using UnityEngine;

public class saveLoad : MonoBehaviour {
    //If something important has been achieved: https://docs.unity3d.com/ScriptReference/PlayerPrefs.Save.html
    public Grid grid;
    float secondsSinceSave = 0;

    public void Save() {
        PlayerPrefs.SetFloat("grid.position.x", grid.transform.position.x);
        PlayerPrefs.SetFloat("grid.position.y", grid.transform.position.y);
        PlayerPrefs.SetFloat("grid.position.z", grid.transform.position.z);

        PlayerPrefs.SetFloat("grid.eulerAngles.x", grid.transform.eulerAngles.x);
        PlayerPrefs.SetFloat("grid.eulerAngles.y", grid.transform.eulerAngles.y);
        PlayerPrefs.SetFloat("grid.eulerAngles.z", grid.transform.eulerAngles.z);
    }

    public void Update() {
        secondsSinceSave += Time.deltaTime;
        if (secondsSinceSave > 10) {
            secondsSinceSave = 0;
            Save();
        }
    }   
     
    public void Start() {
        var prefix = "grid.position.";
        if (PlayerPrefs.HasKey(prefix+"x") && PlayerPrefs.HasKey(prefix+"y") && PlayerPrefs.HasKey(prefix+"z")) {
            grid.transform.position = new Vector3(
                PlayerPrefs.GetFloat(prefix+"x")
            ,   PlayerPrefs.GetFloat(prefix+"y")
            ,   PlayerPrefs.GetFloat(prefix+"z")
            );
        }

        prefix = "grid.eulerAngles.";
        if (PlayerPrefs.HasKey(prefix+"x") && PlayerPrefs.HasKey(prefix+"y") && PlayerPrefs.HasKey(prefix+"z")) {
            grid.transform.eulerAngles = new Vector3(
                PlayerPrefs.GetFloat(prefix+"x")
            ,   PlayerPrefs.GetFloat(prefix+"y")
            ,   PlayerPrefs.GetFloat(prefix+"z")
            );
        }
    }
}