using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Unit selectedUnits;
    public Unit viewUnit;
    public int playerTurn = 1;

    public GameObject selectedSquare;
    public GameObject statsPanel;

    public Vector2 statsPanelShift;

    public Image playerIndicator;
    public Sprite player1Indicator;
    public Sprite player2Indicator;

    public Text player1GoldText, player2GoldText;
    public Text healthText, armorText, attackDamageText, defenseDamageText;

    public int player1Gold=100,player2Gold=100;

    public BarrakItem purchasedItem;
    void Start()
    {
        GetGoldIncome(1);
    }
    public void ToggleStatesPanel(Unit unit)
    {

    }
    public void GetGoldIncome(int playerTurn)
    {
        foreach (Village village in FindObjectsOfType<Village>())
        {
            if (village.playerNumber == playerTurn)
            {
                if (playerTurn == 1)
                {
                    player1Gold += village.goldPerTurn;
                }
                else
                    player2Gold += village.goldPerTurn;
            }
        }
        UpdateGoldText();
    }
    public void UpdateGoldText()
    {
        player1GoldText.text = player1Gold.ToString();
        player2GoldText.text = player2Gold.ToString();
    }
    public void ResetTiles()
    {
        foreach (Tiles tile in FindObjectsOfType<Tiles>())
        {
            tile.Reset();
        }
    }
    private void Update()
    {
        if (selectedUnits != null)
        {
            selectedSquare.SetActive(true);
            selectedSquare.transform.position = selectedUnits.transform.position;
        }
        else
        {
            selectedSquare.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            EndTurn();
        }
    }
    void EndTurn()
    {
        if (playerTurn == 1)
        {
            playerTurn = 2;
            playerIndicator.sprite = player2Indicator;
        }
        else if(playerTurn == 2)
        {
            playerTurn = 1;
            playerIndicator.sprite = player1Indicator;
        }
        GetGoldIncome(playerTurn);
        if (selectedUnits != null)
        {
            selectedUnits.selected = false;
            selectedUnits = null;
        }
        ResetTiles();
        foreach (Unit units in FindObjectsOfType<Unit>())
        {
            units.hasMoved = false;
            units.weaponIcon.SetActive(false);
            units.hasAttacked = false;
        }
    }
}
