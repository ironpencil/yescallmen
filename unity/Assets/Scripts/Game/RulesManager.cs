using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class RulesManager : MonoBehaviour {

    public static RulesManager rulesManager;

    public UILabel spiteTotalLabel;
    public UILabel spiteLeftLabel;
    public UILabel actionsLeftLabel;

    private int spiteTotal = 0;
    public int SpiteTotal
    {
        get
        {
            return spiteTotal;
        }
        set
        {
            spiteTotal = value;
            spiteTotalLabel.text = SpiteTotal.ToString();
        }
    }

    private int spiteLeft = 0;
    public int SpiteLeft
    {
        get
        {
            return spiteLeft;
        }
        set
        {
            spiteLeft = value;
            spiteLeftLabel.text = SpiteLeft.ToString();
        }
    }

    private int actionsLeft = 1;
    public int ActionsLeft
    {
        get
        {
            return actionsLeft;
        }
        set
        {
            actionsLeft = value;
            actionsLeftLabel.text = ActionsLeft.ToString();
        }
    }

	// Use this for initialization
	void Start () {

        rulesManager = this;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public bool CanPlayCard(GameObject card)
    {
        GameCard gameCard = card.GetComponent<GameCard>();

        if (gameCard == null) { return false; }

        Debug.Log("CanPlayCard: " + gameCard.Title);

        if (gameCard.cardType == GameCard.CardType.Action && ActionsLeft == 0)
        {
            GameMessageManager.gameMessageManager.AddLine("You do not have enough Actions to play " + gameCard.Title + ".", false);
            return false;
        }

        if (gameCard.spiteUsed > SpiteLeft) {
            GameMessageManager.gameMessageManager.AddLine("You do not have enough Spite to play " + gameCard.Title + ".", false); 
            return false;
        }

        Debug.Log("CanPlayCard = true");
        return true;
    }

    //public void ReparentAndPlayCard(GameObject cardObject, GameObject newParent)
    //{
    //    UIGrid currentGrid = cardObject.transform.parent.gameObject.GetComponent<UIGrid>();

    //    cardObject.transform.parent = newParent.transform;

    //    UIGrid destinationGrid = newParent.GetComponent<UIGrid>();

    //    if (destinationGrid != null)
    //    {
    //        destinationGrid.repositionNow = true;
    //    }

    //    if (currentGrid != null)
    //    {
    //        currentGrid.repositionNow = true;
    //    }

    //    //have to enable/re-enable the scroll view to get it to update to the new parent
    //    UIDragScrollView dragScrollView = cardObject.GetComponent<UIDragScrollView>();

    //    if (dragScrollView != null)
    //    {
    //        //dragScrollView.scrollView = handScrollView;
    //        dragScrollView.enabled = false;
    //        dragScrollView.enabled = true;
    //    }

    //    NGUITools.MarkParentAsChanged(cardObject);

    //    PlayCard(cardObject);
    //}

    public void PlayCard(GameObject cardObject)
    {
        CardController cardController = cardObject.GetComponent<CardController>();

        if (cardController == null) { return; }

        GameCard gameCard = cardController.gameCard;

        if (gameCard == null) { return; }

        StringBuilder sb = new StringBuilder("Playing " + gameCard.Title + ". ");

        if (gameCard.cardType == GameCard.CardType.Action)
        {
            ActionsLeft--;
            sb.Append("-1 Action ");
        }

        if (gameCard.actionsAdded > 0)
        {
            ActionsLeft += gameCard.actionsAdded;
            sb.Append("+" + gameCard.actionsAdded + " Actions ");
        }

        if (gameCard.spiteUsed > 0)
        {
            SpiteLeft -= gameCard.spiteUsed;
            sb.Append("-" + gameCard.spiteUsed + " Spite ");               
        }

        if (gameCard.spiteAdded > 0)
        {
            SpiteTotal += gameCard.spiteAdded;
            SpiteLeft += gameCard.spiteAdded;
            sb.Append("+" + gameCard.spiteAdded + " Spite ");     
        }

        GameMessageManager.gameMessageManager.AddLine(sb.ToString(), false);
        
        //cardController.UpdateCurrentZone();

        CardEventManager.cardEventManager.QueueEvents(cardObject);

    }

    public void ResetTurn()
    {
        ActionsLeft = 1;
        SpiteTotal = 0;
        SpiteLeft = 0;
    }
}
