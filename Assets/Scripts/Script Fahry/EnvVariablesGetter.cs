using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvVariablesGetter
{
    public void GetLoginData(RepositoryLoginData _repositoryLogin)
    {
        if(Environment.GetEnvironmentVariable("Id")!= null)
            _repositoryLogin.id = int.Parse(Environment.GetEnvironmentVariable("Id") ?? string.Empty);
        _repositoryLogin.nama = Environment.GetEnvironmentVariable("Nama");
        _repositoryLogin.username = Environment.GetEnvironmentVariable("Username");
        _repositoryLogin.user_code = Environment.GetEnvironmentVariable("UserCode");
        if(Environment.GetEnvironmentVariable("StatusLogin")!= null)
            _repositoryLogin.status_login = (Environment.GetEnvironmentVariable("StatusLogin") ?? string.Empty);
        if(Environment.GetEnvironmentVariable("CountLogin")!= null)
            _repositoryLogin.count_login = int.Parse(Environment.GetEnvironmentVariable("CountLogin") ?? string.Empty);
        _repositoryLogin.login_date = Environment.GetEnvironmentVariable("LoginDate");
        _repositoryLogin.role = Environment.GetEnvironmentVariable("Role");
        _repositoryLogin.rank = Environment.GetEnvironmentVariable("Rank");
        _repositoryLogin.AppLicense = Environment.GetEnvironmentVariable("AppLicense");
        _repositoryLogin.ModLicense = Environment.GetEnvironmentVariable("ModLicense");
        _repositoryLogin.AppSerial = Environment.GetEnvironmentVariable("AppSerial");
        _repositoryLogin.ModSerial = Environment.GetEnvironmentVariable("ModSerial");
    }
}
