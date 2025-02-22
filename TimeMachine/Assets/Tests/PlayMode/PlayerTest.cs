using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class PlayerTest
{
    private GameObject player;

    [SetUp]
    public void Setup()
    {
        SceneManager.LoadScene("TestScene");
    }

    [UnityTest]
    public IEnumerator Player_SpawnsCorrectly()
    {
        yield return new WaitForSeconds(1); // Wait for scene to load

        // Find the player
        player = GameObject.FindWithTag("Player");
        Assert.IsNotNull(player, "Player object not found.");
    }
}
