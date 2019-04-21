using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialTextController : MonoBehaviour
{
    [SerializeField]
    private Text _tutText;
    [SerializeField]
    private int _tutTextPlaceHolder = 0;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(_tutTextPlaceHolder == 0)
        {
            _tutText.text = "Slowly move the camera around in order to detect planes. \n Once detected they will be shown by a coloured mesh";
        }
        else if(_tutTextPlaceHolder == 1)
        {
            _tutText.text = "The game scales according to the size of the plane. \n click on a plane's mesh to select it as the plane to play on";
        }
        else if (_tutTextPlaceHolder == 2)
        {
            _tutText.text = "Tap again to spawn your character. \n The character can be controlled using the joystick and X/Y buttons";
        }
        else if (_tutTextPlaceHolder == 3)
        {
            _tutText.enabled = false;
        }
    }

    public void IncrementTutText()
    {
        _tutTextPlaceHolder += 1;
    }
}
