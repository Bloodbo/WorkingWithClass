namespace WorkingWithClass;
class Items
{
	public string nameItem = "None";
	public string description = "None";
	public int amountItems = 0;
	public string creatorItem = "None";

	// конструктор класса:
	public Items(string nameItem, string description, int amountItems, string creatorItem)
	{
		this.nameItem = nameItem;
		this.description = description;
		this.amountItems = amountItems;
		this.creatorItem = creatorItem;
	}

	public void PrintItemsInfo()
	{
		Console.WriteLine($"Название сущности:{nameItem}\nОписание:{description}" +
		$"\nКоличество(масса):{amountItems}\nСоздатель:{creatorItem}");
	}
}