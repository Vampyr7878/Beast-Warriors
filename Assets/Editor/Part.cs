using System.Globalization;
using UnityEngine;

public class Part
{
    public int Color { get; private set; }

    public Matrix4x4 Matrix { get; private set; }

    public string Name { get; private set; }

    public Part(string[] words)
    {
        Color = int.Parse(words[1]);
        Vector4 column0 = new Vector4(float.Parse(words[5], CultureInfo.InvariantCulture), float.Parse(words[8], CultureInfo.InvariantCulture), float.Parse(words[11], CultureInfo.InvariantCulture), 0f);
        Vector4 column1 = new Vector4(float.Parse(words[6], CultureInfo.InvariantCulture), float.Parse(words[9], CultureInfo.InvariantCulture), float.Parse(words[12], CultureInfo.InvariantCulture), 0f);
        Vector4 column2 = new Vector4(float.Parse(words[7], CultureInfo.InvariantCulture), float.Parse(words[10], CultureInfo.InvariantCulture), float.Parse(words[13], CultureInfo.InvariantCulture), 0f);
        Vector4 column3 = new Vector4(float.Parse(words[2], CultureInfo.InvariantCulture), float.Parse(words[3], CultureInfo.InvariantCulture), float.Parse(words[4], CultureInfo.InvariantCulture), 1f);
        Matrix = new Matrix4x4(column0, column1, column2, column3);
        Name = words[14].Replace(".dat", "");
    }
}
