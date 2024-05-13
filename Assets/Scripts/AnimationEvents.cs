using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    public void activeFalse()
    {
        gameObject.SetActive(false);
    }

    public void activeTrue()
    {
        gameObject.SetActive(true);
    }
}
