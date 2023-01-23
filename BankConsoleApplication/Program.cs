using BankConsoleApplication;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Threading;

ObservableCollection<Account> accounts = new ObservableCollection<Account>();

Random randomNumber = new Random();

string directoryPath = "C:\\BankAccounts";
string filePath = "C:\\BankAccounts\\Accounts.json";

string currentAccountNumber = "";
int currentAccountBalance = 0;

FileActions();
MenuActions();

void WriteAccounts()
{
    using (StreamWriter streamWriter = new StreamWriter(filePath, false))
    {
        foreach (var account in accounts)
        {
            string writeAccount = JsonSerializer.Serialize(account);
            streamWriter.WriteLine($"{writeAccount}");
        }
    }
}

void ReadAccounts()
{
    using (StreamReader streamReader = new StreamReader(filePath))
    {
        while (!streamReader.EndOfStream)
        {
            string row = streamReader.ReadLine();
            Account account = JsonSerializer.Deserialize<Account>(row);
            accounts.Add(account);
        }
    }
}

void MenuActions()
{
    try
    {
        Console.Clear();

        Console.WriteLine("1. Создать новый аккаунт");
        Console.WriteLine("2. Войти в существующий аккаунт");

        int menuActionNumber = int.Parse(Console.ReadLine());

        switch (menuActionNumber)
        {
            case 1:
                Console.Clear();
                CreateAccountActions();
                break;
            case 2:
                Console.Clear();
                EnterAccountActions();
                break;
        }
    }
    catch(Exception exception)
    {
        Console.Clear();
        Console.WriteLine($"Ошибка - {exception.Message}");
        Thread.Sleep(1000);
        MenuActions();
    }
}

void IsAccountFounded(string existingAccountNumber)
{
    bool isAccountFounded = false;

    for (int i = 0; i < accounts.Count; i++)
    {
        if (existingAccountNumber == accounts[i].AccountNumber)
        {
            isAccountFounded = true;

            currentAccountNumber = existingAccountNumber;
            currentAccountBalance = accounts[i].AccountBalance;
        }
    }

    if (isAccountFounded)
    {
        AccountActions();
    }
    else
    {
        Console.WriteLine("Номер счёта не найден!");
        EnterAccountActions();
    }
}

void CreateAccountActions()
{
    string newAccountNumber = randomNumber.Next(111111, 999999).ToString();

    Account account = new Account { AccountNumber = newAccountNumber, AccountBalance = 0 };
    accounts.Add(account);

    currentAccountNumber = newAccountNumber;

    WriteAccounts();
    AccountActions();
}

void EnterAccountActions()
{
    try
    {
        Console.WriteLine("Укажите номер счёта");

        string existingAccountNumber = Console.ReadLine();
        IsAccountFounded(existingAccountNumber);
    }
    catch (Exception exception)
    {
        Console.Clear();
        Console.WriteLine($"Ошибка - {exception.Message}");
        Thread.Sleep(1000);
        MenuActions();
    }
}

void AccountActions()
{
    try
    {
        Console.Clear();

        Console.WriteLine($"Номер счёта: {currentAccountNumber}");
        Console.WriteLine($"Баланс: {currentAccountBalance} руб.");

        Console.WriteLine("1. Внести деньги на счёт");
        Console.WriteLine("2. Снять деньги со счёта");
        Console.WriteLine("3. Отправить деньги на другой счёт");
        Console.WriteLine("4. Выйти");

        int actionNumber = int.Parse(Console.ReadLine());

        switch (actionNumber)
        {
            case 1:
                TakeOnMoney();
                break;
            case 2:
                TakeOffMoney();
                break;
            case 3:
                SendMoney();
                break;
            case 4:
                Console.Clear();
                MenuActions();
                break;
        }
    }
    catch (Exception exception)
    {
        Console.Clear();
        Console.WriteLine($"Ошибка - {exception.Message}");
        Thread.Sleep(1000);
        AccountActions();
    }
}

void TakeOnMoney()
{
    try
    {
        Console.Clear();

        Console.WriteLine("Укажите сумму для внесения");
        int money = int.Parse(Console.ReadLine());

        currentAccountBalance += money;
        SaveAccountChanges();
        WriteAccounts();
        MenuActions();
    }
    catch (Exception exception)
    {
        Console.Clear();
        Console.WriteLine($"Ошибка - {exception.Message}");
        Thread.Sleep(1000);
        AccountActions();
    }
}

void TakeOffMoney()
{
    try
    {
        Console.Clear();

        Console.WriteLine("Укажите сумму для снятия");
        int money = int.Parse(Console.ReadLine());

        currentAccountBalance -= money;
        SaveAccountChanges();
        WriteAccounts();
        MenuActions();
    }
    catch (Exception exception)
    {
        Console.Clear();
        Console.WriteLine($"Ошибка - {exception.Message}");
        Thread.Sleep(1000);
        AccountActions();
    }
}

void SendMoney()
{
    try
    {
        Console.Clear();

        Console.WriteLine("Укажите номер счёта получателя");
        string accountNumber = Console.ReadLine();

        Console.WriteLine("Укажите сумму для отправки");
        int money = int.Parse(Console.ReadLine());

        for (int i = 0; i < accounts.Count; i++)
        {
            if (accountNumber == accounts[i].AccountNumber)
            {
                currentAccountBalance -= money;
                accounts[i].AccountBalance += money;

                SaveAccountChanges();
                WriteAccounts();
                MenuActions();
            }
        }
    }
    catch (Exception exception)
    {
        Console.Clear();
        Console.WriteLine($"Ошибка - {exception.Message}");
        Thread.Sleep(1000);
        AccountActions();
    }
}

void SaveAccountChanges()
{
    for (int i = 0; i < accounts.Count; i++)
    {
        if (currentAccountNumber == accounts[i].AccountNumber)
        {
            accounts[i].AccountNumber = currentAccountNumber;
            accounts[i].AccountBalance = currentAccountBalance;
        }
    }
}

void FileActions()
{
    if (Directory.Exists(directoryPath))
    {
        if (File.Exists(filePath))
        {
            ReadAccounts();
        }

        else
        {
            File.Create(filePath);
        }
    }

    else
    {
        Directory.CreateDirectory(directoryPath);
    }
}
