using UnityEngine;
using UnityEngine.UI;

public class HeroManagerUI : MonoBehaviour
{
    [SerializeField] private GameObject contentHero;
    [SerializeField] private GameObject cardHero;
    [SerializeField] private ScrollRect scrollRect;
    private RectTransform currentCard;
    private GameManager gameManager;
    private void Awake()
    {
        gameManager = GameManager.Instance;
    }

    private void Start()
    {
        RenderHeroCardUI();
    }

    private void RenderHeroCardUI()
    {
        PlayerCharacter[] playerCharacters = gameManager.equipmentManager.Characters;

        for (int i = 0; i < playerCharacters.Length; i++)
        {
            GameObject heroCardInit = Instantiate(cardHero);
            HeroCard newCardHero = heroCardInit.GetComponent<HeroCard>();
            newCardHero.id = i;
            newCardHero.SetDataCard(playerCharacters[i].thumbnail, playerCharacters[i].name,
            playerCharacters[i].price, playerCharacters[i].start, playerCharacters[i].evaluate);
            newCardHero.transform.SetParent(contentHero.transform, false);
        }
    }

    public void ScrollTo(RectTransform target)
    {
        Ultils.ScrollTo(scrollRect, target);
        currentCard = target;
    }
}
