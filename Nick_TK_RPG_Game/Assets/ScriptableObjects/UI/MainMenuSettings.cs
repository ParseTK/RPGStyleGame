using UnityEngine;

[CreateAssetMenu(fileName = "MainMenuSettings", menuName = "UI/Main Menu Settings")]
public class MainMenuSettings : ScriptableObject
{
    [Header("Scene Configuration")]
    public string gameplayScene = "Lobby";
}