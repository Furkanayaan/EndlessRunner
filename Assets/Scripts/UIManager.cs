using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    public static UIManager I;
    public TMP_Text levelCount;
    public TMP_Text goldCount;
    public GameObject youDie;
    public Button restart;
    void Start() {
        I = this;
        restart.onClick.AddListener(() => {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        });
    }

    // Update is called once per frame
    void Update() {
        levelCount.text = "Level Count: " + CharacterMovement.I.CurrentLevel();
        goldCount.text = "Gold: " + CharacterMovement.I.CurrentGold();
    }
}
