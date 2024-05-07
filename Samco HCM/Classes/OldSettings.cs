using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;
// ReSharper disable InconsistentNaming
// ReSharper disable IdentifierTypo

namespace Samco_HCM.Classes;

[DataContract (Name = "MySettings", Namespace = "http://schemas.datacontract.org/2004/07/Health_Care_Management")]
internal class OldSettings
{
    public enum DatabaseTypes
    {
        MicrosoftSQLServer,
        MicrosoftSQLLocal,
        MySQL,
        Access
    }
    
    [DataMember] public DatabaseTypes DatabaseType;
    [DataMember] public string ServerAddress;
    [DataMember] public string DatabaseName;
    [DataMember] private string DatabaseEncUserid;

    public string DatabaseUserId
    {
        get => DecryptData(DatabaseEncUserid);
        set => DatabaseEncUserid = Convert.ToString(EncryptData(value));
    }

    [DataMember] private string DatabaseEncPass;

    public string DatabasePassword
    {
        get => DecryptData(DatabaseEncPass);
        set => DatabaseEncPass = Convert.ToString(EncryptData(value));
    }

    [DataMember] public string DatabaseFilePath;

    [DataMember] public string UniversityName;

    [DataMember] public string ShahrestanName;

    [DataMember] public string MarkazName;

    [DataMember] public string MarkazPhone;

    [DataMember] public string MarkazAddress;

    [DataMember] private string SettingVersionString;

    public Version SettingVersion
    {
        get => Version.Parse(SettingVersionString);
        set => SettingVersionString = Convert.ToString(value.ToString());
    }

    [DataMember] public string AppThemeName;

    [DataMember]
    public bool IsClinic;

    [DataMember] public List<string> PersonnelRole;

    [DataMember] public DateTime? StartShiftTime;

    [DataMember] public DateTime? EndShiftTime;

    private string EncryptData(string Message)
    {
        if (string.IsNullOrEmpty(Message)) { return null; }
        byte[] Results;
        var UTF8 = new UTF8Encoding();
        var HashProvider = MD5.Create();
        //Set the encryption key
        var TDESKey = HashProvider.ComputeHash("HCMSettings"u8.ToArray());
        var TDESAlgorithm = TripleDES.Create();
        TDESAlgorithm.Key = TDESKey;
        TDESAlgorithm.Mode = CipherMode.ECB;
        TDESAlgorithm.Padding = PaddingMode.PKCS7;
        var DataToEncrypt = UTF8.GetBytes(Message);
        try
        {
            var Encryptor = TDESAlgorithm.CreateEncryptor();
            Results = Encryptor.TransformFinalBlock(DataToEncrypt, 0, DataToEncrypt.Length);
        }
        finally
        {
            TDESAlgorithm.Clear();
            HashProvider.Clear();
        }
        return Convert.ToBase64String(Results);
    }

    private string DecryptData(string Message)
    {
        if (string.IsNullOrEmpty(Message)) { return null; }
        byte[] Results;
        var UTF8 = new UTF8Encoding();
        var HashProvider = MD5.Create();
        var TDESKey = HashProvider.ComputeHash("HCMSettings"u8.ToArray());
        var TDESAlgorithm = TripleDES.Create();
        TDESAlgorithm.Key = TDESKey;
        TDESAlgorithm.Mode = CipherMode.ECB;
        TDESAlgorithm.Padding = PaddingMode.PKCS7;
        var DataToDecrypt = Convert.FromBase64String(Message);
        try
        {
            var Decryptor = TDESAlgorithm.CreateDecryptor();
            Results = Decryptor.TransformFinalBlock(DataToDecrypt, 0, DataToDecrypt.Length);
        }
        finally
        {
            TDESAlgorithm.Clear();
            HashProvider.Clear();
        }
        return UTF8.GetString(Results);
    }
}