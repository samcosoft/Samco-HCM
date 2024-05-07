using DevExpress.Xpo.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using BotCrypt;
// ReSharper disable InconsistentNaming

namespace Samco_HCM_Shared.Classes
{
    public class HCMSettings
    {
        [JsonIgnore]
        public string? SettingDirectory { get; set; }
        private const string EncryptionKey = "SamcoHCM";

        #region Database Details

        public enum DatabaseTypes
        {
            NotSet,
            MicrosoftSQLServer,
            MicrosoftSQLLocal,
            MySql,
            Access
        }

        [JsonIgnore]
        public string? ConnectionString
        {
            get
            {
                switch (DatabaseType)
                {
                    case DatabaseTypes.MicrosoftSQLServer:
                        if (string.IsNullOrEmpty(ServerAddress) || string.IsNullOrEmpty(DatabaseUserId) || string.IsNullOrEmpty(DatabasePassword) || string.IsNullOrEmpty(DatabaseName)) return null;

                        return MSSqlConnectionProvider.GetConnectionString(ServerAddress, DatabaseUserId, DatabasePassword, DatabaseName);
                    case DatabaseTypes.MicrosoftSQLLocal:
                        if (string.IsNullOrEmpty(ServerAddress) || string.IsNullOrEmpty(DatabaseName) || string.IsNullOrEmpty(DatabaseFilePath)) return null;

                        return MSSqlConnectionProvider.GetConnectionStringWithAttachForLocalDB(ServerAddress, DatabaseName, DatabaseFilePath);
                    case DatabaseTypes.MySql:
                        if (string.IsNullOrEmpty(ServerAddress) || string.IsNullOrEmpty(DatabaseUserId) || string.IsNullOrEmpty(DatabasePassword) || string.IsNullOrEmpty(DatabaseName)) return null;

                        return MySqlConnectionProvider.GetConnectionString(ServerAddress, DatabaseUserId, DatabasePassword, DatabaseName);
                    case DatabaseTypes.Access:
                        if (string.IsNullOrEmpty(DatabaseFilePath) || string.IsNullOrEmpty(DatabasePassword)) return null;

                        return AccessConnectionProvider.GetConnectionStringACE(DatabaseFilePath, DatabasePassword);
                    case DatabaseTypes.NotSet:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                return null;
            }
        }

        [DataMember] public DatabaseTypes DatabaseType { get; set; }

        [DataMember] public string? serverAddress;

        [JsonIgnore]
        public string? ServerAddress
        {
            get => serverAddress;
            set
            {
                serverAddress = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(ServerAddress)));
            }
        }

        [DataMember]
        private string? databaseName { get; set; }

        [JsonIgnore]
        public string? DatabaseName
        {
            get => databaseName;
            set
            {
                databaseName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DatabaseName"));
            }
        }

        [DataMember]
        public string? databaseEncUserid { get; set; }

        [JsonIgnore]
        public string DatabaseUserId
        {
            get => databaseEncUserid != null ? Crypter.DecryptString(EncryptionKey, databaseEncUserid) : string.Empty;
            set
            {
                databaseEncUserid = Crypter.EncryptString(EncryptionKey, value);
                OnPropertyChanged(new PropertyChangedEventArgs("DatabaseUserId"));
            }
        }

        [DataMember]
        public string? databaseEncPass { get; set; }

        [JsonIgnore]
        public string DatabasePassword
        {
            get => databaseEncPass != null ? Crypter.DecryptString(EncryptionKey, databaseEncPass) : string.Empty;
            set
            {
                databaseEncPass = Crypter.EncryptString(EncryptionKey, value);
                OnPropertyChanged(new PropertyChangedEventArgs("DatabasePassword"));
            }
        }

        [DataMember]
        public string? databaseFilePath { get; set; }

        [JsonIgnore]
        public string? DatabaseFilePath
        {
            get => databaseFilePath;
            set
            {
                databaseFilePath = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DatabaseFilePath"));
            }
        }

        public void ResetDatabaseSettings()
        {
            DatabaseType = DatabaseTypes.NotSet;
            serverAddress = null;
            databaseName = null;
            databaseEncUserid = null;
            databaseEncPass = null;
            databaseFilePath = null;
        }

        #endregion


        #region General Information

        [DataMember] public string? UniversityName { get; set; }

        [JsonIgnore]
        public byte[]? UniversityIcon
        {
            get => System.IO.File.Exists(SettingDirectory + "\\UniIcon") ? System.IO.File.ReadAllBytes(SettingDirectory + "\\UniIcon") : null;
            set
            {
                if (value != null) System.IO.File.WriteAllBytes(SettingDirectory + "\\UniIcon", value);
            }
        }

        [DataMember] public string? ShahrestanName { get; set; }
        [DataMember] public string? MarkazName { get; set; }
        [DataMember] public string? MarkazPhone { get; set; }
        [DataMember] public string? MarkazAddress { get; set; }

        [DataMember] public string? SettingVersionString { get; set; }

        [DataMember]
        public Version? SettingVersion
        {
            get => string.IsNullOrEmpty(SettingVersionString) ? null : Version.Parse(SettingVersionString);
            set => SettingVersionString = value == null ? string.Empty : value.ToString();
        }

        [DataMember]
        public string? AppThemeName { get; set; }

        [DataMember] public bool IsClinic { get; set; }

        [DataMember] public List<string>? PersonnelRole { get; set; }

        [DataMember] public DateTime? StartShiftTime { get; set; }

        [DataMember] public DateTime? EndShiftTime { get; set; }

        #endregion

        public void Save()
        {
            SamcoSoftShared.WriteToJsonFile(SettingDirectory + "\\Settings.json", this);
        }

        public void SaveBackup()
        {
            SamcoSoftShared.WriteToJsonFile(SettingDirectory + "\\Settings.bck", this);
        }

        #region INotifyPropertyChanged members

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }

        #endregion
    }
}
