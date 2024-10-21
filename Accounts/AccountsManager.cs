using System.Text;
using LiteDB;

namespace Kino.Accounts;

public class AccountsManager
{
	public static AccountsManager Instance { get; } = new AccountsManager();
	public Account? CurrentAccount { get; set; }
	public bool IsLoggedIn => CurrentAccount != null;
	private LiteDatabase _db;
	
	public AccountsManager()
	{
		_db = new LiteDatabase("accounts.db");
	}
	
	public void AddAccount(Account acc)
	{
		ILiteCollection<Account>? col = _db.GetCollection<Account>("accounts");
		col.Insert(acc);
	}
	
	public Account CreateAccount(string login, string password, bool hasAdminPermissions)
	{
		Account acc = Account.CreateNewAccount(login, password, hasAdminPermissions);
		AddAccount(acc);
		return acc;
	}
	
	public Account? GetAccount(string login)
	{
		ILiteCollection<Account>? col = _db.GetCollection<Account>("accounts");
		return col.FindOne(x => x.Login == login);
	}
	
	public bool Login(string login, string password)
	{
		Account? acc = GetAccount(login);
		if (acc == null || acc.Password != password)
		{
			return false;
		}
		
		CurrentAccount = acc;
		LoggedIn?.Invoke(this, new LoggedInEventArgs(acc));
		return true;
	}
	
	public event EventHandler<LoggedInEventArgs> LoggedIn;
}

public class LoggedInEventArgs : EventArgs
{
	public Account Account { get; }
	
	public LoggedInEventArgs(Account acc)
	{
		Account = acc;
	}
}