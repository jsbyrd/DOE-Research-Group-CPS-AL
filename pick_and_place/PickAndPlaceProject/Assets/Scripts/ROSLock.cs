using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ROSLock : MonoBehaviour
{
    private bool rosLock;

    // Start is called before the first frame update
    void Start()
    {
        rosLock = false;
    }

    public void Lock() {
        rosLock = true;
    }

    public void Unlock() {
        rosLock = false;
    }

    public bool IsLocked() {
        return rosLock;
    }

}
