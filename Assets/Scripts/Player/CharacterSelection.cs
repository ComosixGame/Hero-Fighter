using UnityEngine;
using Cinemachine;

public class CharacterSelection : MonoBehaviour
{
    public GameObject player;
    public EquipmentManager equipmentManager;
    private Vector3 playerPos;
    public CinemachineVirtualCamera virtualCamera;
    [SerializeField] private MapGeneration mapGeneration;
    private GameManager gameManager;

    private void Awake()
    {
        gameManager = GameManager.Instance;
    }

    private void OnEnable()
    {
        gameManager.OnNewGame += NewGame;
    }

    private void OnDisable()
    {
        gameManager.OnNewGame -= NewGame;
    }

    // Start is called before the first frame update
    void Start()
    {
        playerPos = gameManager.levelState.checkPointPlayer;
        SpawnSelectedCharacter(playerPos);
    }

    private void NewGame()
    {
        Destroy(player);
    }

    private void GetPlayerPos(Transform pos)
    {
        player.gameObject.SetActive(false);
        this.playerPos = pos.transform.position;
    }

    //Choice Character in Hero UI
    public void ChoiceCharacters(string id)
    {
        gameManager.SelecteCharacter(id);
    }


    //Call Method When Want Spawn Character
    //Check Point is Position Spawn Player each Level
    public void SpawnSelectedCharacter(Vector3 checkPoint)
    {
        string selectedCharacter = PlayerData.Load().selectedCharacter;
        GameObject character = equipmentManager.GetCharacter(selectedCharacter).character;
        player = Instantiate(character, checkPoint, Quaternion.LookRotation(Vector3.right));
        virtualCamera.Follow = player.transform;
        gameManager.player = player.transform;
        gameManager.virtualCamera = virtualCamera;
    }

    public void RevivalPlayer()
    {
        player.GetComponent<PlayerDamageable>().Revival();
        gameManager.RevivalPlayer();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(playerPos, 0.5f);
    }

}
