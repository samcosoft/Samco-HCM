using DevExpress.Xpo.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using BotCrypt;
using DeviceId;

// ReSharper disable InconsistentNaming

namespace Samco_HCM_Shared.Classes
{
    public class HCMSettings
    {
        [JsonIgnore]
        public string? SettingDirectory { get; set; }

        [JsonIgnore] private string EncryptionKey { get; set; }
        public string? SettingFileName { get; set; }

        public HCMSettings()
        {
            EncryptionKey = new DeviceIdBuilder()
                .AddMachineName()
                .AddMacAddress()
                .ToString();
        }

        public HCMSettings(string settingFileName)
        {
            SettingFileName = settingFileName;
            EncryptionKey = new DeviceIdBuilder()
                .AddMachineName()
                .AddMacAddress()
                .ToString();
        }
        #region Database Details

        public enum DatabaseTypes
        {
            NotSet,
            MicrosoftSQLServer,
            MicrosoftSQLLocal,
            MySql,
            Access,
            SQLite
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
                    case DatabaseTypes.SQLite:
                        return string.IsNullOrEmpty(DatabaseFilePath) ? null : SQLiteConnectionProvider.GetConnectionString(DatabaseFilePath);
                    case DatabaseTypes.NotSet:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                return null;
            }
        }

        [DataMember] public DatabaseTypes DatabaseType { get; set; }

        [DataMember] public string? serverAddress { get; set; }
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

