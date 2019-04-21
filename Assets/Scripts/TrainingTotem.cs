using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;
using UnityEngine.UI;

public class TrainingTotem : MonoBehaviour
{
    [SerializeField]
    private int _hitPoints = 100;//int.MaxValue;
    //private int _armour = 100;
    [SerializeField]
    private GameObject _centerOfMass;
    [SerializeField]
    private Image _healthBar;

    //Scaling Variables
    private GameManager _gm;


    private int maxHitPoints;
    private Text hptext;
    // Start is called before the first frame update
    void Start()
    {
        maxHitPoints = _hitPoints;
        _gm = FindObjectOfType<GameManager>();

        this.gameObject.transform.localScale = new Vector3(_gm.GetArenaScale(), _gm.GetArenaScale(), _gm.GetArenaScale()) * 0.05f;
        //hptext = GameObject.Find("Joystick Status").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        float hpbarFill = (float)_hitPoints / (float)maxHitPoints;
        _healthBar.fillAmount = hpbarFill;
        //hptext.text = "Totem hpbar fill: " + hpbarFill;
    }

    public void TakeDamage(int damage)
    {
        _hitPoints -= damage;
        this.transform.GetComponent<Renderer>().material.color = Color.red;
    }

    public Vector3 GetCenterOfMass()
    {
        return _centerOfMass.transform.position;
    }
}
