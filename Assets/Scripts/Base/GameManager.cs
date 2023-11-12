using System;
using System.Collections;
using System.Collections.Generic;
using Alteruna;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public ulong DISCORDID { get; private set; }
    public bool LoginIn { get; private set; }
    public string USERNAME { get; private set; }
    
    private Multiplayer _multiplayer;

    private void FixedUpdate()
    {
        // Поиск Network Manager для манипуляций
        _multiplayer = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<Multiplayer>();
    }

    public void AuthorizeDiscord(ulong id, string username)
    {
        if (LoginIn) return;
        
        // Инициализация
        DISCORDID = id;
        USERNAME = username;
        LoginIn = true;
        
        // Вставка Никнейма
        _multiplayer.enabled = true;
        _multiplayer.SetUsername(username);
    }
}
