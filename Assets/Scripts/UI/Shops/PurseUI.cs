using RPG.Inventories;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace RPG.UI
{
    public class PurseUI : MonoBehaviour
    {
        // configs
        [SerializeField] private TextMeshProUGUI balanceField;

        // state
        GameObject player;
        Purse playerPurse;

        void Awake()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            playerPurse = player.GetComponent<Purse>();
        }

        void OnEnable()
        {
            if (playerPurse != null)
            {
                playerPurse.OnChange += RefreshUI;
            }            
        }

        void OnDisable()
        {
            if (playerPurse != null)
            {
                playerPurse.OnChange -= RefreshUI;
            }
        }

        void Start()
        {
            RefreshUI();
        }

        void RefreshUI()
        {
            balanceField.text = $"${playerPurse.GetBalance():N2}";
        }
    }
}
