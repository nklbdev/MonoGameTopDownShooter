namespace XTiled
{
    /// <summary>
    /// Represents a custom property value
    /// </summary>
    public struct Property
    {
        /// <summary>
        /// Raw String value of the propery
        /// </summary>
        public string Value;
        /// <summary>
        /// Value converted to a float, null if conversion failed
        /// </summary>
        public float? AsSingle;
        /// <summary>
        /// Value converted to an int, null if conversion failed
        /// </summary>
        public int? AsInt32;
        /// <summary>
        /// Value converted to a boolean, null if conversion failed
        /// </summary>
        public bool? AsBoolean;

        /// <summary>
        /// Creates a property from a raw string value
        /// </summary>
        /// <param name="value">Value of the property</param>
        public static Property Create(string value)
        {
            var p = new Property { Value = value };

            bool testBool;
            p.AsBoolean = bool.TryParse(value, out testBool) ? (bool?) testBool : null;

            float testSingle;
            p.AsSingle = float.TryParse(value, out testSingle) ? (float?) testSingle : null;

            int testInt;
            p.AsInt32 = int.TryParse(value, out testInt) ? (int?) testInt : null;

            return p;
        }
    }
}