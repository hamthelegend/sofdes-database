using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SofdesDatabase;

[Table("Users")]
public class User
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Name { get; set; }
    public string Gender { get; set; }
}

public static class UserUtil
{
    public static bool IsValidEmail(this string email)
    {
        return new Regex(@"^[\w.]{1,64}@(\w+\.){1,253}\w{2,63}$").IsMatch(email);
    }
    public static bool IsValidName(this string name)
    {
        return new Regex(@"^[a-zA-Z. ]+$").IsMatch(name);
    }
}

public class UsersContext : DbContext
{
    public DbSet<User> Users { get; set; }

    public string DbPath { get; }

    public UsersContext()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = System.IO.Path.Join(path, "users.db");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");

}