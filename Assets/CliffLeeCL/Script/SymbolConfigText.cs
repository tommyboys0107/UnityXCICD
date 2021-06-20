using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SymbolConfigText : MonoBehaviour
{
    [SerializeField]
    Text symbolConfigText = null;

    void Start()
    {
#if Debug
        symbolConfigText.text = "Debug";
#elif Release
        symbolConfigText.text = "Release";
#endif
    }
}
