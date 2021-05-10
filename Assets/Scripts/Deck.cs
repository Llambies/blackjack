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
    public Text probAMessage;
    public Text probBMessage;
    public Text probCMessage;

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
        int manoJugador = player.GetComponent<CardHand>().points;


        if (cardIndex == 4)
        {
            int manoDealer = dealer.GetComponent<CardHand>().cards[1].GetComponent<CardModel>().value;
            float casosFavorablesA = 0;

            for (int i = 0; i < 52; i++)
            {
                if (player.GetComponent<CardHand>().cards.Count > 1 && player.GetComponent<CardHand>().points != 21)
                {

                    if (dealer.GetComponent<CardHand>().cards[1].GetComponent<CardModel>().front.Equals(faces[i]) || player.GetComponent<CardHand>().cards[0].GetComponent<CardModel>().front.Equals(faces[i]) || player.GetComponent<CardHand>().cards[1].GetComponent<CardModel>().front.Equals(faces[i]))
                    {

                    }
                    else
                    {
                        if (manoDealer + values[i] > manoJugador)
                        {
                            casosFavorablesA++;
                        }
                        else if ((manoDealer == 1 || values[i] == 1) && manoDealer + values[i] == 11)
                        {
                            casosFavorablesA++;
                        }
                    }
                }

            }
            float probA = casosFavorablesA / 49;
            //Debug.Log(probA);
            probAMessage.text = "Proba A : " + probA.ToString() + " ";
        }

        float casosFavorablesB = 0;

        for (int i = 0; i < 52; i++)
        {
            bool repartida = false;
            if (dealer.GetComponent<CardHand>().cards.Count != 0)
            {
                for (int j = 0; j < dealer.GetComponent<CardHand>().cards.Count; j++)
                {
                    if (dealer.GetComponent<CardHand>().cards[j].GetComponent<CardModel>().front.Equals(faces[i]))
                    {
                        repartida = true;
                    }
                }
            }

            if (player.GetComponent<CardHand>().cards.Count != 0)
            {
                for (int j = 0; j < player.GetComponent<CardHand>().cards.Count; j++)
                {
                    if (player.GetComponent<CardHand>().cards[j].GetComponent<CardModel>().front.Equals(faces[i]))
                    {
                        repartida = true;
                    }
                }
            }


            if (!repartida)
            {
                if (17 <= manoJugador + values[i] && manoJugador + values[i] <= 21)
                {
                    casosFavorablesB++;
                }
            }

        }
        float probB = casosFavorablesB / (52 - dealer.GetComponent<CardHand>().cards.Count - player.GetComponent<CardHand>().cards.Count);
        //Debug.Log(probB);
        probBMessage.text = "Proba B : " + probB.ToString();


        float casosFavorablesC = 0;

        for (int i = 0; i < 52; i++)
        {
            bool repartida = false;
            if (dealer.GetComponent<CardHand>().cards.Count != 0)
            {
                for (int j = 0; j < dealer.GetComponent<CardHand>().cards.Count; j++)
                {
                    if (dealer.GetComponent<CardHand>().cards[j].GetComponent<CardModel>().front.Equals(faces[i]))
                    {
                        repartida = true;
                    }
                }
            }

            if (player.GetComponent<CardHand>().cards.Count != 0)
            {
                for (int j = 0; j < player.GetComponent<CardHand>().cards.Count; j++)
                {
                    if (player.GetComponent<CardHand>().cards[j].GetComponent<CardModel>().front.Equals(faces[i]))
                    {
                        repartida = true;
                    }
                }
            }


            if (!repartida)
            {
                if (manoJugador + values[i] > 21)
                {
                    casosFavorablesC++;
                }
            }

        }
        float probC = casosFavorablesC / (52 - dealer.GetComponent<CardHand>().cards.Count - player.GetComponent<CardHand>().cards.Count);
        //Debug.Log(probC);
        probCMessage.text = "Proba C : " + probC.ToString();
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
        if (cardIndex < 5)
        {
            dealer.GetComponent<CardHand>().InitialToggle();
        }
        //Repartimos carta al jugador
        PushPlayer();

        if (player.GetComponent<CardHand>().points > 21)
        {
            hitButton.interactable = false;
            stickButton.interactable = false;
            finalMessage.text = "Has perdido!";
        }

    }

    public void Stand()
    {
        hitButton.interactable = false;
        stickButton.interactable = false;
        if (cardIndex < 5)
        {
            dealer.GetComponent<CardHand>().InitialToggle();
        }

        while (dealer.GetComponent<CardHand>().points < 17)
        {
            PushDealer();
        }
        if (dealer.GetComponent<CardHand>().points > 21)
        {
            finalMessage.text = "Has ganado!";
        }
        else if (dealer.GetComponent<CardHand>().points < player.GetComponent<CardHand>().points)
        {
            finalMessage.text = "Has ganado!";
        }
        else if (dealer.GetComponent<CardHand>().points > player.GetComponent<CardHand>().points)
        {
            finalMessage.text = "Has perdido!";
        }
        else if (dealer.GetComponent<CardHand>().points == player.GetComponent<CardHand>().points)
        {
            finalMessage.text = "Empate!";
        }

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
