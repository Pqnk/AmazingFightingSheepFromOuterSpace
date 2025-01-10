using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum KeyLanguage
{
    Bienvenue,
    Langue,
    Valider,
    SauvegardeAuto,
    Rejoindre,
    DebutIci,
    Bouger,
    Frapper,
    Sauter,
    Objectif
}


public class LangageManager : MonoBehaviour
{
    public string currentLanguage = "En";
    public KeyLanguage keyLanguage;
    private Dictionary<KeyLanguage, string> localizedTexts;

    private void Awake()
    {
        LoadLocalizedTexts();
    }

    private void LoadLocalizedTexts()
    {
        localizedTexts = new Dictionary<KeyLanguage, string>();

        switch (currentLanguage)
        {
            case "En":
                localizedTexts.Add(KeyLanguage.Bienvenue, "Welcome");
                localizedTexts.Add(KeyLanguage.Langue, "Language");
                localizedTexts.Add(KeyLanguage.Valider, "Validate");
                localizedTexts.Add(KeyLanguage.SauvegardeAuto, "This game has an automatic save system.\r\nWhen this icon appears, do not turn off the game.\r\n\r\n Two players, controller required!");
                localizedTexts.Add(KeyLanguage.Rejoindre, "Press any button to join.");
                localizedTexts.Add(KeyLanguage.DebutIci, "Start here.");
                localizedTexts.Add(KeyLanguage.Bouger, "Move");
                localizedTexts.Add(KeyLanguage.Frapper, "Hit");
                localizedTexts.Add(KeyLanguage.Sauter, "Jump");
                localizedTexts.Add(KeyLanguage.Objectif, "Be the first to reach the spaceship!");
                break;

            case "Fr":
                localizedTexts.Add(KeyLanguage.Bienvenue, "Bienvenue");
                localizedTexts.Add(KeyLanguage.Langue, "Langue");
                localizedTexts.Add(KeyLanguage.Valider, "Valider");
                localizedTexts.Add(KeyLanguage.SauvegardeAuto, "Ce jeu possède un systeme de sauvegarde automatique des scores.\r\nLors que cette icone apparait n'éteignez pas le jeu.\r\n\r\nDeux joueurs, manette obligatoire !");
                localizedTexts.Add(KeyLanguage.Rejoindre, "Appuyez sur n'importe quel bouton pour rejoindre.");
                localizedTexts.Add(KeyLanguage.DebutIci, "Commencez ici.");
                localizedTexts.Add(KeyLanguage.Bouger, "Bouger");
                localizedTexts.Add(KeyLanguage.Frapper, "Frapper");
                localizedTexts.Add(KeyLanguage.Sauter, "Sauter");
                localizedTexts.Add(KeyLanguage.Objectif, "Soyez le premier à atteindre le vaisseau!");
                break;

            case "De":
                localizedTexts.Add(KeyLanguage.Bienvenue, "Willkommen");
                localizedTexts.Add(KeyLanguage.Langue, "Sprache");
                localizedTexts.Add(KeyLanguage.Valider, "Validierung");
                localizedTexts.Add(KeyLanguage.SauvegardeAuto, "Dieses Spiel verfügt über ein automatisches Speichersystem.\r\nWenn dieses Symbol erscheint, schalten Sie das Spiel nicht aus.\r\n\r\n Zwei Spieler, Controller erforderlich!");
                localizedTexts.Add(KeyLanguage.Rejoindre, "Drücken Sie eine beliebige Taste, um beizutreten.");
                localizedTexts.Add(KeyLanguage.DebutIci, "Beginnen Sie hier.");
                localizedTexts.Add(KeyLanguage.Bouger, "Bewegen");
                localizedTexts.Add(KeyLanguage.Frapper, "Schlag");
                localizedTexts.Add(KeyLanguage.Sauter, "Springen");
                localizedTexts.Add(KeyLanguage.Objectif, "Seien Sie der Erste, der das Raumfahrzeug erreicht!");
                break;

            case "Es":
                localizedTexts.Add(KeyLanguage.Bienvenue, "Bienvenido");
                localizedTexts.Add(KeyLanguage.Langue, "Idioma");
                localizedTexts.Add(KeyLanguage.Valider, "Validar");
                localizedTexts.Add(KeyLanguage.SauvegardeAuto, "Este juego tiene un sistema de guardado automático.\r\nCuando aparezca este icono, no apagues el juego.\r\n\r\n ¡Dos jugadores, se requiere un controlador!");
                localizedTexts.Add(KeyLanguage.Rejoindre, "Presione cualquier botón para unirse.");
                localizedTexts.Add(KeyLanguage.DebutIci, "Comience aquí.");
                localizedTexts.Add(KeyLanguage.Bouger, "Movimiento");
                localizedTexts.Add(KeyLanguage.Frapper, "Golpear");
                localizedTexts.Add(KeyLanguage.Sauter, "Saltar");
                localizedTexts.Add(KeyLanguage.Objectif, "Sé el primero en llegar a la nave espacial.");
                break;
        }
    }
    
    public string GetLocalizedText(KeyLanguage key)
    {
        if (localizedTexts.ContainsKey(key))
        {
            return localizedTexts[key];
        }
        else
        {
            return "Erreur, la key n'existe pas !";
        }
    }

    public void ChangeLanguage(string newLanguage)
    {
        currentLanguage = newLanguage;
        LoadLocalizedTexts();
    }
}
