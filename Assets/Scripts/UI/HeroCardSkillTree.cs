using UnityEngine;
using UnityEngine.UI;

public class HeroCardSkillTree : MonoBehaviour
{
    private int id;
    private SkillTreeUI skillTreeUI;
    [SerializeField] private Image thumbnail;
    [SerializeField] private Button button;
  

    private void Awake()
    {
        button = GetComponentInChildren<Button>();
    }

    private void OnEnable()
    {
        button.onClick.AddListener(ClickButton);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(ClickButton);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void ClickButton()
    {
        
    }

    public void SetDataCard(int id,PlayerCharacter playerCharacter, SkillTreeUI skillTreeUI)
    {
        this.id = id;
        this.thumbnail.sprite = playerCharacter.thumbnail;
        this.skillTreeUI = skillTreeUI;
    }

    
}
