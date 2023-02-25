using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelection : MonoBehaviour
{
    private GameObject player;
    private PlayerData playerData;
    public EquipmentManager equipmentManager;
    
    // Start is called before the first frame update
    void Start()
    {
        playerData = PlayerData.Load();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Choice Character in Hero UI
    public void ChoiceCharacters(int index)
    {
        playerData.selectedCharacter = index;
        playerData.Save();
    }


    //Call Method When Want Spawn Character
    //Check Point is Position Spawn Player each Level
    public void SpawnSelectedCharacter(Vector3 checkPoint)
    {
        playerData = PlayerData.Load();
        int selectedCharacter = playerData.selectedCharacter;
        player = Instantiate(equipmentManager.Characters[selectedCharacter].character, checkPoint, equipmentManager.Characters[selectedCharacter].character.transform.rotation);
    }
}
