using System.Text;

namespace Kino.Accounts;

public class Account
{
	public Guid Id { get; set; }
	public string Login { get; set; }
	public string Password { get; set; }
	public bool HasAdminPermissions { get; set; }
	
	public static Account CreateNewAccount(string login, string password, bool hasAdminPermissions)
	{
		Account acc = new Account();
		acc.Id = Guid.NewGuid();
		acc.Login = login;
		acc.Password = password;
		acc.HasAdminPermissions = hasAdminPermissions;
		return acc;
	}
}