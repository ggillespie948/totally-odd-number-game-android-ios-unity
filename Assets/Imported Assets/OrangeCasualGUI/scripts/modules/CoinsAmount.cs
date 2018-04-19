using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CoinsAmount : MonoBehaviour {

    public Text label;

    public float speed = 2f;
    public int amount;

    private int _currentAmount = 0;

    void Update() {
        if(_currentAmount != amount) {
            int add = Mathf.CeilToInt((amount - _currentAmount) * Time.deltaTime * speed);
            if(Mathf.Abs(_currentAmount - amount) <= add*1.5) {
                _currentAmount = amount;
            }
            else {
                _currentAmount += add;
            }
            label.text = _currentAmount + "";
        }
    }
}
