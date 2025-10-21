using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Nothing.Model;


public partial class UserTable : BaseClass
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int UserId { get; set; }

    private string _login;
    public string Login
    {
        get => _login;
        set { _login = value; OnPropertyChanged(); }
    }

    private string _password;
    public string Password
    {
        get => _password;
        set { _password = value; OnPropertyChanged(); }
    }
}

//public partial class UserTable
//{
//    public int UserId { get; set; }

//    public string Login { get; set; } = null!;

//    public string Password { get; set; } = null!;
//}
