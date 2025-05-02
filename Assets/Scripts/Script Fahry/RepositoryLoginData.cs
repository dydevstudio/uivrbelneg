using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Repository Login Data", menuName = "Repository Login Data")]
public class RepositoryLoginData : ScriptableObject
{
    public int id;
    public string username;
    public string nama;
    public string user_code;
    public string status_login;
    public int count_login;
    public string login_date;
    public string role;
    public string rank;
    public string AppLicense;
    public string ModLicense;
    public string AppSerial;
    public string ModSerial;
}
