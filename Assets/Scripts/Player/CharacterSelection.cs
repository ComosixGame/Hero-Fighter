using UnityEngine;
using Cinemachine;

public class CharacterSelection : MonoBehaviour
{
    private GameObject player;
    private PlayerData playerData;
    public EquipmentManager equipmentManager;
    public Vector3 playerPos;
    public CinemachineVirtualCamera virtualCamera;
    private GameManager gameManager;

    private void Awake()
    {
        gameManager = GameManager.Instance;
    }

    private void OnEnable() 
    {
        gameManager.OnStartGame += StartGame;
        gameManager.OnLose += GetPlayerPos;
        gameManager.OnNewGame += NewGame;
    }

    private void OnDisable() 
    {
        gameManager.OnStartGame -= StartGame;
        gameManager.OnLose += GetPlayerPos;
        gameManager.OnNewGame -= NewGame;
    }

    // Start is called before the first frame update
    void Start()
    {
        playerData = PlayerData.Load();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void StartGame()
    {
        SpawnSelectedCharacter(playerPos);
    }

    private void NewGame()
    {
        GameObject.Destroy(player);
    }

    private void GetPlayerPos(Transform pos)
    {
        player.gameObject.SetActive(false);
        this.playerPos = pos.transform.position;
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
        player = Instantiate(equipmentManager.Characters[selectedCharacter].character, checkPoint,equipmentManager.Characters[selectedCharacter].character.transform.rotation);
        virtualCamera.Follow = player.transform;
    }

    public void PlayerRevival()
    {
        SpawnSelectedCharacter(playerPos);
    }

    private void OnDrawGizmos() 
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(playerPos, 0.5f);
    }


}
