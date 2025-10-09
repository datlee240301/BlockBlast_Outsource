// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("f2vZtCdwJrKZwBOvswbH1S/puS63UniTnC3LY4cxr52OJ2JaRpFLGYqDLwGvkHBMr91+oaDbHC77CqfharNpZmL+1xvTQWVFPq/SmQpSpuyv/KB9ZYkIPHyqzSubJ09blf2MKrMBgqGzjoWKqQXLBXSOgoKChoOAkJU4g1bxVKVg8vjTV7lE4Ygh8ixUd9V8YnWdzzHDCnexbsMhimT9MgGCjIOzAYKJgQGCgoMvpxIzqEmCeZGpLd0O+7bLX2HGfOOMN/ZSRkGC46VaRWJwBHIOVuEKbisS/TSc0Le7BR6CNL6cs3jnjcDtvI4LKk7zRwaCeKYSimLJ/jiaRyUmeR8cri2WebVnryBmTl6v/+tbgh0MULyrL+ZL01ApOxY5iIGAgoOC");
        private static int[] order = new int[] { 13,4,12,7,5,13,8,8,13,13,11,11,13,13,14 };
        private static int key = 131;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
