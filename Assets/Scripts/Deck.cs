using UnityEngine;
using UnityEngine.UI;

public class Deck : MonoBehaviour
{
    public Sprite[] faces;
    public GameObject dealer;
    public GameObject player;
    public Button hitButton;
    public Button stickButton;
    public Button playAgainButton;
    public Text finalMessage;
    public Text probMessage;

    public int[] values = new int[52];
    int cardIndex = 0;    
       
    private void Awake()
    {    
        InitCardValues();        

    }

    private void Start()
    {
        ShuffleCards();
        StartGame();        
    }

    private void InitCardValues()
    {
        int cuenta = 0;
        for (int i = 0; i < values.Length; i++)
        {
            cuenta++;
            if (cuenta > 13)
            {
                cuenta = 1;
            }

            if (cuenta >= 10)
            {
                values[i] = 10;
            }
            else
            {
                values[i] = cuenta;
            }
        }
    }

    private void ShuffleCards()
    {
        int[] valuesAux = new int[52];
        Sprite[] facesAux = new Sprite[52];
        int aux = 0;



        for (int i = 0; i < 52; i++)
        {
            int valor = 0;
            while (valor == 0)
            {
                aux = Random.Range(0, 52);
                valor = values[aux];
            }
            values[aux] = 0;
            valuesAux[i] = valor;
            facesAux[i] = faces[aux];
        }

        values = valuesAux;
        faces = facesAux;
    }

    void StartGame()
    {
        for (int i = 0; i < 2; i++)
        {
            PushPlayer();
            PushDealer();

            CardHand Player = player.gameObject.GetComponent<CardHand>();
            CardHand Dealer = dealer.gameObject.GetComponent<CardHand>();

            if (Player.points == 21)
            {
                hitButton.interactable = false;
                stickButton.interactable = false;
                finalMessage.text = "Has ganado!";
            }
            else if (Dealer.points == 21)
            {
                hitButton.interactable = false;
                stickButton.interactable = false;
                finalMessage.text = "Has perdido!";
            }

        }
    }

    private void CalculateProbabilities()
    {
        /*TODO:
         * Calcular las probabilidades de:
         * - Teniendo la carta oculta, probabilidad de que el dealer tenga más puntuación que el jugador
         * - Probabilidad de que el jugador obtenga entre un 17 y un 21 si pide una carta
         * - Probabilidad de que el jugador obtenga más de 21 si pide una carta          
         */
    }

    void PushDealer()
    {
        /*TODO:
         * Dependiendo de cómo se implemente ShuffleCards, es posible que haya que cambiar el índice.
         */
        dealer.GetComponent<CardHand>().Push(faces[cardIndex],values[cardIndex]);
        cardIndex++;        
    }

    void PushPlayer()
    {
        /*TODO:
         * Dependiendo de cómo se implemente ShuffleCards, es posible que haya que cambiar el índice.
         */
        player.GetComponent<CardHand>().Push(faces[cardIndex], values[cardIndex]/*,cardCopy*/);
        cardIndex++;
        CalculateProbabilities();
    }       

    public void Hit()
    {
        /*TODO: 
         * Si estamos en la mano inicial, debemos voltear la primera carta del dealer.
         */
        
        //Repartimos carta al jugador
        PushPlayer();

        /*TODO:
         * Comprobamos si el jugador ya ha perdido y mostramos mensaje
         */      

    }

    public void Stand()
    {
        /*TODO: 
         * Si estamos en la mano inicial, debemos voltear la primera carta del dealer.
         */

        /*TODO:
         * Repartimos cartas al dealer si tiene 16 puntos o menos
         * El dealer se planta al obtener 17 puntos o más
         * Mostramos el mensaje del que ha ganado
         */                
         
    }

    public void PlayAgain()
    {
        hitButton.interactable = true;
        stickButton.interactable = true;
        finalMessage.text = "";
        player.GetComponent<CardHand>().Clear();
        dealer.GetComponent<CardHand>().Clear();          
        cardIndex = 0;
        ShuffleCards();
        StartGame();
    }
    
}
