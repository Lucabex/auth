namespace auth.Models;
public class User
{
    public int Id{get; set;}
    public string UserName{get;set;}=string.Empty;
    public string HashedPassword{get;set;}=string.Empty;
}