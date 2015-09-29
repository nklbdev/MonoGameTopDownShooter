namespace XTiled
{
    public struct Property
    {
        public string Value;
        public float? AsSingle;
        public int? AsInt32;
        public bool? AsBoolean;

        public static Property Create(string value)
        {
            bool result1;
            float result2;
            int result3;
            return new Property
            {
                Value = value,
                AsBoolean = !bool.TryParse(value, out result1) ? new bool?() : result1,
                AsSingle = !float.TryParse(value, out result2) ? new float?() : result2,
                AsInt32 = !int.TryParse(value, out result3) ? new int?() : result3
            };
        }
    }
}