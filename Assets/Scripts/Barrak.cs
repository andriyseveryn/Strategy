using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Barrak : MonoBehaviour
{
    public Button player1ToggleButton;
    public Button player2ToggleButton;

    public GameObject player1Menu;
    public GameObject player2Menu;

    GameManager gm;
    void Start()
    {
        gm = GetComponent<GameManager>();
    }
    void Update()
    {
        if (gm.playerTurn == 1)
        {
            player1ToggleButton.interactable = true;
            player2ToggleButton.interactable = false;
        }
        else
        {
            player1ToggleButton.interactable = false;
            player2ToggleButton.interactable = true;
        }
    }
    public void ToggleMenu(GameObject menu)
    {
        menu.SetActive(!menu.activeSelf);
    }
    public void CloseMenu()
    {
        player1Menu.SetActive(false);
        player2Menu.SetActive(false);
    }
    public void BuyItem(BarrakItem item)
    {
        if (gm.playerTurn==1 && item.cost<= gm.player1Gold)
        {
            gm.player1Gold -= item.cost;
            player1Menu.SetActive(false);
        }
        else if (gm.playerTurn == 2 && item.cost <= gm.player2Gold)
        {
            gm.player2Gold -= item.cost;
            player2Menu.SetActive(false);
        }
        else
        {
            print("NOT ENOUGH GOLD");
            return;
        }
        gm.UpdateGoldText();
        gm.purchasedItem = item;
        if (gm.selectedUnits != null)
        {
            gm.selectedUnits.selected = false;
            gm.selectedUnits = null;
        }
        GetCreatibleTiles();
    }
    void GetCreatibleTiles()
    {
        foreach (Tiles tile in FindObjectsOfType<Tiles>())
        {
            if (tile.isClear())
            {
                tile.SetCreatable();
            }
        }
    }
}
