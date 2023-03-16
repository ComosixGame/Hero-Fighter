using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class SaveSystem<T> where T : class, new() {
    private static string EncryptWord = "$2a$12$bFGaHUjLQsB12SXjTPGzIuh1Gqc93EA4Wow0ka93uWxZ31X1sfdKy";
    private static string directory = "/Saves";
    public static void Save(T data, bool useEncrypt = false) {
        if(!Directory.Exists(Application.persistentDataPath + directory)) {
            Directory.CreateDirectory(Application.persistentDataPath + directory);
        }
        string json = JsonUtility.ToJson(data);
        string path = Application.persistentDataPath + $"{directory}/{typeof(T).Name}.sav";
        if(useEncrypt) {
            json = Encrypt(json);
        }
        File.WriteAllText(path, json);

    }

    public static T Load(bool useEncrypt = false) {
        string path = Application.persistentDataPath + $"{directory}/{typeof(T).Name}.sav";
        if(File.Exists(path)) {
            string json = File.ReadAllText(path);
            if(useEncrypt) {
                json = Encrypt(json);
            }
            T data = JsonUtility.FromJson<T>(json);
            return data;
        } else {
            T newData = new T();
            Save(newData, useEncrypt);
            return newData;
        }
    }

    private static string Encrypt (string text) {
        string modifiedData = "";
        for(int i = 0; i<text.Length; i++) {
            modifiedData += (char) (text[i] ^ EncryptWord[i % EncryptWord.Length]);
        }

        return modifiedData;
    }
}


public class PlayerData
{
    public int money, selectedCharacter, ownedSkills;
    public int LatestLevel {
        get { 
            return levels.Count > 0 ? levels[levels.Count - 1] : 0;
        }
    }

    public int LatestChapter {
        get {
            return chapters.Count > 0 ? chapters[chapters.Count - 1] : 0;
        }
    }
    public List<int> levels, characters, skills, chapters;


    public PlayerData() {
        money = 0;
        selectedCharacter = 0;
        ownedSkills = 0;
        levels = new List<int>();
        characters = new List<int>();
        skills = new List<int>();
        skills.Add(0);
        characters.Add(0);
    }
    public void Save() {
        SaveSystem<PlayerData>.Save(this, true);
    }
    
    public static PlayerData Load() {
        return SaveSystem<PlayerData>.Load(true);
    }
}


public class SettingData
{
    public bool mute;
    public float resolutionScale;
    public int fps;

    public SettingData() {
        mute = false;
        resolutionScale = 0.7f;
        fps = 60;
    }
    
    public static SettingData Load() {
        return SaveSystem<SettingData>.Load();
    }

    public void Save() {
        SaveSystem<SettingData>.Save(this);
    }

}
