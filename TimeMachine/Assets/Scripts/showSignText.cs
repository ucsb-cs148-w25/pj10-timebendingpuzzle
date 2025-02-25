using UnityEngine;
using TMPro; // If you're using TextMeshPro

public class SignPostScript : MonoBehaviour
{
    public GameObject signText; // Reference to the text UI
    public string message; // The story message

    private void Start()
    {
        signText.SetActive(false); // Hide text initially
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // signText.GetComponent<TextMeshProUGUI>().text = message; // Update the message
            signText.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            signText.SetActive(false); // Hide text when player leaves
        }
    }
}

