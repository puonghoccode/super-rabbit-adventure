using System;
using UnityEngine;

public static class PlayerWallet
{
    public static event Action<int> CoinsChanged;

    private const string CoinsKey = "total_coins";

    public static int Coins => PlayerPrefs.GetInt(CoinsKey, 0);

    public static void AddCoins(int amount)
    {
        if (amount <= 0)
        {
            return;
        }

        int next = Coins + amount;
        SetCoins(next);
    }

    public static bool TrySpendCoins(int amount)
    {
        if (amount <= 0)
        {
            return true;
        }

        int current = Coins;
        if (current < amount)
        {
            return false;
        }

        SetCoins(current - amount);
        return true;
    }

    public static void SetCoins(int amount)
    {
        int clamped = Mathf.Max(0, amount);
        PlayerPrefs.SetInt(CoinsKey, clamped);
        PlayerPrefs.Save();
        CoinsChanged?.Invoke(clamped);
    }
}
