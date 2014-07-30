using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class TurnManager : MonoBehaviour {

    public static TurnManager turnManager;

    public UITweener gameScale;

    public enum TurnState
    {
        OutOfBattle,        
        EnemyTurn,
        PlayerInactive,
        PlayerDraw,
        PlayerActive,        
        PlayerCleanup,    
        StartBattle,
        EndShow
    }
    
    private TurnState currentTurnState = TurnState.EndShow;
    public TurnState CurrentState { get { return currentTurnState; } }

    public UILabel ShowNumberLabel;
    public UILabel CallInfoHeaderLabel;

    public string CallInfoHeaderDefaultText = "#YesCallMen - Call Info";

	// Use this for initialization
	void Start () {
        turnManager = this;

        int showNumber = Globals.GetInstance().PlayerShowsCompleted + 1;
        ShowNumberLabel.text = showNumber.ToString();
        CallInfoHeaderLabel.text = CallInfoHeaderDefaultText;
	}
	
	// Update is called once per frame
	void Update () {

	}

    private bool started = false;

    public void StartStateMachine()
    {
        if (!started)
        {
            ChangeState(TurnState.StartBattle);
            started = true;
        }
    }

    public void ChangeState(TurnState newState)
    {
        switch (newState)
        {
            case TurnState.OutOfBattle:
                StartCoroutine(DoOutOfBattle());
                break;
            case TurnState.EnemyTurn:
                DoEnemyTurn();
                break;
            case TurnState.PlayerInactive:
                DoPlayerInactive();
                break;
            case TurnState.PlayerDraw:
                DoPlayerDraw();
                break;
            case TurnState.PlayerActive:
                DoPlayerActive();
                break;
            case TurnState.PlayerCleanup:
                DoPlayerCleanup();
                break;
            case TurnState.StartBattle:
                DoStartBattle();
                break;
            case TurnState.EndShow:
                StartCoroutine(DoEndShow());
                break;
            default:
                break;
        }
    }

    private IEnumerator DoEndShow()
    {
        currentTurnState = TurnState.EndShow;

        

        string finishText = "";

        if (Globals.GetInstance().PlayerFinishedLastShow)
        {
            finishText = ">> Well that's all the time we have for tonight's show. Join me next time on #YesCallMen to get more TRUTH BOMBS dropped on you.";
        }
        else
        {
            finishText = ">> I'M DONE! I'M JUST DONE!! MAYBE WE'LL BE BACK TOMORROW, AND HOPEFULLY THESE DAMN FEMINAZIS WILL STOP WITH THEIR BULLSHIT!";
        }

        //GameMessageManager.gameMessageManager.AddLine("", false);
        //GameMessageManager.gameMessageManager.AddLine("", false);
        //GameMessageManager.gameMessageManager.AddLine("", false);
        //GameMessageManager.gameMessageManager.AddLine("", false);
        //GameMessageManager.gameMessageManager.AddLine("", false);
        //GameMessageManager.gameMessageManager.AddLine("", false);

        //while (!GameMessageManager.gameMessageManager.isFinished)
        //{
        //    yield return new WaitForSeconds(0.1f);
        //}

        GameMessageManager.gameMessageManager.ClearText();
        GameMessageManager.gameMessageManager.AddLine(finishText, false, GameMessageManager.Speaker.Host);

        while (!GameMessageManager.gameMessageManager.IsFinished)
        {
            yield return new WaitForSeconds(0.1f);
        }
        
        Globals.GetInstance().SaveSession();

        StartCoroutine(DisplaySaveText());

        yield return new WaitForSeconds(4.0f);

        //gameScale.PlayReverse();

        BarWipe.instance.DoWipe(false);

        yield return new WaitForSeconds(1.25f);

        Globals.GetInstance().LastScene = Globals.GameScene.Battle;
        Application.LoadLevel("outofbattle");
        //end the show, clean up, go back to outofbattle scene

    }

    private void DoStartBattle()
    {

        Debug.Log("Starting Battle");
        Globals.GetInstance().PlayerFinishedLastShow = false;

        currentTurnState = TurnState.StartBattle;
        BattleManager.battleManager.StartNewBattle();

        GameMessageManager.gameMessageManager.AddLine(">> Caller, you're on the Mascu-Line, go ahead.", false, GameMessageManager.Speaker.Host);

        //GameMessageManager.gameMessageManager.AddLine("How to Play: Play cards by dragging them to the play area.", false);
        //GameMessageManager.gameMessageManager.AddLine("After each turn, all cards in play and in hand will be sent to the Discard Pile.", false);
        //GameMessageManager.gameMessageManager.AddLine("When you have to draw a card and there are none in the Draw Pile, Discard Pile will be shuffled to make a new Draw Pile.", false);
        //GameMessageManager.gameMessageManager.AddLine("Spite cards can be played for free to increase Spite.", false);
        //GameMessageManager.gameMessageManager.AddLine("Argument cards use Spite when played. Action cards use Actions when played.", false);
        //GameMessageManager.gameMessageManager.AddLine("Any changes you make to your deck during play by Trashing (removing) or Gaining (adding) cards are permanent, and your deck will persist across battles.", false);
        
        //enemy goes first
        //StartCoroutine(ChangeToStateWhenMessageFinished(TurnState.EnemyTurn, Globals.GetInstance().SHORT_DISPLAY_TIME));
        ChangeState(TurnState.EnemyTurn);
        //ChangeState(TurnState.PlayerDraw);
    }

    private void DoPlayerCleanup()
    {
        if (currentTurnState == TurnState.PlayerActive)
        {
            CardEventManager.cardEventManager.TurnOffProcessing();

            currentTurnState = TurnState.PlayerCleanup;

            DiscardHand();
            CleanupCardsInPlay();

            GameMessageManager.gameMessageManager.ClearText();

            GameMessageManager.gameMessageManager.AddLine(">> " + MRAManager.instance.GetHostArgument(), false,
                GameMessageManager.Speaker.Host);


            ChangeState(TurnState.EnemyTurn);
            //StartCoroutine(ChangeToStateWhenMessageFinished(TurnState.EnemyTurn, Globals.GetInstance().SHORT_DISPLAY_TIME));
            
        }
    }

    private static void CleanupCardsInPlay()
    {
        List<CardController> cardsInPlay = CardZoneManager.cardZoneManager.GetCardsInZone(CardContainer.CardZone.Play);

        UIGrid playGrid = CardZoneManager.cardZoneManager.playContainer.GetComponent<UIGrid>();
        UIGrid discardGrid = CardZoneManager.cardZoneManager.discardContainer.GetComponent<UIGrid>();

        foreach (CardController card in cardsInPlay)
        {
            card.gameObject.transform.parent = CardZoneManager.cardZoneManager.discardContainer.transform;
            card.CurrentZone = CardContainer.CardZone.Discard;

            GameCard gameCard = card.gameCard;
            if (gameCard != null)
            {
                if (gameCard.cardDefinition != null)
                {
                    DeckManager.deckManager.AddCardToDiscard(gameCard.cardDefinition, gameCard.isGainedCard);
                }
            }

            NGUITools.MarkParentAsChanged(card.gameObject);
        }

        if (playGrid != null) { playGrid.repositionNow = true; }
        if (discardGrid != null) { discardGrid.repositionNow = true; }
    }

    private static void DiscardHand()
    {
        List<CardController> cardsInHand = CardZoneManager.cardZoneManager.GetCardsInZone(CardContainer.CardZone.Hand);

        UIGrid handGrid = CardZoneManager.cardZoneManager.handContainer.GetComponent<UIGrid>();
        UIGrid discardGrid = CardZoneManager.cardZoneManager.discardContainer.GetComponent<UIGrid>();

        foreach (CardController card in cardsInHand)
        {
            card.gameObject.transform.parent = CardZoneManager.cardZoneManager.discardContainer.transform;
            card.CurrentZone = CardContainer.CardZone.Discard;

            GameCard gameCard = card.gameCard;
            if (gameCard != null)
            {
                if (gameCard.cardDefinition != null)
                {
                    DeckManager.deckManager.AddCardToDiscard(gameCard.cardDefinition, gameCard.isGainedCard);
                }
            }

            NGUITools.MarkParentAsChanged(card.gameObject);
        }

        if (handGrid != null) { handGrid.repositionNow = true; }
        if (discardGrid != null) { discardGrid.repositionNow = true; }
    }

    private void DoPlayerActive()
    {
        if (currentTurnState == TurnState.PlayerDraw ||
            currentTurnState == TurnState.PlayerInactive)
        {
            currentTurnState = TurnState.PlayerActive;
            CardEventManager.cardEventManager.TurnOnProcessing();

            CardZoneManager.cardZoneManager.CurrentTargetZone = CardContainer.CardZone.Play;
        }
    }

    private void DoPlayerDraw()
    {
        //Debug.Log("DoPlayerDraw()::Enter");
        if (currentTurnState == TurnState.EnemyTurn ||
            currentTurnState == TurnState.StartBattle)
        {
            //Debug.Log("DoPlayerDraw()::CurrentState==" + currentTurnState.ToString());
            if (BattleManager.battleManager.IsPlayerAlive())
            {
                if (!Globals.GetInstance().DoIntroTutorial)
                {
                    Globals.GetInstance().TargetAudioBalance = Globals.SIMPLE_LINES_BALANCE;
                }

                //Debug.Log("DoPlayerDraw()::IsPlayerAlive==true");
                //do player draw phase
                currentTurnState = TurnState.PlayerDraw;
                List<GameObject> cardsDrawn = DrawPileController.drawPile.DrawToFullHand();
                //ChangeState(TurnState.PlayerActive);
                //StartCoroutine(ChangeToStateWhenDisplayEmpty(TurnState.PlayerActive, 1.0f));

                if (cardsDrawn.Count > 0)
                {
                    string cardDrawText = "[Drawing new hand. Drew cards: ";
                    List<string> cardNames = new List<string>();
                    foreach (GameObject card in cardsDrawn)
                    {
                        GameCard gameCard = card.GetComponent<GameCard>();
                        cardNames.Add(gameCard.Title);
                    }
                    cardDrawText += string.Join(", ", cardNames.ToArray()) + "]";

                    //GameMessageManager.gameMessageManager.AddLine(cardDrawText, false);                    
                }
               
                //set a new turn
                RulesManager.rulesManager.ResetTurn();

                StartCoroutine(ChangeToStateWhenDisplayEmpty(TurnState.PlayerActive, Globals.GetInstance().SHORT_DISPLAY_TIME));
            }
            else
            {
                //Debug.Log("DoPlayerDraw()::IsPlayerAlive==false");
                StartCoroutine(DoBattleLost());
            }
        }
    }    

    private void DoPlayerInactive()
    {
        if (currentTurnState == TurnState.PlayerActive)
        {
            currentTurnState = TurnState.PlayerInactive;
        }
    }

    private void DoEnemyTurn()
    {
        if (currentTurnState == TurnState.PlayerCleanup ||
            currentTurnState == TurnState.StartBattle)
        {
            if (BattleManager.battleManager.IsEnemyAlive())
            {
                currentTurnState = TurnState.EnemyTurn;
                EnemyActionManager.enemyActionManager.TakeTurn();
            }
            else
            {
                StartCoroutine(DoBattleWon());
            }    
        }
    }

    private IEnumerator DoBattleWon()
    {
        while (!GameMessageManager.gameMessageManager.IsFinished)
        {
            yield return new WaitForSeconds(0.1f);
        }

        Globals.GetInstance().PlayerBattlesWon++;
        MRAManager.instance.AddAnotherStrangeQuote();

        GameMessageManager.gameMessageManager.AddLine(">> The caller screams in frustration, then the line goes dead.", false, GameMessageManager.Speaker.System);
        GameMessageManager.gameMessageManager.AddLine(">> Ha ha, I accept your defeat, caller!", false, GameMessageManager.Speaker.Host);

        if (BattleManager.battleManager.BattleNumber >= BattleManager.battleManager.BattlesPerShow)
        {
            //we've finished the show

            Globals.GetInstance().PlayerLevel++;

            CardGainController.cardGainController.GainFinishedShowCard();
            Globals.GetInstance().PlayerFinishedLastShow = true;
            StartCoroutine(ChangeToStateWhenDisplayEmpty(TurnState.EndShow, 0.0f));
        }
        else
        {

            CardGainController.cardGainController.GainBattleWonCard();
            StartCoroutine(ChangeToStateWhenDisplayEmpty(TurnState.OutOfBattle, Globals.GetInstance().SHORT_DISPLAY_TIME));
        }
        
    }

    private IEnumerator DoBattleLost()
    {
        while (!GameMessageManager.gameMessageManager.IsFinished)
        {
            yield return new WaitForSeconds(0.1f);
        }

        Globals.GetInstance().PlayerBattlesLost++;
        MRAManager.instance.AddAnotherStrangeQuote();
        //BattleManager.battleManager.PlayerCurrentAnger = BattleManager.battleManager.PlayerMaxAnger;

        GameMessageManager.gameMessageManager.AddLine(">> I CAN'T HANDLE ANY MORE OF THESE IDIOTS TONIGHT!", false, GameMessageManager.Speaker.Host);

        CardGainController.cardGainController.GainBattleLostCard();
        Globals.GetInstance().PlayerFinishedLastShow = false;
        StartCoroutine(ChangeToStateWhenDisplayEmpty(TurnState.EndShow, 0.0f));        
    }

    private IEnumerator DoOutOfBattle()
    {  
        //GameMessageManager.gameMessageManager.AddLine("", false);
        //GameMessageManager.gameMessageManager.AddLine("", false);
        //GameMessageManager.gameMessageManager.AddLine("", false);
        //GameMessageManager.gameMessageManager.AddLine("", false);
        //GameMessageManager.gameMessageManager.AddLine("", false);
        //GameMessageManager.gameMessageManager.AddLine("", false);

        //while (!GameMessageManager.gameMessageManager.isFinished)
        //{
        //    yield return new WaitForSeconds(0.1f);
        //}

        yield return new WaitForSeconds(0.1f);

        GameMessageManager.gameMessageManager.ClearText();
        GameMessageManager.gameMessageManager.AddLine(">> Alright let's take the next caller.", false, GameMessageManager.Speaker.Host);

        currentTurnState = TurnState.OutOfBattle;

        DiscardHand();
        DeckManager.deckManager.ShuffleDiscardIntoDeck();

        Globals.GetInstance().SaveSession();
        StartCoroutine(DisplaySaveText());

        while (!GameMessageManager.gameMessageManager.IsFinished)
        {
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(0.2f);

        ChangeState(TurnManager.TurnState.StartBattle);
    }

    public IEnumerator DisplaySaveText()
    {
        CallInfoHeaderLabel.text = "Saving";

        yield return new WaitForSeconds(0.25f);

        CallInfoHeaderLabel.text += " .";

        yield return new WaitForSeconds(0.25f);

        CallInfoHeaderLabel.text += " .";

        yield return new WaitForSeconds(0.25f);

        CallInfoHeaderLabel.text += " .";

        yield return new WaitForSeconds(0.25f);

        CallInfoHeaderLabel.text = CallInfoHeaderDefaultText;
    }

    public IEnumerator ChangeToStateAfterSeconds(TurnState newState, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        ChangeState(newState);
    }

    public IEnumerator ChangeToStateWhenDisplayEmpty(TurnState newState, float additionalWaitTime)
    {
        Debug.Log("Current State: " + currentTurnState.ToString());
        while (CardDisplayController.cardDisplayController.displayedCards.Count > 0)
        {
            yield return new WaitForSeconds(0.25f);
            Debug.Log("Cards left in display: " + CardDisplayController.cardDisplayController.displayedCards.Count);
        }

        yield return new WaitForSeconds(additionalWaitTime);

        Debug.Log("Changing to State: " + newState.ToString());

        ChangeState(newState);
    }

    public IEnumerator ChangeToStateWhenMessageFinished(TurnState newState, float additionalWaitTime)
    {
        //Debug.Log("Waiting on Message to transition to " + newState.ToString());
        while (!GameMessageManager.gameMessageManager.IsFinished)
        {
            yield return new WaitForSeconds(0.25f);
            //Debug.Log("Cards left in display: " + CardDisplayController.cardDisplayController.displayedCards.Count);
        }

        //Debug.Log("Message finished. Waiting for Additional time: " + additionalWaitTime);

        yield return new WaitForSeconds(additionalWaitTime);

        //Debug.Log("Wait over. Transitioning to new state.");
        //Debug.Log("Changing to Out of Battle State");

        ChangeState(newState);
    }


    public bool CanPlayCards()
    {
        return CurrentState == TurnState.PlayerActive;
    }
}
