using System;
using System.Configuration;
using System.Text;
using DeployLX.Licensing.v5;

namespace Samco_HCM.Classes;

/// <summary>
/// 	Helper class to quickly add licensing to your project. 
/// </summary>
internal class LicenseHelper : IDisposable
{
    private SecureLicense _license;
    private NoLicenseException _lastError;

    /// <summary>
    /// Make sure all license resources are disposed and any pending data is persisted.
    /// </summary>
    private void Dispose()
    {
        _license?.Dispose();
        _license = null;
    }

    void IDisposable.Dispose() => Dispose();

    /// <summary>
    /// Gets a value that indicates if the current license is for a trial edition.
    /// </summary>
    public bool IsTrial => Check(false) && _license.IsTrial;

    /// <summary>
    /// Gets a value that indicates if the current license has an Activation limit and it has
    /// been activated.
    /// </summary>
    public bool IsActivated => Check(false) && _license.IsActivated;

    /// <summary>
    /// Gets a value that indicates if the current license has an Activation limit.
    /// </summary>
    public bool HasActivation => Check(false) && _license.IsActivation;

    /// <summary>
    /// Gets a reference to the last license validation error.
    /// </summary>
    public NoLicenseException LastError => _lastError;

    /// <summary>
    /// 	Check for a valid license but don't throw any exceptions if a license cannot be found.
    /// </summary>
    /// <param name = "silent">Indicates if DeployLX should validate without showing any forms to the user.</param>
    /// <returns>Returns true if a valid license was found, otherwise false.</returns>
    public bool Check(bool silent)
    {
        try
        {
            Required(silent);
            return true;
        }
        catch (NoLicenseException)
        {
            return false;
        }
    }

    /// <summary>
    /// 	Asks DeployLX to assert that the application has a valid license.
    /// </summary>
    /// <exception cref = "DeployLX.Licensing.v5.NoLicenseException">Thrown when a valid license could not be obtained. See exception ValidationRecords for details.</exception>
    public void Required()
    {
        Required(false);
    }

    /// <summary>
    /// 	Asks DeployLX to assert that the application has a valid license.
    /// </summary>
    /// <exception cref = "NoLicenseException">Thrown when a valid license could not be obtained. See exception ValidationRecords for details.</exception>
    /// <param name = "silent">Indicates if DeployLX should validate without showing any forms to the user.</param>
    public void Required(bool silent)
    {
        try
        {
            var info = new LicenseValidationRequestInfo { DontShowForms = silent }; // , .SerialNumbers = GetApplicationSerialNumbers(), .SaveExternalSerials = True}
            var license = SecureLicenseManager.Validate(this, default, info);
            if (_license is not null && !ReferenceEquals(license, _license))
            {
                _license.Dispose();
            }
            _license = license;
            _lastError = null;
        }
        // Save serial
        // If license.SerialNumber IsNot Nothing Then
        // writeSetting("General", "SerialNumber", license.SerialNumber)
        // End If
        catch (NoLicenseException ex)
        {
            _lastError = ex;
            RecordLicenseError(ex);
            throw;
        }
    }

    private string[] GetApplicationSerialNumbers()
    {
        // Dim serial = readSetting("General/SerialNumber")
        var serial = ConfigurationManager.AppSettings["SerialNumber"];
        return serial is null ? null : new[] { serial };
    }

    /// <summary>
    /// Called when a valid license could not be found when calling <see cref="Required"/> or <see cref="Check"/>.
    /// </summary>
    /// <param name="ex">Exception reported by DeployLX.</param>
    public void RecordLicenseError(NoLicenseException ex)
    {
        // warning Log license validation errors. 
    }

    public string GetLicenseEdition()
    {
        // Check for a license but don't prompt the user if the license hasn't been registered yet.
        return !Check(true) ? "Not licensed." : _license.Type;
    }

    /// <summary>
    /// 	Gets a string representation of the license ownership data.
    /// </summary>
    public string GetLicenseInfo()
    {
        var info = new StringBuilder();

        // Check for a license but don't prompt the user if the license hasn't been registered yet.
        if (!Check(true))
        {
            return "Not licensed.";
        }

        info.AppendLine(_license.Type);

        if (_license.Username is not null)
        {
            info.AppendLine(_license.Username);
        }

        if (_license.Organization is not null)
        {
            info.AppendLine(_license.Organization);
        }

        if (_license.IsUnlocked)
        {
            info.AppendLine(_license.SerialNumber);
        }

        if (_license.IsActivation && !_license.IsActivated)
        {
            info.AppendLine("Not Activated");
        }

        return info.ToString();
    }


    /// <summary>
    /// Activates the license on the current machine.
    /// </summary>
    /// <returns>
    /// Returns true if the license was activated, otherwise false.
    /// </returns>
    public bool Activate()
    {
        // Hardware Locking and the Activation Process
        // http://xheo.com/docs/dlxl/4/html/developers%20guide/licensing%20basics/hardware%20locking%20and%20the%20activation%20process.html


        if (!Check(false))
        {
            return false;
        }

        if (_license.Limits.FindLimitByType(typeof(ActivationLimit),true) is not ActivationLimit activation)
        {
            return false;
        }

        if (activation.HasActivated)
        {
            return true;
        }

        try
        {
            var info = new LicenseValidationRequestInfo();
            // #If DEBUG Then
            // ' See http://xheo.com/knowledge-base/deploylx/licensing/enabling-developer-mode
            // info.DeveloperMode = True
            // #End If

            var newLicense = SecureLicenseManager.ShowForm(activation, default, this, default, info, default);
            if (newLicense is not null && !ReferenceEquals(newLicense, _license))
            {
                _license?.Dispose();
                _license = newLicense;
            }

            return true;
        }
        catch (SecureLicenseException)
        {
            // TODO: Decide how to handle failures here. Might parse exception or just
            // ignore and assume DeployLX error reporting to user was enough.
        }

        return false;
    }

    /// <summary>
    /// Deactivates the license from the current machine.
    /// </summary>
    /// <returns>
    /// Returns true if the license was deactivated, otherwise false.
    /// </returns>
    public bool Deactivate()
    {
        if (_license is null)
        {
            return false;
        }

        var info = new LicenseValidationRequestInfo();
        // #If DEBUG Then
        // ' See http://xheo.com/knowledge-base/deploylx/licensing/enabling-developer-mode
        // info.DeveloperMode = True
        // #End If

        return SecureLicenseManager.Deactivate(_license, this, default, info);
    }

    /// <summary>
    /// Shows the registration form to the user so they can enter a new serial number. Might be used
    /// to upgrade to a better edition or fix a wrongly entered serial number.
    /// </summary>
    /// <returns>
    /// Returns true if the registration unlocked a new license, otherwise false.
    /// </returns>
    public bool ReShowRegistrationForm()
    {
        // Need to validate the license first. 
        if (_license is null)
        {
            return false;
        }

        var info = new LicenseValidationRequestInfo { SaveExternalSerials = true };
        // #If DEBUG Then
        // ' See http://xheo.com/knowledge-base/deploylx/licensing/enabling-developer-mode
        // info.DeveloperMode = True
        // #End If

        var newLicense = _license.ShowRegistrationForm(this, default, info);
        if (newLicense is not null && !ReferenceEquals(newLicense, _license))
        {
            _license.Dispose();
            _license = newLicense;
            return true;
        }
        return false;
    }

}