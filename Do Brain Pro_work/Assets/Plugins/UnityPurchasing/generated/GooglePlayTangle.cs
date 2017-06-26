#if UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE_OSX || UNITY_TVOS
// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("vlvzyJ8X9QxP4akHBbe6tg0w41hoFEXNdx1pFV5QvKNTv+oTtkBRwzTa8Z5Un5XBI7IwhDTB4WMzaot5eMpJanhFTkFizgDOv0VJSUlNSEv0pPNUWceUInz9FRotlFs3WpzGXV9HH6r+SbrQLDfth8zrB3MLMlGj8hMvg5eN6O4gYmAcADyQzQx0lbq2IibKzI2ExO/ZncEK8i/sGffYJz2VS5bTgVYlZwugl6/9QTZANR07pG9l4WM1OVQUOXM0qa96yIbO26zKSUdIeMpJQkrKSUlIn390o6OImAGusrhf18DU4GqGluefKKUFPR+Z4bGGH+nnN9WCnh9fC7lA1LPJp2KBW68+hYqXLmZsDxJQd7Y2/F0+p3j0sjPLCfZC2UpLSUhJ");
        private static int[] order = new int[] { 11,8,12,11,9,11,12,13,12,11,12,11,12,13,14 };
        private static int key = 72;

        public static byte[] Data() {
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
#endif
