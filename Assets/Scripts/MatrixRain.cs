using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class MatrixRain : MonoBehaviour {

    private MatrixStream s;
    private GUIStyle style;
    private GUIStyle styleFirst;
    private int nbColumn;
    private List<MatrixStream> streams;

    private Vector2 resolutionSave;

    private float speedMin;
    private float speedMax;

    // Use this for initialization
    void Start () {

        resolutionSave = new Vector2( Screen.width, Screen.height );
        nbColumn = 35;

        speedMin = 0.5f;
        speedMax = 5.0f;

        style = new GUIStyle();
        style.normal.textColor = Color.green;
        style.fontSize = Screen.width / nbColumn;

        styleFirst = new GUIStyle();
        styleFirst.normal.textColor = new Color( 0.8f, 1f, 0.8f );
        styleFirst.fontSize = Screen.width / nbColumn;

        streams = new List<MatrixStream>();

        ResetAll();
    }
	
	// Update is called once per frame
	void Update () {

        if (resolutionSave != new Vector2(Screen.width, Screen.height)) {

            int newSize = Screen.width / nbColumn;

            styleFirst.fontSize = newSize;
            style.fontSize = newSize;

            foreach (MatrixStream ms in streams) {

                ms.SetSize(newSize);
            }

            resolutionSave = new Vector2(Screen.width, Screen.height);
        }
    }

    private void OnGUI() {

        foreach( MatrixStream ms in streams) {

            ms.Update(Time.deltaTime, Screen.width / nbColumn );

            for (int i = 0; i < ms.symbols.Count; i++) {

                if ( i == ms.symbols.Count - 1 && ms.colorFirstSymbol ) GUI.Label(new Rect(ms.symbols[i].x, ms.symbols[i].y, 0, 0), ms.symbols[i].symbol.ToString(), styleFirst);
                else GUI.Label(new Rect(ms.symbols[i].x, ms.symbols[i].y, 0, 0), ms.symbols[i].symbol.ToString(), style);
            }
        }
    }

    private void ResetAll() {

        streams.Clear();

        for (int i = 0; i < nbColumn; i++) {

            MatrixStream sTemp = new MatrixStream(i, Screen.width / nbColumn, speedMin, speedMax);
            streams.Add(sTemp);
        }
    }
}

public class MatrixStream {

    float speed;
    int size;
    int indiceX;
    float x;
    float y;
    int nbSymbols;

    public bool colorFirstSymbol;
    public List<MatrixSymbol> symbols;

    public void SetSize(int _size) {

        size = _size;
        CalculX();
    }

    public void CalculX() {

        x = indiceX * size;
    }

    public MatrixStream(int _indiceX, int _size, float speedMin, float speedMax) {

        indiceX = _indiceX;

        SetSize(_size);

        speed = UnityEngine.Random.Range(speedMin, speedMax);

        colorFirstSymbol = UnityEngine.Random.Range(0, 2) == 1;

        symbols = new List<MatrixSymbol>();

        nbSymbols = UnityEngine.Random.Range(3, (Screen.height / size) - 1);

        CalculX();
        y = UnityEngine.Random.Range(0, Screen.height - (nbSymbols * size));

        ResetSymbols();
    }

    public void Update(float timeSpent, int _size) {

        y = ( y + speed > Screen.height )? y - Screen.height : y + speed;
        x = indiceX * size;

        for (int i = 0; i < symbols.Count; i++) {

            symbols[i].Update(timeSpent);
            symbols[i].SetPosition(x, y + i*size );
        }
    }

    public void ResetSymbols() {

        symbols.Clear();

        for (int i = 0; i < nbSymbols; i++) {

            MatrixSymbol sTemp = new MatrixSymbol(x, i * y);
            symbols.Add(sTemp);
        }
    }
}

public class MatrixSymbol {

    public GUIStyle style;
    public char symbol;
    public float switchInterval;
    public float y;
    public float x;

    public float time;

    public MatrixSymbol(float _x, float _y) {

        SetPosition(_x, _y);

        switchInterval = UnityEngine.Random.Range( 2, 15 );
        ResetToRandomSymbol();
    }

    public void ResetToRandomSymbol() {

        symbol = (char)UnityEngine.Random.Range( 0x30A0 + 0, 0x30A0 + 95);
    }

    public void Update(float _timeSpent) {

        time += _timeSpent;

        if ( time >= switchInterval) {

            time -= switchInterval;
            ResetToRandomSymbol();
        }
    }

    public void SetPosition( float _x, float _y) {

        x = _x;
        y = _y;

        if ( y > Screen.height) { y -= Screen.height; }
    }
}
