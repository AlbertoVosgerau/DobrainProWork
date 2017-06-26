using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SaveDataEditor {

    [MenuItem("Dobrain/Clear Save Data")]
    public static void Clear()
    {
        PlayerPrefs.DeleteAll();
    }

}
