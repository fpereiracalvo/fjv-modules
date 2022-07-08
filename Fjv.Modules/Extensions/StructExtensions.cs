using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace Fjv.Modules.Extensions
{
    //taken from https://stackoverflow.com/questions/3278827/how-to-convert-a-structure-to-a-byte-array-in-c

    public static class StructExtensions
    {
        public static byte[] ToBytes<T>(this T obj)
            where T : struct
        {
            int size = Marshal.SizeOf(typeof(T));
            byte[] arr = new byte[size];

            IntPtr ptr = Marshal.AllocHGlobal(size);

            Marshal.StructureToPtr(obj, ptr, true);
            Marshal.Copy(ptr, arr, 0, size);
            Marshal.FreeHGlobal(ptr);

            return arr;
        }

        public static T ToObject<T>(this byte[] value)
            where T : struct
        {
            int size = Marshal.SizeOf(typeof(T));

            IntPtr ptr = Marshal.AllocHGlobal(size);

            Marshal.Copy(value, 0, ptr, size);
            var obj = (T)Marshal.PtrToStructure(ptr, typeof(T));
            Marshal.FreeHGlobal(ptr);

            return obj;
        }

        public static List<T> GetListFrom<T>(this byte[] input)
            where T : struct
        {
            var list = new List<T>();
            var size = Marshal.SizeOf(typeof(T));

            var pages = input.Length / size;

            return Enumerable.Range(0, pages).Select(s=>input.Skip(s*size).Take(size).ToArray().ToObject<T>()).ToList();
        }
    }
}