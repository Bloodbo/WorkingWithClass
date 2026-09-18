namespace WorkingWithClass;
class Items
{
	public string nameItems = "None";
	public string description = "None";
	public int count = 0;
	public string creator = "None";
	public Items(string nameItems,string description,int count, string creator) 
	{
	  this.nameItems = nameItems;
	  this.description = description;
	  this.count = count;
	  this.creator = creator;
	}
	public void PrintItemsInfo()
	{
		Console.WriteLine($"Название сущности:{nameItems}\nОписание:{description}" +
		$"\nКоличество(масса):{count}\nСоздатель:{creator}");
	}
}