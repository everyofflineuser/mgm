using System;
using System.Collections;
using System.Collections.Generic;
using Alteruna;
using UnityEngine;
using ShadowGroveGames.LoginWithDiscord.Scripts;
using User = ShadowGroveGames.LoginWithDiscord.Scripts.Communication.DTO.User;

public class AuthInDiscord : MonoBehaviour
{
    private GameManager manager;
    private void Start()
    {
        // Присваивание переменных и открытие страницы с авторизацией
        manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        OpenLoginPage();
    }

    private void OpenLoginPage()
    {
        Debug.Log(LoginWithDiscordScript.Instance.GenerateDiscordOAuthUrl()); // Дебаг
        Application.OpenURL(LoginWithDiscordScript.Instance.GenerateDiscordOAuthUrl());
    }

    public void OnLoginSuccess()
    {
        // Авторизация
        User? user = LoginWithDiscordScript.Instance.GetUser();
        manager.AuthorizeDiscord(user.Value.Id, user.Value.Username);
    }
}
