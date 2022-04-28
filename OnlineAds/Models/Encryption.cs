using System;

namespace OnlineAds.Models
{
    public class Encryption
    {
        public string Encode(string password)
        {
            try
            {
                byte[] EncDataByte = new byte[password.Length];
                EncDataByte = System.Text.Encoding.UTF8.GetBytes(password);
                string EncryptedData = Convert.ToBase64String(EncDataByte);
                return EncryptedData;
            }
            catch(Exception ex)
            {
                throw new System.Exception("Error in Encode: " + ex.Message); 
            }
        }
    }
}
