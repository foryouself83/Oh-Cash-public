using System.Runtime.InteropServices;
using System.Security;

namespace PopupCash.Main.Extensions
{
    /// <summary>
    /// SecureStringExtensions
    /// </summary>
    public static class SecureStringExtensions
    {
        /// <summary>
        /// Converts a secured string to an unsecured string.
        /// </summary>
        public static string ToUnsecuredString(this SecureString secureString)
        {
            // copy&paste from the internal System.Net.UnsafeNclNativeMethods
            IntPtr bstrPtr = IntPtr.Zero;
            if (secureString != null)
            {
                if (secureString.Length != 0)
                {
                    try
                    {
                        bstrPtr = Marshal.SecureStringToBSTR(secureString);
                        return Marshal.PtrToStringBSTR(bstrPtr);
                    }
                    finally
                    {
                        if (bstrPtr != IntPtr.Zero)
                            Marshal.ZeroFreeBSTR(bstrPtr);
                    }
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// Copies the existing instance of a secure string into the destination, clearing the destination beforehand.
        /// </summary>
        public static void CopyInto(this SecureString source, SecureString destination)
        {
            destination.Clear();
            foreach (var chr in source.ToUnsecuredString())
            {
                destination.AppendChar(chr);
            }
        }

        /// <summary>
        /// Converts an unsecured string to a secured string.
        /// </summary>
        public static SecureString ToSecuredString(this string plainString)
        {
            if (string.IsNullOrEmpty(plainString))
            {
                return new SecureString();
            }

            SecureString secure = new SecureString();
            foreach (char c in plainString)
            {
                secure.AppendChar(c);
            }
            return secure;
        }

        public static bool SecureStringEqual(SecureString secureString1, SecureString secureString2)
        {
            if (secureString1 == null)
            {
                throw new ArgumentNullException("s1");
            }
            if (secureString2 == null)
            {
                throw new ArgumentNullException("s2");
            }

            if (secureString1.Length != secureString2.Length)
            {
                return false;
            }

            IntPtr ss_bstr1_ptr = IntPtr.Zero;
            IntPtr ss_bstr2_ptr = IntPtr.Zero;

            try
            {
                ss_bstr1_ptr = Marshal.SecureStringToBSTR(secureString1);
                ss_bstr2_ptr = Marshal.SecureStringToBSTR(secureString2);

                String str1 = Marshal.PtrToStringBSTR(ss_bstr1_ptr);
                String str2 = Marshal.PtrToStringBSTR(ss_bstr2_ptr);

                return str1.Equals(str2);
            }
            finally
            {
                if (ss_bstr1_ptr != IntPtr.Zero)
                {
                    Marshal.ZeroFreeBSTR(ss_bstr1_ptr);
                }

                if (ss_bstr2_ptr != IntPtr.Zero)
                {
                    Marshal.ZeroFreeBSTR(ss_bstr2_ptr);
                }
            }
        }
    }
}
