using UnityEngine;

public class Card : MonoBehaviour
{
    public GameObject front;
    public GameObject back;

    public void FlipToBack()
    {
        front.SetActive(false);
        back.SetActive(true);
    }

    public void FlipToFront()
    {
        front.SetActive(true);
        back.SetActive(false);
    }
}