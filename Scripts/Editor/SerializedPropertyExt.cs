using UnityEditor;

public static class SerializedPropertyExt
{
    public static void SwapArrayElements(this SerializedProperty property, int i1, int i2)
    {
        var largest = i1 > i2 ? i1 : i2;
        var smallest = i1 < i2 ? i1 : i2;
        property.MoveArrayElement(smallest, largest);
        property.MoveArrayElement(largest - 1, smallest);
    }
}