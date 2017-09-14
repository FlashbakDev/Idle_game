using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighInteger {

    [SerializeField] private static string[] suffixes = { "", "mille", "million", "milliard", "billion", "billiard", "trillion", "trilliard", "quadrillion", "quadrilliard", "quintillion", "quintilliard", "sextillion", "sextilliard", "septillion", "septilliard", "octillion", "octilliard", "nonillion", "nonilliard", "decillion", "decilliard" };

    [SerializeField] private string number;

    // Constructeurs ===================================================================================================

    public HighInteger() {

        number = "";
    }

    public HighInteger(int n) {

        number = n.ToString();
    }

    public HighInteger(string s) {

        number = s;
    }

    public HighInteger(HighInteger n) {

        number = n.HighIntegerNumber;
    }

    // Méthodes ===================================================================================================

    public static HighInteger NoZero(HighInteger n) {

        if (n.GetLength() <= 1)
            return n;

        if (n.HighIntegerNumber[0] != '0')
            return n;

        for (int i = 0; i < n.GetLength(); i++) {

            if (n.HighIntegerNumber[i] != '0')
                return new HighInteger( n.HighIntegerNumber.Substring(i, n.GetLength() - i) );
        }

        return n;
    }

    public static void FillWithZero(ref HighInteger n1, ref HighInteger n2) {

        if (n1.GetLength() == n2.GetLength())
            return;

        HighInteger mTemp = new HighInteger("");

        if (n1.GetLength() < n2.GetLength()) {

            mTemp = new HighInteger( n1 );

            for (int i = mTemp.GetLength(); i < n2.GetLength(); i++) {

                mTemp = new HighInteger( "0" + mTemp.HighIntegerNumber );
            }

            n1 = new HighInteger( mTemp );
        }
        else {

            mTemp = new HighInteger( n2 );

            for (int i = mTemp.GetLength(); i < n1.GetLength(); i++) {

                mTemp = new HighInteger("0" + mTemp.HighIntegerNumber);
            }

            n2 = new HighInteger( mTemp );
        }
    }

    public int GetLength() { return number.Length; }

    public override bool Equals(object obj) {

        var item = obj as HighInteger;

        if (item == null) {
            return false;
        }

        return this.number.Equals(item.number);
    }

    public override int GetHashCode() {

        return this.number.GetHashCode();
    }

    public override string ToString() {

        string resultat = "";

        string suffix = ((GetLength() - 1) / 3 < suffixes.Length )?  suffixes[ (GetLength() - 1)/3 ]: "???";
        if (suffix != suffixes[0] && ( GetLength()%3 != 1 || number[0] != '1') ) suffix = suffix + "s";

        for (int i = 0; i < GetLength(); i ++ ) {

            if ( i%3 == 0 && i != 0 ) { resultat = "," + resultat; }

            resultat = number[GetLength() - i - 1] + resultat;
        }

        if ( resultat.IndexOf(',') != -1 ) {

            resultat = resultat.Substring( 0, resultat.IndexOf(',') + 4 );
        }

        return resultat + " " + suffix;
    }

    // Surcharge d'opérateurs ===================================================================================================

    public static HighInteger operator +(HighInteger n1, HighInteger n2) {

        //Debug.Log("HighInteger - operator +( HighInteger n1 = " + n1.HighIntegerNumber + ", HighInteger n2 = " + n2.HighIntegerNumber + "  ) : Start ");

        HighInteger resultat = new HighInteger("");
        int reste = 0;

        FillWithZero(ref n1, ref n2);

        //Debug.Log("HighInteger - operator +(...) : n1 = "+ n1.HighIntegerNumber + ", n2 = "+ n2.HighIntegerNumber );

        for (int i = 0; i < n1.GetLength(); i++) {

            int indice = n1.GetLength() - 1 - i;

            int sum = Convert.ToInt32(n1.HighIntegerNumber[indice].ToString(), 10) + Convert.ToInt32(n2.HighIntegerNumber[indice].ToString(), 10) + reste;

            //Debug.Log("GameManager - sum = " + sum +" ( c1 = "+ Convert.ToInt32(n1[indice].ToString(), 10) + ", c2 = "+ Convert.ToInt32(n2[indice].ToString(), 10) + ", reste = "+ reste );

            reste = sum / 10;

            resultat = new HighInteger( (sum % 10).ToString() + resultat.HighIntegerNumber );
        }

        if (reste > 0) { resultat = new HighInteger( reste.ToString() + resultat.HighIntegerNumber ); }

        //Debug.Log("HighInteger - operator +() : end ");

        return new HighInteger( resultat );
    }

    public static HighInteger operator +(HighInteger n1, int i1) {

        return new HighInteger( n1 + new HighInteger(i1) );
    }

    public static HighInteger operator -(HighInteger n1, HighInteger n2) {

        HighInteger resultat = new HighInteger("");
        int reste = 0;

        FillWithZero(ref n1, ref n2);

        for (int i = 0; i < n1.GetLength(); i++) {

            int indice = n1.GetLength() - 1 - i;

            int nb1 = Convert.ToInt32(n1.HighIntegerNumber[indice].ToString(), 10);
            int nb2 = Convert.ToInt32(n2.HighIntegerNumber[indice].ToString(), 10) + reste;

            if (nb1 < nb2) {

                nb1 += 10;
                reste = 1;
            }
            else {

                reste = 0;
            }

            int sum = nb1 - nb2;

            //Debug.Log("GameManager - Minus() : sum = " + sum +" ( c1 = "+ nb1.ToString() + ", c2 = "+ nb2.ToString() + ", reste = "+ reste );

            resultat = new HighInteger( sum.ToString() + resultat.HighIntegerNumber );
        }

        if (reste > 0) { resultat = new HighInteger(); }

        resultat = new HighInteger( NoZero(resultat) );

        //Debug.Log("GameManager - Minus() : end, res = " + resultat);

        return new HighInteger( resultat );
    }

    public static HighInteger operator -(HighInteger n1, int i1) {

        return new HighInteger(n1 - new HighInteger(i1));
    }

    public static bool operator <(HighInteger n1, HighInteger n2) {

        if (n1.GetLength() == n2.GetLength()) {

            for (int i = 0; i < n1.GetLength(); i++) {

                if (Convert.ToInt32(n1.HighIntegerNumber[i].ToString(), 10) != Convert.ToInt32(n2.HighIntegerNumber[i].ToString(), 10)) {

                    return Convert.ToInt32(n1.HighIntegerNumber[i].ToString(), 10) < Convert.ToInt32(n2.HighIntegerNumber[i].ToString(), 10);
                }
            }
        }

        return n1.GetLength() < n2.GetLength();
    }

    public static bool operator <(HighInteger n1, int i1) {

        return n1 < new HighInteger(i1);
    }

    public static bool operator >(HighInteger n1, HighInteger n2) {

        if (n1.GetLength() == n2.GetLength()) {

            for (int i = 0; i < n1.GetLength(); i++) {

                if ( Convert.ToInt32( n1.HighIntegerNumber[i].ToString(), 10 ) != Convert.ToInt32(n2.HighIntegerNumber[i].ToString(), 10)) {

                    return Convert.ToInt32(n1.HighIntegerNumber[i].ToString(), 10) > Convert.ToInt32(n2.HighIntegerNumber[i].ToString(), 10);
                }
            }
        }

        return n1.GetLength() > n2.GetLength();
    }

    public static bool operator >(HighInteger n1, int i1) {

        return n1 > new HighInteger(i1);
    }

    public static bool operator ==(HighInteger n1, HighInteger n2) {

        return n1.HighIntegerNumber.Equals(n2.HighIntegerNumber);
    }

    public static bool operator ==(HighInteger n1, int i1) {

        return n1 == new HighInteger(i1);
    }

    public static bool operator !=(HighInteger n1, HighInteger n2) {

        return !( n1.HighIntegerNumber == n2.HighIntegerNumber );
    }

    public static bool operator !=(HighInteger n1, int i1) {

        return n1 != new HighInteger( i1 );
    }

    public static HighInteger operator *(HighInteger n1, HighInteger n2) {

        HighInteger resultat = new HighInteger("");

        while( n2 > new HighInteger("0") ) {

            resultat = new HighInteger(resultat + n1);

            n2--;
        }

        return new HighInteger(resultat);
    }

    public static HighInteger operator *(HighInteger n1, int i1) {

        return new HighInteger( n1 * new HighInteger(i1) );
    }

    public static HighInteger operator *(int i1, HighInteger n1) {

        return new HighInteger(n1 * new HighInteger(i1));
    }

    public static bool operator <=(HighInteger n1, HighInteger n2) {

        return n1 == n2 || n1 < n2;
    }

    public static bool operator <=(HighInteger n1, int i1) {

        return n1 == i1 || n1 < i1;
    }

    public static bool operator >=(HighInteger n1, HighInteger n2) {

        return n1 == n2 || n1 > n2;
    }

    public static bool operator >=(HighInteger n1, int i1) {

        return n1 == i1 || n1 > i1;
    }

    public static HighInteger operator --(HighInteger n1) {

        return new HighInteger( n1 - 1 );
    }

    public static HighInteger operator ++(HighInteger n1) {

        return new HighInteger(n1 + 1);
    }

    // Accesseurs ===================================================================================================

    public string HighIntegerNumber { get { return number; } set { number = value; } }
}
