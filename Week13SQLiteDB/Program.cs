using System.Data.Common;
using System.Data.SQLite;

ReadData(CreateConnection()); //kutsun funktsiooni valja
//InsertCustomer(CreateConnection());
//RemoveCustomer(CreateConnection());
static SQLiteConnection  CreateConnection()
{
    SQLiteConnection connection = new SQLiteConnection("Data Source = mydb.db; Version = 3; New = True; Compress = True;");

    try
    {
        connection.Open();
        Console.WriteLine("DB found.");
    }
    catch //hakkab toimima siis kui "try" block toimib
    {
        Console.WriteLine("DB not found.");
    }
    return connection;
}

static void ReadData(SQLiteConnection myConnection)
{
    Console.Clear();
    SQLiteDataReader reader; //reader on nimi objektile kuhu hakkame andmeid salvestama
    SQLiteCommand command; //see on paring

    command = myConnection.CreateCommand(); //teeme uhenduse
    command.CommandText = "SELECT rowid, * FROM Customer"; //panin paringu kokku

    reader = command.ExecuteReader(); //pane paring kaima, hakka lugema andmeid ja salvesta need readerisse

    while (reader.Read()) //kui reader leiab andmed siis ta loeb neid, all on 3 muutujat, mis andmed me tahame katte saada
    {
        string readerRowId = reader["rowid"].ToString();
        string readerSrtringFirstName = reader.GetString(1);
        string readerStringLastName = reader.GetString(2);
        string readerStringDoB = reader.GetString(3);

        Console.WriteLine($"{readerRowId}. Full name: {readerSrtringFirstName} {readerStringLastName}; DoB: {readerStringDoB}");
    }

    myConnection.Close();

}
//siin hakkame lisama andmeid
static void InsertCustomer(SQLiteConnection myConnection)
{
    SQLiteCommand command; //on valmis andmeid endasse salvestama
    string fName, lName, dob;

    Console.WriteLine("Enter first name:");
    fName = Console.ReadLine();
    Console.WriteLine("Enter last name:");
    lName = Console.ReadLine();
    Console.WriteLine("Enter date of birth (mm-dd-yyy):");
    dob = Console.ReadLine();

    command = myConnection.CreateCommand();
    command.CommandText = $"INSERT INTO customer(firstName, lastName, dateOfBirth) " +
    $"VALUES ('{fName}', '{lName}', '{dob}')";

    int rowInserted = command.ExecuteNonQuery(); //kask, mis lisab andmed tabelisse
    Console.WriteLine($"Row inserted: {rowInserted}");

    ReadData(myConnection);
}

//siin hakkame andmeid kustutama

static void RemoveCustomer(SQLiteConnection myConnection)
{
    SQLiteCommand command; //initsialiseerime kasu

    string idToDelete;
    Console.WriteLine("Enter an id to delete a customer:");
    idToDelete = Console.ReadLine();

    command = myConnection.CreateCommand(); //loome uhenduse
    command.CommandText = $"DELETE FROM customer WHERE rowid = {idToDelete}";
    int rowRemoved = command.ExecuteNonQuery();
    Console.WriteLine($"{rowRemoved} was removed from the table customer.");

    ReadData(myConnection); //kuvab uuendatud tabelit kosnoolis
}

   

