using UnityEngine;
using System.Collections;

public class ButtonHandler
{
    public enum ButtonState { Normal, Scaled };
    public ButtonState _state; // the state of the button

    public Rect _normalSize; // stores the normal size of the button
    public Rect _scaledSize; // stores the scaled size of the button

    public float _scale; // how much to scale the button by
    public string _callback; // a link to the callback function that is executed to trigger animation
    public GameObject _gameObject; // gameObject that called the function

    public ButtonHandler(Rect normal, GameObject gameObject, float scale, string callback)
    {
        // Initialize the values
        _normalSize = normal;
        _scale = scale;
        _state = ButtonState.Normal;
        _gameObject = gameObject;
        _callback = callback;

        float scaledBtnWidth = normal.width * scale;
        float scaledBtnHeight = normal.height * scale;
        float scaledBtnXOffset = normal.x + ((normal.width - scaledBtnWidth) / 2);
        float scaledBtnYOffset = normal.y + ((normal.height - scaledBtnHeight) / 2);
        _scaledSize = new Rect(scaledBtnXOffset, scaledBtnYOffset, scaledBtnWidth, scaledBtnHeight);
    }

    public void UpdateLocation(Rect normal)
    {
        _normalSize = normal;

        float scaledBtnWidth = normal.width * _scale;
        float scaledBtnHeight = normal.height * _scale;
        float scaledBtnXOffset = normal.x + ((normal.width - scaledBtnWidth) / 2);
        float scaledBtnYOffset = normal.y + ((normal.height - scaledBtnHeight) / 2);
        _scaledSize = new Rect(scaledBtnXOffset, scaledBtnYOffset, scaledBtnWidth, scaledBtnHeight);
    }

    public void OnMouseOver(Rect buttonRect)
    {
        if (_normalSize.Contains(Event.current.mousePosition) &&
            _state == ButtonState.Normal)
        {
            //iTween.Stop(_gameObject, "value");
            _state = ButtonState.Scaled;
            iTween.ValueTo(_gameObject, iTween.Hash("from", buttonRect, "to", _scaledSize, "easetype", iTween.EaseType.easeOutBack, "onupdate", _callback, "time", 0));
        }

        else if (!_normalSize.Contains(Event.current.mousePosition) &&
            _state == ButtonState.Scaled)
        {
            //iTween.Stop(_gameObject, "value");
            _state = ButtonState.Normal;
            iTween.ValueTo(_gameObject, iTween.Hash("from", buttonRect, "to", _normalSize, "easetype", iTween.EaseType.easeOutExpo, "onupdate", _callback, "time", 0));
        }

    }
}
