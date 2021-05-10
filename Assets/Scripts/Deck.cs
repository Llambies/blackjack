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

    public Button subirButton;
    public Button bajarButton;
    public Button apostarButton;

    public Text finalMessage;

    public Text probAMessage;
    public Text probBMessage;
    public Text probCMessage;

    public Text banca;
    public int bancaAmount;
    public Text apuesta;
    public int apuestaAmount;

    public int[] values = new int[52];
    int cardIndex = 0;    
       
    private void Awake()
    {    
        InitCardValues();        

    }

    private void Start()
    {
        bancaAmount = 1000;
        banca.text = bancaAmount.ToString();
        ShuffleCards();
        hitButton.interactable = false;
        stickButton.interactable = false;
        playAgainButton.interactable = false;
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
                ganar();
            }
            else if (Dealer.points == 21)
            {
                hitButton.interactable = false;
                stickButton.interactable = false;
                perder();
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
            probAMessage.text = probA.ToString() + " ";
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
        probBMessage.text = probB.ToString();


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
        probCMessage.text = probC.ToString();
    }

    void PushDealer()
    {
        /*TODO:
         * Dependiendo de cómo se implemente ShuffleCards, es posible que haya que cambiar el índice.
         */
        dealer.GetComponent<CardHand>().Push(faces[cardIndex],values[cardIndex]);
        cardIndex++;
        CalculateProbabilities();
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
            perder();
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
            ganar();
        }
        else if (dealer.GetComponent<CardHand>().points < player.GetComponent<CardHand>().points)
        {
            ganar();
        }
        else if (dealer.GetComponent<CardHand>().points > player.GetComponent<CardHand>().points)
        {
            perder();
        }
        else if (dealer.GetComponent<CardHand>().points == player.GetComponent<CardHand>().points)
        {
            empate();
        }

    }

    public void Apostar()
    {
        subirButton.interactable = false;
        bajarButton.interactable = false;
        apostarButton.interactable = false;

        hitButton.interactable = true;
        stickButton.interactable = true;
        playAgainButton.interactable = true;
        StartGame();
    }

    public void subirApuesta()
    {
        if(apuestaAmount+10 <= bancaAmount)
        {
            apuestaAmount += 10;
            apuesta.text = apuestaAmount.ToString();
        }
        
    }
    public void bajarApuesta()
    {
        if (apuestaAmount - 10 >= 0)
        {
            apuestaAmount -= 10;
            apuesta.text = apuestaAmount.ToString();
        }
    }

    public void ganar()
    {
        finalMessage.text = "Has ganado!";
        bancaAmount += apuestaAmount;
        banca.text = bancaAmount.ToString();
        apuestaAmount = 0;
        apuesta.text = apuestaAmount.ToString();
    }

    public void perder()
    {
        finalMessage.text = "Has perdido!";
        bancaAmount -= apuestaAmount;
        banca.text = bancaAmount.ToString();
        apuestaAmount = 0;
        apuesta.text = apuestaAmount.ToString();
    }

    public void empate()
    {
        finalMessage.text = "Empate!";
        apuestaAmount = 0;
        apuesta.text = apuestaAmount.ToString();
    }

    public void PlayAgain()
    {
        apuestaAmount = 0;
        apuesta.text = apuestaAmount.ToString();

        subirButton.interactable = true;
        bajarButton.interactable = true;
        apostarButton.interactable = true;

        hitButton.interactable = false;
        stickButton.interactable = false;
        playAgainButton.interactable = false;

        finalMessage.text = "";
        player.GetComponent<CardHand>().Clear();
        dealer.GetComponent<CardHand>().Clear();          
        cardIndex = 0;
        ShuffleCards();
        
    }
    
}