        [DataMember] private string? databaseName { get; set; }
        [JsonIgnore]
        public string? DatabaseName
        {
            get => databaseName;
            set
            {
                databaseName = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(DatabaseName)));
            }
        }

        [DataMember] public string? databaseEncUserid { get; set; }
        [JsonIgnore]
        public string DatabaseUserId
        {
            get => databaseEncUserid != null ? Crypter.DecryptString(EncryptionKey, databaseEncUserid) : string.Empty;
            set
            {
                databaseEncUserid = Crypter.EncryptString(EncryptionKey, value);
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(DatabaseUserId)));
            }
        }

        [DataMember] public string? databaseEncPass { get; set; }
        [JsonIgnore]
        public string DatabasePassword
        {
            get => databaseEncPass != null ? Crypter.DecryptString(EncryptionKey, databaseEncPass) : string.Empty;
            set
            {
                databaseEncPass = Crypter.EncryptString(EncryptionKey, value);
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(DatabasePassword)));
            }
        }

        [DataMember] public string? databaseFilePath { get; set; }

        [JsonIgnore]
        public string? DatabaseFilePath
        {
            get => databaseFilePath;
            set
            {
                databaseFilePath = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(DatabaseFilePath)));
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
        [DataMember] public string? ApplicationLicense { get; set; }
        [DataMember] public string? SettingVersionString { get; set; }

        [DataMember]
        public Version? SettingVersion
        {
            get => string.IsNullOrEmpty(SettingVersionString) ? null : Version.Parse(SettingVersionString);
            set => SettingVersionString = value == null ? string.Empty : value.ToString();
        }

        [DataMember] public string? AppThemeName { get; set; }

        [DataMember] public bool IsClinic { get; set; }

        [DataMember] public List<string>? PersonnelRole { get; set; }

        [DataMember] public DateTime? StartShiftTime { get; set; }

        [DataMember] public DateTime? EndShiftTime { get; set; }

        #endregion

        #region Network Settings
        [JsonIgnore]
        public string? RemoteConnectionString
        {
            get
            {
                switch (DatabaseType)
                {
                    case DatabaseTypes.MicrosoftSQLServer:
                        if (string.IsNullOrEmpty(RemoteServerAddress) || string.IsNullOrEmpty(RemoteDatabaseUserId) || string.IsNullOrEmpty(RemoteDatabasePassword) || string.IsNullOrEmpty(RemoteDatabaseName)) return null;

                        return MSSqlConnectionProvider.GetConnectionString(RemoteServerAddress, RemoteDatabaseUserId, RemoteDatabasePassword, RemoteDatabaseName);
                    case DatabaseTypes.MySql:
                        if (string.IsNullOrEmpty(RemoteServerAddress) || string.IsNullOrEmpty(RemoteDatabaseUserId) || string.IsNullOrEmpty(RemoteDatabasePassword) || string.IsNullOrEmpty(RemoteDatabaseName)) return null;

                        return MySqlConnectionProvider.GetConnectionString(RemoteServerAddress, RemoteDatabaseUserId, RemoteDatabasePassword, RemoteDatabaseName);
                    case DatabaseTypes.NotSet:
                        break;
                }

                return null;
            }
        }
        [DataMember] public DatabaseTypes RemoteDatabaseType { get; set; }
        [DataMember] public string? remoteServerAddress { get; set; }

        [JsonIgnore]
        public string? RemoteServerAddress
        {
            get => remoteServerAddress;
            set
            {
                remoteServerAddress = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(RemoteServerAddress)));
            }
        }

        [DataMember] public string? remoteDatabaseName { get; set; }
        [JsonIgnore]
        public string? RemoteDatabaseName
        {
            get => remoteDatabaseName;
            set
            {
                remoteDatabaseName = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(RemoteDatabaseName)));
            }
        }

        [DataMember] public string? remoteDatabaseEncUserId { get; set; }
        [JsonIgnore]
        public string RemoteDatabaseUserId
        {
            get => remoteDatabaseEncUserId != null ? Crypter.DecryptString(EncryptionKey, remoteDatabaseEncUserId) : string.Empty;
            set
            {
                remoteDatabaseEncUserId = Crypter.EncryptString(EncryptionKey, value);
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(RemoteDatabaseUserId)));
            }
        }

        [DataMember] public string? remoteDatabaseEncPassword { get; set; }
        [JsonIgnore]
        public string RemoteDatabasePassword
        {
            get => remoteDatabaseEncPassword != null ? Crypter.DecryptString(EncryptionKey, remoteDatabaseEncPassword) : string.Empty;
            set
            {
                remoteDatabaseEncPassword = Crypter.EncryptString(EncryptionKey, value);
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(RemoteDatabasePassword)));
            }
        }

        public void ResetNetworkSettings()
        {
            RemoteDatabaseType = DatabaseTypes.NotSet;
            RemoteServerAddress = null;
            RemoteDatabaseName = null;
            remoteDatabaseEncUserId = null;
            remoteDatabaseEncPassword = null;
        }

        [DataMember] public bool ActiveClient { get; set; }

        #endregion

        #region LaboratorySettings

        [DataMember] public string? machineIdColumn { get; set; }
        [JsonIgnore]
        public string? MachineIdColumn
        {
            get => machineIdColumn;
            set
            {
                machineIdColumn = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(MachineIdColumn)));
            }
        }
        [DataMember] public string? machineNameColumn { get; set; }

        [JsonIgnore]
        public string? MachineNameColumn
        {
            get => machineNameColumn;
            set
            {
                machineNameColumn = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(MachineNameColumn)));
            }
        }

        [DataMember] public string? machineTestColumn { get; set; }

        [JsonIgnore]
        public string? MachineTestColumn
        {
            get => machineTestColumn;
            set
            {
                machineTestColumn = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(MachineTestColumn)));
            }
        }

        [DataMember] public string? machineResultColumn { get; set; }

        [JsonIgnore]
        public string? MachineResultColumn
        {
            get => machineResultColumn;
            set
            {
                machineResultColumn = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(MachineResultColumn)));
            }
        }

        #endregion

        public void Save()
        {
            SamcoSoftShared.WriteToJsonFile(SettingDirectory + $"\\{SettingFileName}.json", this);
        }

        public void SaveBackup()
        {
            SamcoSoftShared.WriteToJsonFile(SettingDirectory + $"\\{SettingFileName}.bck", this);
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
